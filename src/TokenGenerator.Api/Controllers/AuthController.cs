using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenGenerator.Api.Models.Requests;
using TokenGenerator.Api.Models.Responses;
using TokenGenerator.Application.Dtos;
using TokenGenerator.Application.Interfaces.IServices;

namespace TokenGenerator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration,
            IEmailService emailService,
            IMapper mapper, 
            IPasswordService passwordService, 
            IUserService userService, 
            IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
            _passwordService = passwordService;
            _userService = userService;
            _mapper = mapper;
            _emailService = emailService;
        }


        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegisterAccountAsync([FromBody] RegisterUserRequest userRequest)
        {
            if (userRequest == null || string.IsNullOrWhiteSpace(userRequest.Email))
            {
                return BadRequest("Invalid user data.");
            }

            _passwordService.CreatePasswordHash(userRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new UserDto()
            {
                Email = userRequest.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            var createdUser = await _userService.CreateUserAsync(user);

            var verifyUrl = $"{_configuration["Domain:BaseUrl"]}/api/auth/verify-email?token={createdUser.VerificationToken}";

            var email = new EmailMessage()
            {
                To = createdUser.Email,
                Subject = "Final Step - Verify Your Email",
                Body = $"<p>Click <a href='{verifyUrl}'>here</a> to verify your email. For Swagger Test Copy and Paste {createdUser.VerificationToken} in the input field.</p>"
            };

            await _emailService.SendEmailAsync(email);

            return Created("", _mapper.Map<UserResponse>(createdUser));
        }


        [HttpPost("Login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            if (string.IsNullOrWhiteSpace(userLoginRequest.Email) || string.IsNullOrWhiteSpace(userLoginRequest.Password))
            {
                return BadRequest("Invalid user");
            }

            var user = await _userService.GetUserByEmailAsync(userLoginRequest.Email);

            if (user == null)
            {
                return NotFound("Invalid Account");
            }

            var isPasswordVerified = _passwordService.VerifyPasswordHash(userLoginRequest.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordVerified)
            {
                return Unauthorized("Incorrect password");
            }

            var token = _authService.CreateToken(user);
            
            return Ok(token);
        }

        [HttpGet("verify-email")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Missing token");
            }

            var user = await _userService.GetUserByVerificationTokenAsync(token);
            
            if (user == null) 
            {
                return NotFound("Oops! User not found or token invalid");
            }

            if (user.IsEmailVerified)
            {
                return BadRequest("Email already verified.");
            }

            if (user.TokenExpirationTime == null || user.TokenExpirationTime < DateTimeOffset.UtcNow) 
            {
                return BadRequest("Oops! Token has expired.");
            }

            user.IsEmailVerified = true;
            
            user.VerificationToken = string.Empty;
            
            user.TokenExpirationTime = DateTimeOffset.MinValue;

            await _userService.UpdateUserAsync(user);
            
            return Ok("Sweet! Email Verified");
        }
    }
}
