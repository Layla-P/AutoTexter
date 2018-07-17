using System.Threading.Tasks;

namespace AutoTexter.Services
{
    public interface IAuthenticationService
    {
        Task<string> FetchTokenAsync();
    }
}
