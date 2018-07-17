using AutoTexter.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AutoTexter.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        public static readonly string FetchTokenUri =
            "https://westus.api.cognitive.microsoft.com/sts/v1.0/issueToken";

        private readonly string _subscriptionKey;
        public AuthenticationService(IOptions<CsAccount> csAccount)
        {
            _subscriptionKey = csAccount.Value.SubscriptionKey;
        }

        public async Task<string> FetchTokenAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
                var uriBuilder = new UriBuilder(FetchTokenUri);

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                return await result.Content.ReadAsStringAsync();
            }
        }

    }
}
