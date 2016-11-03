using System.Threading.Tasks;

namespace WebNGen.Identity.Client
{
    public interface IIdentityDiscoveryService
    {
        Task<string> GetTokensUri();
    }
}
