﻿using System;
using System.Threading.Tasks;
using GitCredentialManager.UI.Commands;
using GitCredentialManager.UI.Controls;

namespace GitCredentialManager.UI
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

                int exitCode = app.RunAsync(args)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                Environment.Exit(exitCode);
            }
        }
    }
}
