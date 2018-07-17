using System.Net;

namespace AutoTexter.Models
{
    public class HttpSpeechResponse
    {
        public HttpStatusCode Code { get; set; }
        public string Path { get; set; }
    }
}
