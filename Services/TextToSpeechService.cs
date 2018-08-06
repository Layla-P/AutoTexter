using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoTexter.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AutoTexter.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly StorageCreds _storageCreds;

        //I am using Constructor Injection for the services we need.  This was set up in the Startup.cs file
        public TextToSpeechService(IAuthenticationService authenticationService, IOptions<StorageCreds> storageCreds)
        {
            _authenticationService =
                authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

            _storageCreds =
                storageCreds.Value ?? throw new ArgumentNullException(nameof(storageCreds));

        }

        public Task<HttpSpeechResponse> GetSpeech(string body, string @from)
        {
            throw new NotImplementedException();
        }
    }
}

