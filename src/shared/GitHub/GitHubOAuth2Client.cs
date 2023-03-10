using System;
using System.Net.Http;
using GitCredentialManager;
using GitCredentialManager.Authentication.OAuth;

namespace GitHub
{
    public class GitHubOAuth2Client : OAuth2Client
    {
        public GitHubOAuth2Client(HttpClient httpClient, ISettings settings, Uri baseUri)
            : base(httpClient, CreateEndpoints(baseUri),
                GetClientId(settings), GetRedirectUri(settings), GetClientSecret(settings)) { }

        private static OAuth2ServerEndpoints CreateEndpoints(Uri baseUri)
        {
            Uri authEndpoint = new Uri(baseUri, GitHubConstants.OAuthAuthorizationEndpointRelativeUri);
            Uri tokenEndpoint = new Uri(baseUri, GitHubConstants.OAuthTokenEndpointRelativeUri);
            Uri deviceAuthEndpoint = new Uri(baseUri, GitHubConstants.OAuthDeviceEndpointRelativeUri);

            return new OAuth2ServerEndpoints(authEndpoint, tokenEndpoint)
            {
                DeviceAuthorizationEndpoint = deviceAuthEndpoint
            };
        }

        private static string GetClientId(ISettings settings)
        {
            // Check for developer override value
            if (settings.TryGetSetting(
                GitHubConstants.EnvironmentVariables.DevOAuthClientId,
                Constants.GitConfiguration.Credential.SectionName, GitHubConstants.GitConfiguration.Credential.DevOAuthClientId,
                out string clientId))
            {
                return clientId;
            }

            return GitHubConstants.OAuthClientId;
        }

        private static Uri GetRedirectUri(ISettings settings)
        {
            // Check for developer override value
            if (settings.TryGetSetting(
                GitHubConstants.EnvironmentVariables.DevOAuthRedirectUri,
                Constants.GitConfiguration.Credential.SectionName, GitHubConstants.GitConfiguration.Credential.DevOAuthRedirectUri,
                out string redirectUriStr) && Uri.TryCreate(redirectUriStr, UriKind.Absolute, out Uri redirectUri))
            {
                return redirectUri;
            }

            return GitHubConstants.OAuthRedirectUri;
        }

        private static string GetClientSecret(ISettings settings)
        {
            // Check for developer override value
            if (settings.TryGetSetting(
                GitHubConstants.EnvironmentVariables.DevOAuthClientSecret,
                Constants.GitConfiguration.Credential.SectionName, GitHubConstants.GitConfiguration.Credential.DevOAuthClientSecret,
                out string clientSecret))
            {
                return clientSecret;
            }

            return GitHubConstants.OAuthClientSecret;
        }
    }
}
