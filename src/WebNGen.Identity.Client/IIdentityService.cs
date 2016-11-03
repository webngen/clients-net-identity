using System.Security.Claims;
using System.Threading.Tasks;

namespace WebNGen.Identity.Client
{
    public interface IIdentityService
    {
        Task<Token> CreateAccessTokenAsync(ClaimsPrincipal principal);
    }
}
