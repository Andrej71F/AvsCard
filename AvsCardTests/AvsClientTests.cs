using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using AvsCard.Helpers;
using AvsCard.Client;

namespace AvsCard.Tests
{
    /// <summary>
    /// Integration tests for <see cref="AvsClient"/> verifying end-to-end
    /// communication with the AVS endpoint using real configuration values.
    /// </summary>
    [TestClass]
    [TestCategory("Integration")]
    public class AvsClientTests
    {
        #region Private Fields

        private static string _username = default!;

        private static string _password = default!;

        private static string _terminalId = default!;

        private static string _endpoint = default!;

        private static string _testPan = default!;

        #endregion Private Fields

        #region Private Methods

        /// <summary>
        /// Marks the test as inconclusive when a required configuration key is missing.
        /// </summary>
        /// <param name="key">The missing configuration key.</param>
        /// <returns>Never returns; always throws <see cref="AssertInconclusiveException"/>.</returns>
        private static string AssertInconclusive(string key)
        {
            Assert.Inconclusive($"Configuration key '{key}' is missing in appsettings.test.json.");
            return null!; // never reached
        }

        /// <summary>
        /// Creates a new instance of <see cref="AvsClient"/> using the configured endpoint.
        /// </summary>
        private AvsClient CreateClient()
        {
            return new AvsClient(new HttpClient(), _endpoint);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Loads configuration values from appsettings.test.json and environment variables.
        /// Ensures all required AVS credentials and test parameters are available.
        /// </summary>
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            _username = config["Avs:Username"] ?? AssertInconclusive("Avs:Username");
            _password = config["Avs:Password"] ?? AssertInconclusive("Avs:Password");
            _terminalId = config["Avs:TerminalId"] ?? AssertInconclusive("Avs:TerminalId");
            _endpoint = config["Avs:Endpoint"] ?? AssertInconclusive("Avs:Endpoint");
            _testPan = config["Avs:TestPan"] ?? AssertInconclusive("Avs:TestPan");
        }

        /// <summary>
        /// Sends a BALANCE request and verifies that a valid response is returned.
        /// </summary>
        [TestMethod]
        public void Balance_ShouldReturnValidResponse()
        {
            var client = CreateClient();
            var txId = $"BAL_{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            var request = AvsRequestFactory.CreateBalance(
                _username, _password, _terminalId, txId, _testPan);

            var response = client.Send(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(txId, response.TxId);
            Assert.IsTrue(response.Result >= 0);
        }

        /// <summary>
        /// Sends a REDEEM request and verifies that a response is returned.
        /// </summary>
        [TestMethod]
        public void Redeem_ShouldReturnResponse()
        {
            var client = CreateClient();
            var txId = $"RED_{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            var request = AvsRequestFactory.CreateRedeem(
                _username, _password, _terminalId, txId, _testPan, 100);

            var response = client.Send(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(txId, response.TxId);
            Assert.IsTrue(response.Result >= 0);
        }

        /// <summary>
        /// Sends a REFUND request and verifies that a response is returned.
        /// </summary>
        [TestMethod]
        public void Refund_ShouldReturnResponse()
        {
            var client = CreateClient();
            var txId = $"REF_{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            var request = AvsRequestFactory.CreateRefund(
                _username, _password, _terminalId, txId, _testPan, 100);

            var response = client.Send(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(txId, response.TxId);
            Assert.IsTrue(response.Result >= 0);
        }

        /// <summary>
        /// Performs a REDEEM operation followed by a CANCEL operation
        /// and verifies that both responses are valid.
        /// </summary>
        [TestMethod]
        public void Cancel_ShouldReturnResponse()
        {
            var client = CreateClient();

            // Step 1: Redeem first
            var originalTxId = $"RED_{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            var redeemRequest = AvsRequestFactory.CreateRedeem(
                _username, _password, _terminalId, originalTxId, _testPan, 100);

            var redeemResponse = client.Send(redeemRequest);

            if (redeemResponse.Result != 0)
                Assert.Inconclusive("Redeem failed, cannot test cancel.");

            // Step 2: Cancel it
            var cancelTxId = $"CAN_{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            var cancelRequest = AvsRequestFactory.CreateCancel(
                _username, _password, _terminalId, cancelTxId, originalTxId, _testPan, 100);

            var cancelResponse = client.Send(cancelRequest);

            Assert.IsNotNull(cancelResponse);
            Assert.AreEqual(cancelTxId, cancelResponse.TxId);
            Assert.IsTrue(cancelResponse.Result >= 0);
        }

        #endregion Public Methods
    }
}