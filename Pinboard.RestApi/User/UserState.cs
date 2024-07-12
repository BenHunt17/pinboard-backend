using Pinboard.Domain.Interfaces;
using System.Security.Claims;

namespace Pinboard.RestApi.User
{
    public class UserState : IUserState
    {
        public UserState(IHttpContextAccessor httpContextAccessor)
        {
            Identifier = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                ?? string.Empty;
        }

        public string Identifier { get; }
    }
}
