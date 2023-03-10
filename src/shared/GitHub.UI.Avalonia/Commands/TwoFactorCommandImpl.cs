using System.Threading;
using System.Threading.Tasks;
using GitHub.UI.ViewModels;
using GitHub.UI.Views;
using GitCredentialManager;
using GitCredentialManager.UI;

namespace GitHub.UI.Commands
{
    public class TwoFactorCommandImpl : TwoFactorCommand
    {
        public TwoFactorCommandImpl(ICommandContext context) : base(context) { }

        protected override Task ShowAsync(TwoFactorViewModel viewModel, CancellationToken ct)
        {
            return AvaloniaUi.ShowViewAsync<TwoFactorView>(viewModel, GetParentHandle(), ct);
        }
    }
}
