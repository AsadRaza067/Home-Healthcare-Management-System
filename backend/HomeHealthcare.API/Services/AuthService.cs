using HomeHealthcare.API.Models;
using HomeHealthcare.API.Models.DTOs;
using HomeHealthcare.API.Repositories.Interfaces;
using HomeHealthcare.API.Services.Interfaces;

namespace HomeHealthcare.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ICaregiverRepository _caregiverRepository;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IPatientRepository patientRepository,
            ICaregiverRepository caregiverRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _caregiverRepository = caregiverRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                return null; // Email already registered

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            var userId = await _userRepository.CreateAsync(user);
            user.UserId = userId;

            // Create the role-specific profile row
            if (dto.Role == "Patient")
            {
                await _patientRepository.CreateAsync(new Patient
                {
                    UserId = userId,
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    DateOfBirth = DateTime.UtcNow,
                    MedicalHistory = string.Empty,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else if (dto.Role == "Caregiver")
            {
                await _caregiverRepository.CreateAsync(new Caregiver
                {
                    UserId = userId,
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Specialization = dto.Specialization,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                });
            }

            var token = _tokenService.GenerateToken(user);
            return new AuthResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                UserId = user.UserId
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var token = _tokenService.GenerateToken(user);
            return new AuthResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                UserId = user.UserId
            };
        }
    }
}
