using System.Threading.Tasks;
using AutoTexter.Models;

namespace AutoTexter.Services
{
    public interface ITextToSpeechService
    {
        Task<HttpSpeechResponse> GetSpeech(string body, string from);
    }
}
