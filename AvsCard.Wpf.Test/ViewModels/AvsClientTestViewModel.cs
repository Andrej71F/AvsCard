using AvsCard.Client;
using AvsCard.Helpers;
using AvsCard.RequestDto;
using AvsCard.ResponseDto;
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
    public class AvsClientTestViewModel : BindableBase
    {
        #region Private Fields

        private string _endpoint = AvsConstants.TestUrl;

        private string _username = "avs_test";

        private string _password = "test";

        private string _terminalId = "avs_test";

        private string _pan = AvsConstants.PanPrefix + "9999999990";

        private string _txId = $"TEST_{DateTime.Now:yyyyMMddHHmmssfff}";

        private string _txRef = "";

        private int _amountCents = 1000;

        private bool _useModalDialog = true;

        private bool _useOverlay = false;

        private bool _isBusyOverlay;

        private RichTextBox? _output;

        #endregion Private Fields

        #region Private Methods

        private async Task ExecuteAsync(AvsTransactionType type)
        {
            if (!Validate(type, out var err))
            {
                Append($"[VALIDATION ERROR] {err}\n");
                return;
            }

            try
            {
                if (UseModalDialog)
                    ShowWaitRequested?.Invoke(this, EventArgs.Empty);

                if (UseOverlay)
                    IsBusyOverlay = true;

                var client = new AvsClient(new HttpClient(), Endpoint);

                AvsRequestBase req = type switch
                {
                    AvsTransactionType.BALANCE => AvsRequestFactory.CreateBalance(Username, Password, TerminalId, TxId, Pan),
                    AvsTransactionType.REDEEM => AvsRequestFactory.CreateRedeem(Username, Password, TerminalId, TxId, Pan, AmountCents),
                    AvsTransactionType.REFUND => AvsRequestFactory.CreateRefund(Username, Password, TerminalId, TxId, Pan, AmountCents),
                    AvsTransactionType.CANCEL => AvsRequestFactory.CreateCancel(Username, Password, TerminalId, TxId, TxRef, Pan, AmountCents),
                    _ => throw new ArgumentOutOfRangeException()
                };

                var resp = client.Send(req);
                AppendResponse(resp);
            }
            catch (Exception ex)
            {
                Append($"[EXCEPTION] {ex.Message}\n");
            }
            finally
            {
                if (UseOverlay)
                    IsBusyOverlay = false;

                if (UseModalDialog)
                    HideWaitRequested?.Invoke(this, EventArgs.Empty);
            }
        }

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

        private void Append(string text)
        {
            if (_output == null) return;
            _output.Document.Blocks.Add(new Paragraph(new Run(text)));
            _output.ScrollToEnd();
        }

        private void AppendResponse(AvsResponseBase r)
        {
            if (_output == null) return;

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

        #endregion Private Methods

        #region Public Constructors

        public AvsClientTestViewModel()
        {
            BalanceCommand = new DelegateCommand(async () => await ExecuteAsync(AvsTransactionType.BALANCE));
            RedeemCommand = new DelegateCommand(async () => await ExecuteAsync(AvsTransactionType.REDEEM));
            RefundCommand = new DelegateCommand(async () => await ExecuteAsync(AvsTransactionType.REFUND));
            CancelCommand = new DelegateCommand(async () => await ExecuteAsync(AvsTransactionType.CANCEL));

            ClearOutputCommand = new DelegateCommand(() => _output?.Document.Blocks.Clear());
            GenerateTxIdCommand = new DelegateCommand(() =>
                TxId = $"TEST_{DateTime.Now:yyyyMMddHHmmssfff}");
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler? ShowWaitRequested;

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

        public void AttachRichTextBox(RichTextBox rtb) => _output = rtb;

        #endregion Public Methods
    }
}