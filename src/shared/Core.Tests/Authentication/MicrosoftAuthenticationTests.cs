using System;
using GitCredentialManager.Authentication;
using GitCredentialManager.Tests.Objects;
using Xunit;

namespace GitCredentialManager.Tests.Authentication
{
    public class MicrosoftAuthenticationTests
    {
        [Fact]
        public async System.Threading.Tasks.Task MicrosoftAuthentication_GetAccessTokenAsync_NoInteraction_ThrowsException()
        {
            const string authority = "https://login.microsoftonline.com/common";
            const string clientId = "C9E8FDA6-1D46-484C-917C-3DBD518F27C3";
            Uri redirectUri = new Uri("https://localhost");
            string[] scopes = {"user.read"};
            const string userName = null; // No user to ensure we do not use an existing token

            var context = new TestCommandContext
            {
                Settings = {IsInteractionAllowed = false},
            };

            var msAuth = new MicrosoftAuthentication(context);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => msAuth.GetTokenAsync(authority, clientId, redirectUri, scopes, userName));
        }
    }
}
