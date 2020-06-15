namespace Zatoichi.Common.Infrastructure.Services
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using Microsoft.Extensions.Options;

    [ExcludeFromCodeCoverage]
    public class TokenClient
    {
        public TokenClient(HttpClient client, IOptions<TokenClientOptions> options)
        {
            this.Client = client;
            this.Options = options.Value;
        }

        public HttpClient Client { get; }
        public TokenClientOptions Options { get; }

        public async Task<string> GetToken()
        {
            var response = await this.Client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = this.Options.Address,
                ClientId = this.Options.ClientId,
                ClientSecret = this.Options.ClientSecret
            });

            return response.AccessToken ?? response.Error;
        }
    }
}