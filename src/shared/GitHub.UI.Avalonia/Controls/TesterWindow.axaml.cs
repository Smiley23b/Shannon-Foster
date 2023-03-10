using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GitHub.UI.ViewModels;
using GitHub.UI.Views;
using GitCredentialManager;
using GitCredentialManager.Interop.Linux;
using GitCredentialManager.Interop.MacOS;
using GitCredentialManager.Interop.Posix;
using GitCredentialManager.Interop.Windows;
using GitCredentialManager.UI.Controls;

namespace GitHub.UI.Controls
{
    public class TesterWindow : Window
    {
        private readonly IEnvironment _environment;

        public TesterWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            if (PlatformUtils.IsWindows())
            {
                _environment = new WindowsEnvironment(new WindowsFileSystem());
            }
            else
            {
                IFileSystem fs;
                if (PlatformUtils.IsMacOS())
                {
                    fs = new MacOSFileSystem();
                }
                else
                {
                    fs = new LinuxFileSystem();
                }

                _environment = new PosixEnvironment(fs);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ShowCredentials(object sender, RoutedEventArgs e)
        {
            var vm = new CredentialsViewModel(_environment)
            {
                ShowBrowserLogin = this.FindControl<CheckBox>("useBrowser").IsChecked ?? false,
                ShowDeviceLogin = this.FindControl<CheckBox>("useDevice").IsChecked ?? false,
                ShowTokenLogin = this.FindControl<CheckBox>("usePat").IsChecked ?? false,
                ShowBasicLogin = this.FindControl<CheckBox>("useBasic").IsChecked ?? false,
                EnterpriseUrl = this.FindControl<TextBox>("enterpriseUrl").Text,
                UserName = this.FindControl<TextBox>("username").Text
            };
            var view = new CredentialsView();
            var window = new DialogWindow(view) {DataContext = vm};
            window.ShowDialog(this);
        }

        private void ShowTwoFactorCode(object sender, RoutedEventArgs e)
        {
            var vm = new TwoFactorViewModel(_environment)
            {
                IsSms = this.FindControl<CheckBox>("2faSms").IsChecked ?? false,
            };
            var view = new TwoFactorView();
            var window = new DialogWindow(view) {DataContext = vm};
            window.ShowDialog(this);
        }

        private void ShowDeviceCode(object sender, RoutedEventArgs e)
        {
            var vm = new DeviceCodeViewModel(_environment)
            {
                UserCode = this.FindControl<TextBox>("userCode").Text,
                VerificationUrl = this.FindControl<TextBox>("verificationUrl").Text,
            };
            var view = new DeviceCodeView();
            var window = new DialogWindow(view) {DataContext = vm};
            window.ShowDialog(this);
        }
    }
}
