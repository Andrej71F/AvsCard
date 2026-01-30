using AvsCard.RequestDto;
using System;

namespace AvsCard.Helpers
{
    /// <summary>
    /// Factory class for creating strongly typed AVS request objects.
    /// Provides convenience methods for constructing BALANCE, REDEEM,
    /// REFUND, and CANCEL requests with properly formatted timestamps.
    /// </summary>
    public static class AvsRequestFactory
    {
        #region Private Methods

        /// <summary>
        /// Returns the current local date and time formatted as
        /// "yyyy-MM-dd HH:mm:ss", which matches the AVS specification.
        /// </summary>
        private static string NowString() =>
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        #endregion Private Methods

        // -----------------------------
        // BALANCE
        // -----------------------------

        #region Public Methods

        /// <summary>
        /// Creates a BALANCE request for retrieving the current card balance.
        /// </summary>
        /// <param name="username">AVS username.</param>
        /// <param name="password">AVS password.</param>
        /// <param name="terminalId">Terminal identifier.</param>
        /// <param name="txId">Unique transaction ID.</param>
        /// <param name="pan">Card PAN.</param>
        /// <returns>A fully initialized <see cref="AvsBalanceRequest"/>.</returns>
        public static AvsBalanceRequest CreateBalance(
            string username, string password, string terminalId,
            string txId, string pan)
            => new AvsBalanceRequest
            {
                Username = username,
                Password = password,
                TerminalId = terminalId,
                TxId = txId,
                LocalDateTime = NowString(),
                Card = new RequestDto.AvsCard { Pan = AvsPanValidator.ExtractPan(pan) }
            };

        // -----------------------------
        // REDEEM
        // -----------------------------

        /// <summary>
        /// Creates a REDEEM request for deducting an amount from the card.
        /// </summary>
        /// <param name="username">AVS username.</param>
        /// <param name="password">AVS password.</param>
        /// <param name="terminalId">Terminal identifier.</param>
        /// <param name="txId">Unique transaction ID.</param>
        /// <param name="pan">Card PAN.</param>
        /// <param name="amountCents">Amount in cents.</param>
        /// <returns>A fully initialized <see cref="AvsRedeemRequest"/>.</returns>
        public static AvsRedeemRequest CreateRedeem(
            string username, string password, string terminalId,
            string txId, string pan, int amountCents)
            => new AvsRedeemRequest
            {
                Username = username,
                Password = password,
                TerminalId = terminalId,
                TxId = txId,
                LocalDateTime = NowString(),
                Card = new RequestDto.AvsCard { Pan = AvsPanValidator.ExtractPan(pan) },
                Amount = amountCents,
                Currency = 978
            };

        // -----------------------------
        // REFUND
        // -----------------------------

        /// <summary>
        /// Creates a REFUND request for returning funds to the card.
        /// </summary>
        /// <param name="username">AVS username.</param>
        /// <param name="password">AVS password.</param>
        /// <param name="terminalId">Terminal identifier.</param>
        /// <param name="txId">Unique transaction ID.</param>
        /// <param name="pan">Card PAN.</param>
        /// <param name="amountCents">Amount in cents.</param>
        /// <returns>A fully initialized <see cref="AvsRefundRequest"/>.</returns>
        public static AvsRefundRequest CreateRefund(
            string username, string password, string terminalId,
            string txId, string pan, int amountCents)
            => new AvsRefundRequest
            {
                Username = username,
                Password = password,
                TerminalId = terminalId,
                TxId = txId,
                LocalDateTime = NowString(),
                Card = new RequestDto.AvsCard { Pan = AvsPanValidator.ExtractPan(pan) },
                Amount = amountCents,
                Currency = 978
            };

        // -----------------------------
        // CANCEL
        // -----------------------------

        /// <summary>
        /// Creates a CANCEL request for reversing a previous REDEEM or REFUND transaction.
        /// </summary>
        /// <param name="username">AVS username.</param>
        /// <param name="password">AVS password.</param>
        /// <param name="terminalId">Terminal identifier.</param>
        /// <param name="txId">Unique transaction ID.</param>
        /// <param name="txRef">Reference to the original transaction.</param>
        /// <param name="pan">Card PAN.</param>
        /// <param name="amountCents">Amount in cents.</param>
        /// <returns>A fully initialized <see cref="AvsCancelRequest"/>.</returns>
        public static AvsCancelRequest CreateCancel(
            string username, string password, string terminalId,
            string txId, string txRef, string pan, int amountCents)
            => new AvsCancelRequest
            {
                Username = username,
                Password = password,
                TerminalId = terminalId,
                TxId = txId,
                TxRef = txRef,
                LocalDateTime = NowString(),
                Card = new RequestDto.AvsCard { Pan = AvsPanValidator.ExtractPan(pan) },
                Amount = amountCents,
                Currency = 978
            };

        #endregion Public Methods
    }
}