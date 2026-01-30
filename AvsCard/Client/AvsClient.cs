using AvsCard.Helpers;
using AvsCard.RequestDto;
using AvsCard.ResponseDto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AvsCard.Client;

/// <summary>
/// Provides functionality for sending AVS XML requests and receiving typed AVS responses.
/// Handles request validation, XML serialization, HTTP transport, and response deserialization.
/// Includes structured logging for diagnostics.
/// </summary>
public class AvsClient
{
    #region Private Fields

    private readonly HttpClient _httpClient;

    private readonly string _endpoint;

    private readonly ILogger _logger;

    #endregion Private Fields

    #region Private Methods

    /// <summary>
    /// Validates the AVS request before sending it.
    /// Ensures TXID is present and PAN is valid when provided.
    /// </summary>
    private static void ValidateRequest(AvsRequestBase request)
    {
        if (string.IsNullOrWhiteSpace(request.TxId))
            throw new ArgumentException("TXID is required and must be unique.");

        if (request.Card?.Pan is { } pan)
            AvsPanValidator.ValidatePan(pan);
    }

    /// <summary>
    /// Serializes an AVS request object into an ISO-8859-1 encoded XML string.
    /// </summary>
    private static string SerializeToXml(AvsRequestBase obj)
    {
        var type = obj.GetType();
        var serializer = new XmlSerializer(type);

        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.GetEncoding("ISO-8859-1"),
            Indent = false,
            OmitXmlDeclaration = false
        };

        using var sw = new StringWriterWithEncoding(Encoding.GetEncoding("ISO-8859-1"));
        using var writer = XmlWriter.Create(sw, settings);
        serializer.Serialize(writer, obj, ns);

        return sw.ToString();
    }

    /// <summary>
    /// Deserializes an AVS XML response into the correct response DTO
    /// based on the TYPE attribute in the XML root element.
    /// </summary>
    private static AvsResponseBase DeserializeResponse(string xml)
    {
        var doc = XDocument.Parse(xml);
        var type = doc.Root?.Attribute("TYPE")?.Value?.ToUpperInvariant()
                   ?? throw new InvalidOperationException("Missing TYPE attribute in AVS response.");

        Type targetType = type switch
        {
            "BALANCE" => typeof(AvsBalanceResponse),
            "REDEEM" => typeof(AvsRedeemResponse),
            "REFUND" => typeof(AvsRefundResponse),
            "CANCEL" => typeof(AvsCancelResponse),
            _ => throw new InvalidOperationException($"Unknown AVS response type: {type}")
        };

        var serializer = new XmlSerializer(targetType);
        using var sr = new StringReader(xml);
        return (AvsResponseBase)serializer.Deserialize(sr)!;
    }

    #endregion Private Methods

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="AvsClient"/>.
    /// If no logger is provided, a <see cref="NullLogger"/> is used.
    /// </summary>
    public AvsClient(HttpClient? httpClient, string endpoint, ILogger? logger = null)
    {
        _httpClient = httpClient ?? new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(AvsConstants.TimeoutSeconds)
        };

        if (httpClient != null)
            _httpClient.Timeout = TimeSpan.FromSeconds(AvsConstants.TimeoutSeconds);

        _endpoint = endpoint;
        _logger = logger ?? NullLogger.Instance;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Synchronous wrapper for <see cref="SendAsync"/>.
    /// </summary>
    public AvsResponseBase Send(AvsRequestBase request)
    {
        return SendAsync(request).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends an AVS XML request asynchronously, handles HTTP transport errors,
    /// and returns a strongly typed AVS response object.
    /// Includes structured logging for diagnostics.
    /// </summary>
    public async Task<AvsResponseBase> SendAsync(AvsRequestBase request, CancellationToken ct = default)
    {
        _logger.LogInformation("Sending AVS request TYPE={Type}, TXID={TxId}", request.Type, request.TxId);

        ValidateRequest(request);

        var xml = SerializeToXml(request);
        _logger.LogDebug("Serialized AVS request XML:\n{Xml}", xml);

        using var content = new StringContent(
            xml,
            Encoding.GetEncoding("ISO-8859-1"),
            "text/xml"
        );

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsync(_endpoint, content, ct);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "HTTP timeout while sending AVS request TXID={TxId}", request.TxId);
            return new AvsHttpCommonErrorResponse((int)AvsResultCode.HttpTimeout, "HTTP timeout while sending request");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error while sending AVS request TXID={TxId}", request.TxId);
            return new AvsHttpCommonErrorResponse((int)AvsResultCode.HttpNetworkError, "Network error while sending request");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while sending AVS request TXID={TxId}", request.TxId);
            return new AvsHttpCommonErrorResponse((int)AvsResultCode.HttpUnexpectedError, "Unexpected error while sending request");
        }

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP status code error: {StatusCode} for TXID={TxId}", response.StatusCode, request.TxId);
            return new AvsHttpCommonErrorResponse((int)AvsResultCode.HttpNetworkError, $"HTTP status code error: {response.StatusCode}");
        }

        string responseXml;

        try
        {
            responseXml = await response.Content.ReadAsStringAsync(ct);
            _logger.LogDebug("Received AVS response XML:\n{Xml}", responseXml);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read AVS HTTP response content TXID={TxId}", request.TxId);
            return new AvsHttpCommonErrorResponse((int)AvsResultCode.HttpUnexpectedError, "Failed to read HTTP response content");
        }

        try
        {
            var parsed = DeserializeResponse(responseXml);
            _logger.LogInformation("AVS response parsed successfully TYPE={Type}, RESULT={Result}", parsed.Type, parsed.Result);
            return parsed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse AVS XML response TXID={TxId}", request.TxId);
            return new AvsHttpCommonErrorResponse((int)AvsResultCode.UnknownError, "Failed to parse AVS XML response");
        }
    }

    #endregion Public Methods
}