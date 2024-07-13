using Microsoft.AspNetCore.Mvc;
using Pinboard.Domain.Interfaces.UseCases;
using System.Net;

namespace Pinboard.RestApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserUseCases _userUseCases;

        public UserController(IUserUseCases userUseCases)
        {
            _userUseCases = userUseCases;
        }

        [HttpDelete("current")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public IActionResult DeleteCurrentUser()
        {
            _userUseCases.DeleteCurrentUser();
            return NoContent();
        }
    }
}
