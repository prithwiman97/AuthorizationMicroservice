using AuthorizationMicroservice.JWT;
using AuthorizationMicroservice.Models;
using AuthorizationMicroservice.Providers;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthorizationMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ILog logger = LogManager.GetLogger(typeof(AuthController));
        private IConfiguration Configuration;
        private readonly MedicalRepresentativeProvider provider;
        public AuthController(IConfiguration configuration,MedicalRepresentativeProvider provider)
        {
            Configuration = configuration;
            this.provider = provider;
        }

        // POST api/<AuthController>
        [HttpPost]
        public IActionResult Post([FromBody] MedicalRepresentative representative)
        {
            if (representative == null)
            {
                logger.Error("NULL object received");
                return StatusCode(500);
            }
            try
            {
                if (provider.Validate(representative))
                {
                    TokenGenerator generator = new TokenGenerator(Configuration);
                    string token = generator.GenerateToken();
                    logger.Info("Token received");
                    return Ok(token);
                }
                logger.Error("Unauthorized access");
                return Unauthorized("Invalid Credentials");
            }
            catch
            {
                logger.Error("Internal server error");
                return StatusCode(500);
            }
        }
    }
}
