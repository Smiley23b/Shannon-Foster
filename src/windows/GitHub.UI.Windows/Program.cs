﻿using System;
using System.Threading.Tasks;
using GitHub.UI.Commands;
using GitHub.UI.Controls;
using GitCredentialManager;
using GitCredentialManager.UI;

namespace GitHub.UI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            string appPath = ApplicationBase.GetEntryApplicationPath();
            string installDir = ApplicationBase.GetInstallationDirectory();
            using (var context = new CommandContext(appPath, installDir))
            using (var app = new HelperApplication(context))
            {
                if (args.Length == 0)
                {
                    await Gui.ShowWindow(() => new TesterWindow(), IntPtr.Zero);
                    return;
                }

                app.RegisterCommand(new CredentialsCommandImpl(context));
                app.RegisterCommand(new TwoFactorCommandImpl(context));
                app.RegisterCommand(new DeviceCodeCommandImpl(context));

                int exitCode = app.RunAsync(args)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                Environment.Exit(exitCode);
            }
        }
    }
}
