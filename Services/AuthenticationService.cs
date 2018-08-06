using AutoTexter.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AutoTexter.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        //Your Token API endpoint will most likely be the same as this one, but change it if it's not
        public static readonly string FetchTokenUri =
            "https://westus.api.cognitive.microsoft.com/sts/v1.0/issueToken";

        private readonly string _subscriptionKey;

        public AuthenticationService(IOptions<CsAccount> csAccount)
        {
            _subscriptionKey = csAccount.Value.SubscriptionKey;
        }

        public Task<string> FetchTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
