using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using GitLab.UI.ViewModels;
using GitCredentialManager;
using GitCredentialManager.UI;

namespace GitLab.UI.Commands
{
    public abstract class CredentialsCommand : HelperCommand
    {
        protected CredentialsCommand(ICommandContext context)
            : base(context, "prompt", "Show authentication prompt.")
        {
            AddOption(
                new Option<string>("--url", "GitLab instance URL.")
            );

            AddOption(
                new Option<string>("--username", "Username or email.")
            );

            AddOption(
                new Option("--basic", "Enable username/password (basic) authentication.")
            );

            AddOption(
                new Option("--browser", "Enable browser-based OAuth authentication.")
            );

            AddOption(
                new Option("--pat", "Enable personal access token authentication.")
            );

            AddOption(
                new Option("--all", "Enable all available authentication options.")
            );

            Handler = CommandHandler.Create<CommandOptions>(ExecuteAsync);
        }

        private class CommandOptions
        {
            public string UserName { get; set; }
            public string Url { get; set; }
            public bool Basic { get; set; }
            public bool Browser { get; set; }
            public bool Pat { get; set; }
            public bool All { get; set; }
        }

        private async Task<int> ExecuteAsync(CommandOptions options)
        {
            var viewModel = new CredentialsViewModel(Context.Environment)
            {
                ShowBrowserLogin = options.All || options.Browser,
                ShowTokenLogin   = options.All || options.Pat,
                ShowBasicLogin   = options.All || options.Basic,
            };

            if (Uri.TryCreate(options.Url, UriKind.Absolute, out Uri uri) && !GitLabConstants.IsGitLabDotCom(uri))
            {
                viewModel.Url = options.Url;
            }

            if (!string.IsNullOrWhiteSpace(options.UserName))
            {
                viewModel.UserName = options.UserName;
                viewModel.TokenUserName = options.UserName;
            }

            await ShowAsync(viewModel, CancellationToken.None);

            if (!viewModel.WindowResult)
            {
                throw new Exception("User cancelled dialog.");
            }

            var result = new Dictionary<string, string>();

            switch (viewModel.SelectedMode)
            {
                case AuthenticationModes.Basic:
                    result["mode"] = "basic";
                    result["username"] = viewModel.UserName;
                    result["password"] = viewModel.Password;
                    break;

                case AuthenticationModes.Browser:
                    result["mode"] = "browser";
                    break;

                case AuthenticationModes.Pat:
                    result["mode"] = "pat";
                    result["pat"] = viewModel.Token;
                    result["username"] = viewModel.TokenUserName;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            WriteResult(result);
            return 0;
        }

        protected abstract Task ShowAsync(CredentialsViewModel viewModel, CancellationToken ct);
    }
}
