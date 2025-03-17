using MedicalAPI.Models;
using MedicalAPI.Models.AuthenticationModels;
using MedicalAPI.Repositories;
using MedicalAPI.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq.Expressions;

namespace MedicalAPI.Services
{
    public class UsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRepository<Role> _rolesRepository;
        private readonly IConfiguration _configuration;
        public UsersService(
            IUsersRepository usersRepository,
            IRepository<Role> rolesRepository,
            IConfiguration configuration
        )
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _configuration = configuration;
        }
        public bool Register(RegisterRequest registerRequest)
        {
            // Check if user already exists          
            var existingUser = _usersRepository.GetByEmail(registerRequest.Email);
            if (existingUser != null)
                return false;

            var role = _rolesRepository.Get((int)RolesEnum.user);

            // Create new User object
            var user = new User
            {
                FullName = registerRequest.FullName,
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Role = role!
            };
            // Save user using the Create method in UsersRepository
            return _usersRepository.Create(user);
        }
        public LoginResponse? Login(LoginRequest loginRequest)
        {
            LoginResponse response = new LoginResponse();

            var user = _usersRepository.GetByEmail(loginRequest.Email);

            if (user == null || user.Password != loginRequest.Password)
            {
                return null;
            }

            if (user.Role != null)
            {
                response.Token = JwtUtils.GenerateJwtToken(_configuration, user, user.Role.Name);
                response.User = user.toJson();
            }
            return response;
        }
    }
}
