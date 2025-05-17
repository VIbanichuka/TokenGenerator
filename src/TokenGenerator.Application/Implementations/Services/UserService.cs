using AutoMapper;
using TokenGenerator.Application.Dtos;
using TokenGenerator.Application.Interfaces.IRepositories;
using TokenGenerator.Application.Interfaces.IServices;
using TokenGenerator.Domain.Entities;

namespace TokenGenerator.Application.Implementations.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserService(IAuthService authService, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<UserDto> CreateUserAsync(UserDto user)
        {
            if (string.IsNullOrEmpty(user.Email)) 
            {
                throw new ArgumentNullException("Email is required");
            }

            await CheckIfUserExist(user.Email);

            var newUser = _mapper.Map<User>(user);
            
            await _userRepository.AddAsync(newUser);
            
            await _userRepository.SaveChangesAsync();

            await AssignVerificationTokenAsync(newUser);

            return _mapper.Map<UserDto>(newUser);
        }

        private async Task CheckIfUserExist(string email)
        {
            if (await _userRepository.AnyAsync(u => u.Email == email))
            {
                throw new ArgumentException("User already exist");
            }
        }

        private async Task AssignVerificationTokenAsync(User newUser)
        {
            var userDto = _mapper.Map<UserDto>(newUser);
            
            newUser.VerificationToken = GenerateVerificationToken(userDto);
            
            newUser.TokenExpirationTime = DateTimeOffset.UtcNow.AddMinutes(15); //Change to 24 hours later
            
            _userRepository.Update(newUser);
            
            await _userRepository.SaveChangesAsync();
        }

        private string GenerateVerificationToken(UserDto user)
        {
            var token = _authService.CreateToken(user);
            return token;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var existingUser = await _userRepository.GetByIdAsync(userId);

            if (existingUser != null)
            {
                _userRepository.Remove(existingUser);
            
                await _userRepository.SaveChangesAsync();
                
                return true;
            }
            return false;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.UserId);

            if (existingUser == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var updatedUser = _mapper.Map(user, existingUser);
            
            _userRepository.Update(updatedUser);
            
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _userRepository.FindAsync(user => user.Email == email);
            
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByVerificationTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var user = await _userRepository.FindAsync(u => u.VerificationToken == token);
            
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

    }
}
