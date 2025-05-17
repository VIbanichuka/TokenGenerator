using AutoMapper;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using TokenGenerator.Application.Dtos;
using TokenGenerator.Application.Interfaces.IRepositories;
using TokenGenerator.Application.Interfaces.IServices;
using TokenGenerator.Domain.Entities;

namespace TokenGenerator.Application.Implementations.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IMapper _mapper;

        public AccessTokenService(ITokenRepository tokenRepository, IMapper mapper)
        {
            _tokenRepository = tokenRepository;
            _mapper = mapper;
        }

        public async Task<TokenDto> GenerateAccessTokenAsync(Guid userId, DateTimeOffset expiryDate)
        {
            if (expiryDate <= DateTimeOffset.UtcNow)
            {
                throw new ArgumentException("Expiry date must be in the future.");
            }

            if ((expiryDate - DateTimeOffset.UtcNow).TotalDays > 3)
            {
                throw new ArgumentException("Token lifespan cannot exceed 3 days.");
            }

            var token = GenerateRandomToken(6);

            var tokenDto = new TokenDto
            {
                TokenId = Guid.NewGuid(),
                AccessToken = token,
                UserId = userId,
                AccessTokenExpiryDate = expiryDate
            };

            var accessToken = _mapper.Map<Token>(tokenDto);

            await _tokenRepository.AddAsync(accessToken);

            await _tokenRepository.SaveChangesAsync();

            return _mapper.Map<TokenDto>(accessToken);
        }

        private string GenerateRandomToken(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            var data = new byte[length];
            
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            
            randomNumberGenerator.GetBytes(data);

            var result = new char[length];
            
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[data[i] % chars.Length];
            }

            return new string(result);
        }

        public async Task<bool> VerifyAccessTokenAsync(Guid userId, string accessToken)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId is invalid.");
            }
                
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.Length != 6 || !accessToken.All(char.IsLetterOrDigit))
            {
                return false;
            }

            var token = await _tokenRepository.FindAsync(t => t.AccessToken == accessToken);

            if (token == null) 
            {
                return false;
            }

            if (token.AccessTokenExpiryDate < DateTimeOffset.UtcNow)
            {
                return false;
            }

            if (token.UserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
