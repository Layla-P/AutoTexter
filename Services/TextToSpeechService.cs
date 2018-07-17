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

        public TextToSpeechService(IAuthenticationService authenticationService, IOptions<StorageCreds> storageCreds)
        {
            _authenticationService =
                authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

            _storageCreds =
                storageCreds.Value ?? throw new ArgumentNullException(nameof(storageCreds));

        }

        public async Task<HttpSpeechResponse> GetSpeech(string body, string from)
        {
            var response = new HttpSpeechResponse();
            //below is the endpoint I was given when I added Speech Services, you can substitute it 
            //for the one you get
            var endpoint = "https://westus.tts.speech.microsoft.com/cognitiveservices/v1";
            var token = await _authenticationService.FetchTokenAsync();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Microsoft-OutputFormat", "audio-16khz-128kbitrate-mono-mp3");
                client.DefaultRequestHeaders.Add("User-Agent", "twiliotest");

                client.DefaultRequestHeaders.Add("Authorization", token);

                var uriBuilder = new UriBuilder(endpoint);

                var text = $@"
              <speak version='1.0' xmlns=""http://www.w3.org/2001/10/synthesis"" xml:lang='en-US'>
                <voice  name='Microsoft Server Speech Text to Speech Voice (en-GB, Susan, Apollo)'>
                   You had a text message from {from}
                    <break time = ""100ms"" /> The message was
                    <break time=""100ms""/> {body}
                </voice> 
              </speak>
                   ";

                var content = new StringContent(text, Encoding.UTF8, "application/ssml+xml");

                var result = await client
                    .PostAsync(uriBuilder.Uri.AbsoluteUri, content)
                    .ConfigureAwait(false);

                response.Code = result.StatusCode;
                if (result.IsSuccessStatusCode)
                {
                    var stream = result.Content.ReadAsStreamAsync();

                    using (MemoryStream bytearray = new MemoryStream())
                    {
                        stream.Result.CopyTo(bytearray);

                        response.Path = await StoreSoundbite(bytearray.ToArray())
                            .ConfigureAwait(false);
                    }

                }
                return response;
            }
        }


        private async Task<string> StoreSoundbite(byte[] soundBite)
        {
            var blobPath = "PATH_TO_YOUR_BLOB_STORAGE";
            var name = Path.GetRandomFileName();
            var filename = Path.ChangeExtension(name, ".mp3");
            var urlString = blobPath + filename;

            var creds = new StorageCredentials(_storageCreds.Account, _storageCreds.Key);
            var blob = new CloudBlockBlob(new Uri(urlString), creds);
            blob.Properties.ContentType = "audio/mpeg";

            if (!(await blob.ExistsAsync().ConfigureAwait(false)))
            {
                await blob
                    .UploadFromByteArrayAsync(soundBite, 0, soundBite.Length)
                    .ConfigureAwait(false);
            }

            return urlString;
        }
    }
}

