using AvsCard.Client;
using AvsCard.Helpers;
using AvsCard.RequestDto;
using AvsCard.ResponseDto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;

namespace AvsCard.Tests
{
    [TestClass]
    [TestCategory("ErrorHandling")]
    public class AvsClientErrorTests
    {
        #region Private Fields

        private const string DummyEndpoint = "https://invalid-host-12345.test";

        #endregion Private Fields

        #region Private Methods

        private static AvsBalanceRequest CreateValidRequest()
        {
            return new AvsBalanceRequest
            {
                Username = "test",
                Password = "test",
                TerminalId = "test",
                TxId = "TX123",
                LocalDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Card = new RequestDto.AvsCard
                {
                    Pan = "6364530000029556"
                }
            };
        }

        private AvsClient CreateClient(HttpMessageHandler handler = null!)
        {
            return handler == null
                ? new AvsClient(new HttpClient(), DummyEndpoint)
                : new AvsClient(new HttpClient(handler), DummyEndpoint);
        }

        #endregion Private Methods

        #region Private Classes

        private class FakeHandler : HttpMessageHandler
        {
            #region Private Fields

            private readonly string _response;

            #endregion Private Fields

            #region Protected Methods

            protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_response)
                };
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Send(request, cancellationToken));
            }

            #endregion Protected Methods

            #region Public Constructors

            public FakeHandler(string response)
            {
                _response = response;
            }

            #endregion Public Constructors
        }

        // -------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------
        private class TimeoutHandler : HttpMessageHandler
        {
            #region Protected Methods

            protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                throw new TaskCanceledException("Simulated timeout");
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                throw new TaskCanceledException("Simulated timeout");
            }

            #endregion Protected Methods
        }

        #endregion Private Classes

        #region Public Methods

        // -------------------------------------------------------------
        // 1. INVALID URL → HttpNetworkError
        // -------------------------------------------------------------
        [TestMethod]
        public void InvalidUrl_ShouldReturnHttpNetworkError()
        {
            var client = CreateClient();
            var request = CreateValidRequest();

            var response = client.Send(request);

            Assert.IsInstanceOfType(response, typeof(AvsHttpCommonErrorResponse));
            Assert.AreEqual((int)AvsResultCode.HttpNetworkError, response.Result);
        }

        // -------------------------------------------------------------
        // 2. EMPTY PAN → InvalidRequest
        // -------------------------------------------------------------
        [TestMethod]
        public void EmptyPan_ShouldReturnInvalidRequest()
        {
            var client = CreateClient();
            var request = CreateValidRequest();
            request.Card.Pan = "";

            var response = client.Send(request);

            Assert.IsInstanceOfType(response, typeof(AvsHttpCommonErrorResponse));
            Assert.AreEqual((int)AvsResultCode.DataValidationError, response.Result);
        }

        // -------------------------------------------------------------
        // 3. INVALID PAN → InvalidRequest
        // -------------------------------------------------------------
        [TestMethod]
        public void InvalidPan_ShouldReturnInvalidRequest()
        {
            var client = CreateClient();
            var request = CreateValidRequest();
            request.Card.Pan = "6364530000000000"; // invalid Luhn

            var response = client.Send(request);

            Assert.IsInstanceOfType(response, typeof(AvsHttpCommonErrorResponse));
            Assert.AreEqual((int)AvsResultCode.DataValidationError, response.Result);
        }

        // -------------------------------------------------------------
        // 4. EMPTY TXID → InvalidRequest
        // -------------------------------------------------------------
        [TestMethod]
        public void EmptyTxId_ShouldReturnInvalidRequest()
        {
            var client = CreateClient();
            var request = CreateValidRequest();
            request.TxId = "";

            var response = client.Send(request);

            Assert.IsInstanceOfType(response, typeof(AvsHttpCommonErrorResponse));
            Assert.AreEqual((int)AvsResultCode.InvalidRequest, response.Result);
        }

        // -------------------------------------------------------------
        // 5. TIMEOUT → HttpTimeout
        // -------------------------------------------------------------
        [TestMethod]
        public void Timeout_ShouldReturnHttpTimeout()
        {
            var handler = new TimeoutHandler();
            var client = CreateClient(handler);
            var request = CreateValidRequest();

            var response = client.Send(request);

            Assert.IsInstanceOfType(response, typeof(AvsHttpCommonErrorResponse));
            Assert.AreEqual((int)AvsResultCode.HttpTimeout, response.Result);
        }

        // -------------------------------------------------------------
        // 6. INVALID XML → UnknownError
        // -------------------------------------------------------------
        [TestMethod]
        public void InvalidXml_ShouldReturnUnknownError()
        {
            var handler = new FakeHandler("<INVALID_XML>");
            var client = CreateClient(handler);
            var request = CreateValidRequest();

            var response = client.Send(request);

            Assert.IsInstanceOfType(response, typeof(AvsHttpCommonErrorResponse));
            Assert.AreEqual((int)AvsResultCode.UnknownError, response.Result);
        }

        #endregion Public Methods
    }
}