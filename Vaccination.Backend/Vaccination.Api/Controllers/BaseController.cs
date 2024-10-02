using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vaccination.Api.Exceptions;

namespace Vaccination.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class BaseController : ControllerBase
    {
        protected string GetUserId()
        {
            Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userid");
            if (userIdClaim == null)
            {
                throw new SecurityTokenException("UserId not found in the token");
            }
            return userIdClaim.Value;
        }
    }
}
