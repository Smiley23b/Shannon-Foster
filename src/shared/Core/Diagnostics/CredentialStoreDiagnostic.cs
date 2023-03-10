using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GitCredentialManager.Diagnostics
{
    public class CredentialStoreDiagnostic : Diagnostic
    {
        private readonly ICredentialStore _credentialStore;

        public CredentialStoreDiagnostic(ICredentialStore credentialStore)
            : base("Credential storage")
        {
            EnsureArgument.NotNull(credentialStore, nameof(credentialStore));

            _credentialStore = credentialStore;
        }

        protected override Task<bool> RunInternalAsync(StringBuilder log, IList<string> additionalFiles)
        {
            log.AppendLine($"ICredentialStore instance is of type: {_credentialStore.GetType().Name}");

            // Create a service that is guaranteed to be unique
            string service = $"https://example.com/{Guid.NewGuid():N}";
            const string account = "john.doe";
            const string password = "letmein123"; // [SuppressMessage("Microsoft.Security", "CS001:SecretInline", Justification="Fake credential")]

            try
            {
                log.Append("Writing test credential...");
                _credentialStore.AddOrUpdate(service, account, password);
                log.AppendLine(" OK");

                log.Append("Reading test credential...");
                ICredential outCredential = _credentialStore.Get(service, account);
                if (outCredential is null)
                {
                    log.AppendLine(" Failed");
                    log.AppendLine("Test credential object is null!");
                    return Task.FromResult(false);
                }

                log.AppendLine(" OK");

                if (!StringComparer.Ordinal.Equals(account, outCredential.Account))
                {
                    log.Append("Test credential account did not match!");
                    log.AppendLine($"Expected: {account}");
                    log.AppendLine($"Actual: {outCredential.Account}");
                    return Task.FromResult(false);
                }

                if (!StringComparer.Ordinal.Equals(password, outCredential.Password))
                {
                    log.Append("Test credential password did not match!");
                    log.AppendLine($"Expected: {password}");
                    log.AppendLine($"Actual: {outCredential.Password}");
                    return Task.FromResult(false);
                }
            }
            finally
            {
                log.Append("Deleting test credential...");
                _credentialStore.Remove(service, account);
                log.AppendLine(" OK");
            }

            return Task.FromResult(true);
        }
    }
}
