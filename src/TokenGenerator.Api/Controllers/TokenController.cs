using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TokenGenerator.Api.Models.Requests;
using TokenGenerator.Api.Models.Responses;
using TokenGenerator.Application.Dtos;
using TokenGenerator.Application.Interfaces.IServices;

namespace TokenGenerator.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IMapper _mapper;
        public TokenController(IAccessTokenService accessTokenService, IMapper mapper)
        {
            _accessTokenService = accessTokenService;
            _mapper = mapper;
        }

        [HttpPost("generate-token")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Generate([FromBody] GenerateTokenRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid User!");
            }

            if (userId == Guid.Empty)
            {
                return BadRequest("Invalid user ID.");
            }

            var token = await _accessTokenService.GenerateAccessTokenAsync(userId, request.AccessTokenExpiryDate);

            var response = _mapper.Map<TokenResponse>(token);

            return Ok(response);
        }


        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("verify-access-token")]
        public async Task<IActionResult> Verify([FromBody] VerifyTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AccessToken))
            {
                return BadRequest("Token is required.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid User!");
            }

            var isValid = await _accessTokenService.VerifyAccessTokenAsync(userId, request.AccessToken);
           
            return isValid ? Ok("Token is valid.") : BadRequest("Invalid or expired token.");
        }
    }
}
