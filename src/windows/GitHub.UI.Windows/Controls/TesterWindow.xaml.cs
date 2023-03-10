using System.Windows;
using GitHub.UI.ViewModels;
using GitHub.UI.Views;
using GitCredentialManager.Interop.Windows;
using GitCredentialManager.UI.Controls;

namespace GitHub.UI.Controls
{
    public partial class TesterWindow : Window
    {
        private readonly WindowsEnvironment _environment = new WindowsEnvironment(new WindowsFileSystem());

        public TesterWindow()
        {
            InitializeComponent();
        }

        private void ShowCredentials(object sender, RoutedEventArgs e)
        {
            var vm = new CredentialsViewModel(_environment)
            {
                ShowBrowserLogin = useBrowser.IsChecked ?? false,
                ShowDeviceLogin = useDevice.IsChecked ?? false,
                ShowTokenLogin = usePat.IsChecked ?? false,
                ShowBasicLogin = useBasic.IsChecked ?? false,
                EnterpriseUrl = enterpriseUrl.Text,
                UserName = username.Text
            };
            var view = new CredentialsView();
            var window = new DialogWindow(view) { DataContext = vm };
            window.ShowDialog();
        }

        private void ShowTwoFactorCode(object sender, RoutedEventArgs e)
        {
            var vm = new TwoFactorViewModel(_environment)
            {
                IsSms = twoFaSms.IsChecked ?? false,
            };
            var view = new TwoFactorView();
            var window = new DialogWindow(view) { DataContext = vm };
            window.ShowDialog();
        }

        private void ShowDeviceCode(object sender, RoutedEventArgs e)
        {
            var vm = new DeviceCodeViewModel(_environment)
            {
                UserCode = userCode.Text,
                VerificationUrl = verificationUrl.Text,
            };
            var view = new DeviceCodeView();
            var window = new DialogWindow(view) { DataContext = vm };
            window.ShowDialog();
        }
    }
}
