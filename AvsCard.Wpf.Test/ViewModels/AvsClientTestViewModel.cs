using AvsCard.Client;
using AvsCard.Helpers;
using AvsCard.RequestDto;
using AvsCard.ResponseDto;
using AvsCard.Wpf.Test.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace AvsCard.Wpf.Test.ViewModels
{
    /// <summary>
    /// ViewModel for the AVS test client.
    /// Handles configuration loading, AVS operations, UI state, and logging.
    /// </summary>
    public class AvsClientTestViewModel : BindableBase
    {
        #region Private Fields

        private readonly ILogger<AvsClientTestViewModel> _logger;

        private string _endpoint = AvsConstants.TestUrl;

        private string _username = "test";

        private string _password = "";

        private string _terminalId = "test";

        private string _pan = "636453";

        private string _txId = $"TEST_{DateTime.Now:yyyyMMddHHmmssfff}";

        private string _txRef = "";

        private int _amountCents = 1000;

        private bool _useModalDialog = true;

        private bool _useOverlay = false;

        private bool _isBusyOverlay;

        private RichTextBox? _output;

        #endregion Private Fields

        #region Private Methods

        /// <summary>
        /// Executes an AVS transaction asynchronously and updates UI state.
        /// </summary>
        private async Task ExecuteAsync(AvsTransactionType type)
        {
            _logger.LogInformation("Executing AVS operation: {Operation}", type);

            if (!Validate(type, out var err))
            {
                _logger.LogWarning("Validation failed for {Operation}: {Error}", type, err);
                Append($"[VALIDATION ERROR] {err}\n");
                return;
            }

            try
            {
                if (UseModalDialog)
                    ShowWaitRequested?.Invoke(this, EventArgs.Empty);

                if (UseOverlay)
                    IsBusyOverlay = true;

                _logger.LogDebug("Creating AVS client for endpoint {Endpoint}", Endpoint);
                var client = new AvsClient(new HttpClient(), Endpoint, _logger);

                var req = CreateRequest(type);
                _logger.LogInformation("Sending AVS request: {Type}, TxId={TxId}, Pan={Pan}", type, TxId, Pan);

                var resp = await Task.Run(() => client.Send(req));

                _logger.LogInformation("Received AVS response: {Type}, Result={Result}", resp.Type, resp.Result);
                AppendResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during AVS operation {Operation}", type);
                Append($"[EXCEPTION] {ex.Message}\n");
            }
            finally
            {
                if (UseOverlay)
                    IsBusyOverlay = false;

                if (UseModalDialog)
                    HideWaitRequested?.Invoke(this, EventArgs.Empty);

                _logger.LogInformation("Finished AVS operation: {Operation}", type);
            }
        }

        /// <summary>
        /// Creates an AVS request object based on the transaction type.
        /// </summary>
        private AvsRequestBase CreateRequest(AvsTransactionType type)
        {
            return type switch
            {
                AvsTransactionType.BALANCE => AvsRequestFactory.CreateBalance(Username, Password, TerminalId, TxId, Pan),
                AvsTransactionType.REDEEM => AvsRequestFactory.CreateRedeem(Username, Password, TerminalId, TxId, Pan, AmountCents),
                AvsTransactionType.REFUND => AvsRequestFactory.CreateRefund(Username, Password, TerminalId, TxId, Pan, AmountCents),
                AvsTransactionType.CANCEL => AvsRequestFactory.CreateCancel(Username, Password, TerminalId, TxId, TxRef, Pan, AmountCents),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        /// <summary>
        /// Validates user input before sending a request.
        /// </summary>
        private bool Validate(AvsTransactionType type, out string error)
        {
            if (string.IsNullOrWhiteSpace(Endpoint)) { error = "Endpoint required"; return false; }
            if (string.IsNullOrWhiteSpace(Username)) { error = "Username required"; return false; }
            if (string.IsNullOrWhiteSpace(Password)) { error = "Password required"; return false; }
            if (string.IsNullOrWhiteSpace(TerminalId)) { error = "TerminalId required"; return false; }
            if (string.IsNullOrWhiteSpace(Pan)) { error = "PAN required"; return false; }
            if (string.IsNullOrWhiteSpace(TxId)) { error = "TXID required"; return false; }

            if (type != AvsTransactionType.BALANCE && AmountCents <= 0)
            {
                error = "Amount must be > 0";
                return false;
            }

            if (type == AvsTransactionType.CANCEL && string.IsNullOrWhiteSpace(TxRef))
            {
                error = "TXREF required for CANCEL";
                return false;
            }

            error = "";
            return true;
        }

        /// <summary>
        /// Appends a line of text to the output RichTextBox.
        /// </summary>
        private void Append(string text)
        {
            if (_output == null) return;
            _output.Document.Blocks.Add(new Paragraph(new Run(text)));
            _output.ScrollToEnd();
        }

        /// <summary>
        /// Appends a formatted AVS response to the output.
        /// </summary>
        private void AppendResponse(AvsResponseBase r)
        {
            Append($"=== RESPONSE ({r.Type}) ===");
            Append($"RESULT: {r.Result}");
            Append($"RESULTTEXT: {r.ResultText}");
            Append($"INTERNAL: {r.InternalResultText}");
            Append($"TXID: {r.TxId}");
            Append($"SERVERDATETIME: {r.ServerDateTime}");
            Append("");

            if (r.Card != null)
            {
                Append("CARD:");
                Append($"  PAN: {r.Card.Pan}");
                Append($"  EXPIRY_YEAR: {r.Card.ExpiryYear}");
                Append($"  EXPIRY_MONTH: {r.Card.ExpiryMonth}");
                Append($"  EXPIRY_DATE: {r.Card.ExpiryDate}");
                Append("");
            }

            switch (r)
            {
                case AvsBalanceResponse b: AppendBalance(b.Balance); break;
                case AvsRedeemResponse d: AppendBalance(d.Balance); Append($"AID: {d.Aid}"); break;
                case AvsRefundResponse f: AppendBalance(f.Balance); Append($"AID: {f.Aid}"); break;
                case AvsCancelResponse c: AppendBalance(c.Balance); Append($"AID: {c.Aid}"); Append($"TXREF: {c.TxRef}"); break;
            }

            Append("");
        }

        /// <summary>
        /// Appends balance information to the output.
        /// </summary>
        private void AppendBalance(AvsBalanceInfo? info)
        {
            if (info == null) return;

            var c = info.Currency;
            Append("BALANCE:");
            Append($"  ISOCODE: {c.IsoCode}");
            Append($"  BALANCE: {c.Balance}");
            Append($"  BALANCE_BEFORE: {c.BalanceBefore}");
            Append($"  MIN: {c.Min}");
            Append($"  MAX: {c.Max}");
            Append("");
        }

        /// <summary>
        /// Loads configuration values from appsettings.test.json.
        /// </summary>
        private void LoadConfiguration()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: false);

                var config = builder.Build();
                var avs = config.GetSection("Avs");

                if (!avs.Exists())
                {
                    _logger.LogWarning("AVS configuration section not found in appsettings.test.json");
                    return;
                }

                Endpoint = avs["Endpoint"] ?? Endpoint;
                Username = avs["Username"] ?? Username;
                Password = avs["Password"] ?? Password;
                TerminalId = avs["TerminalId"] ?? TerminalId;

                var testPan = avs["TestPan"];
                if (!string.IsNullOrWhiteSpace(testPan))
                    Pan = testPan;

                _logger.LogInformation("AVS configuration loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load configuration");
                Append($"[CONFIG ERROR] {ex.Message}\n");
            }
        }

        #endregion Private Methods

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AvsClientTestViewModel"/> class.
        /// Loads configuration, initializes logging, and sets up commands.
        /// </summary>
        public AvsClientTestViewModel()
        {
            var factory = LoggerFactoryBuilder.Create();
            _logger = factory.CreateLogger<AvsClientTestViewModel>();

            LoadConfiguration();
            _logger.LogInformation("Configuration loaded. Endpoint={Endpoint}, Username={Username}, TerminalId={TerminalId}", Endpoint, Username, TerminalId);

            BalanceCommand = new DelegateCommand(async () => await ExecuteAsync(AvsTransactionType.BALANCE));
            RedeemCommand = new DelegateCommand(async () => await ExecuteAsync(AvsTransactionType.REDEEM));
            RefundCommand = new DelegateCommand(async () => await ExecuteAsync(AvsTransactionType.REFUND));
            CancelCommand = new DelegateCommand(async () => await ExecuteAsync(AvsTransactionType.CANCEL));

            ClearOutputCommand = new DelegateCommand(() => _output?.Document.Blocks.Clear());
            GenerateTxIdCommand = new DelegateCommand(() =>
            {
                TxId = $"TEST_{DateTime.Now:yyyyMMddHHmmssfff}";
                _logger.LogDebug("Generated new TxId: {TxId}", TxId);
            });
        }

        #endregion Public Constructors

        #region Public Events

        /// <summary>
        /// Occurs when the UI should display a modal wait window.
        /// </summary>
        public event EventHandler? ShowWaitRequested;

        /// <summary>
        /// Occurs when the UI should hide the modal wait window.
        /// </summary>
        public event EventHandler? HideWaitRequested;

        #endregion Public Events

        #region Public Properties

        public string Endpoint { get => _endpoint; set => SetProperty(ref _endpoint, value); }

        public string Username { get => _username; set => SetProperty(ref _username, value); }

        public string Password { get => _password; set => SetProperty(ref _password, value); }

        public string TerminalId { get => _terminalId; set => SetProperty(ref _terminalId, value); }

        public string Pan { get => _pan; set => SetProperty(ref _pan, value); }

        public string TxId { get => _txId; set => SetProperty(ref _txId, value); }

        public string TxRef { get => _txRef; set => SetProperty(ref _txRef, value); }

        public int AmountCents { get => _amountCents; set => SetProperty(ref _amountCents, value); }

        public bool UseModalDialog { get => _useModalDialog; set => SetProperty(ref _useModalDialog, value); }

        public bool UseOverlay { get => _useOverlay; set => SetProperty(ref _useOverlay, value); }

        public bool IsBusyOverlay { get => _isBusyOverlay; set => SetProperty(ref _isBusyOverlay, value); }

        public ICommand BalanceCommand { get; }

        public ICommand RedeemCommand { get; }

        public ICommand RefundCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand ClearOutputCommand { get; }

        public ICommand GenerateTxIdCommand { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Attaches a RichTextBox instance for output logging.
        /// </summary>
        public void AttachRichTextBox(RichTextBox rtb) => _output = rtb;

        #endregion Public Methods
    }
}