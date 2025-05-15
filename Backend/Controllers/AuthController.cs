namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService ;
        }
        [HttpPost] 
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var user = await _authService.Authenticate(login.Email,login.Password);
            if (user == null) {
                return BadRequest("user not found");
            }
            var token = _authService.GenerateToken(user);
            
            return Ok(token);
        }
    }
}