using AvsCard.Wpf.Test.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AvsCard.Wpf.Test.Views
{
    public partial class AvsClientTestView : UserControl
    {
        #region Private Fields

        private PleaseWaitWindow? _waitWindow;

        #endregion Private Fields

        #region Private Methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AvsClientTestViewModel vm)
            {
                vm.ShowWaitRequested += (_, _) => ShowWaitWindow();
                vm.HideWaitRequested += (_, _) => HideWaitWindow();
            }
        }

        private void ShowWaitWindow()
        {
            if (_waitWindow == null)
            {
                _waitWindow = new PleaseWaitWindow
                {
                    Owner = Window.GetWindow(this)
                };
            }
            _waitWindow.Show();
        }

        private void HideWaitWindow()
        {
            if (_waitWindow != null)
            {
                _waitWindow.Close();
                _waitWindow = null;
            }
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AvsClientTestViewModel vm && sender is PasswordBox pb)
                vm.Password = pb.Password;
        }

        private void OutputRichTextBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AvsClientTestViewModel vm && sender is RichTextBox rtb)
                vm.AttachRichTextBox(rtb);
        }

        #endregion Private Methods

        #region Public Constructors

        public AvsClientTestView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        #endregion Public Constructors
    }
}