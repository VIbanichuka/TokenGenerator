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
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var userId = GetUserIdFromClaims();

            if(userId == Guid.Empty)
            {
                return Unauthorized("Invalid User");
            }

            var isDeleted = await _userService.DeleteUserAsync(userId);

            if (!isDeleted)
            {
                return NotFound("User not found");
            }
            
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAccountAsync([FromBody] UpdateUserRequest userRequest)
        {
            if(userRequest == null)
            {
                return BadRequest("Invalid User");
            }

            var userId = GetUserIdFromClaims();

            if (userId == Guid.Empty)
            {
                return Unauthorized("Invalid User");
            }
            
            var userDto = _mapper.Map<UserDto>(userRequest);

            userDto.UserId = userId;

            var updatedUser = await _userService.UpdateUserAsync(userDto);
            
            if (updatedUser == null)
            {
                return NotFound($"User not found.");
            }
            
            var response = _mapper.Map<UserResponse>(updatedUser);
            
            return Ok(response);
        }

        private Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            bool isValid = Guid.TryParse(userIdClaim, out Guid userId);

            return isValid ? userId : Guid.Empty;
        }
    }
}
