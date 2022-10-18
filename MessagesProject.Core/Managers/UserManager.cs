using AutoMapper;
using CSVWorker.Helper;
using MessagesProject.Core.Managers.Interfaces;
using MessagesProject.Models;
using MessagesProject.ModelView;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Tazeez.Common.Extensions;

namespace MessagesProject.Core.Managers
{
    public class UserManager : IUserManager
    {
        private ToDoListDataContext _taskdbContext;
        private IMapper _mapper;

        public UserManager(ToDoListDataContext csvdbContext, IMapper mapper)
        {
            _taskdbContext = csvdbContext;
            _mapper = mapper;
        }

        #region public 

        public LoginUserResponse Login( LoginModelView userReg)
        {
            var user = _taskdbContext.Users
                                   .FirstOrDefault(a => a.Email
                                                           .ToLower().Equals(userReg.Email.ToLower()));

            if (user == null || !VerifyHashPassword(userReg.Password, user.Password))
            {
                throw new ServiceValidationException(300, "Invalid user name or password received");
            }

            var res = _mapper.Map<LoginUserResponse>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";
            return res;
        }

        public LoginUserResponse SignUp(UserRegistrationModel userReg) 
        {
            if (_taskdbContext.Users.Any(a => a.Email.ToLower().Equals(userReg.Email.ToLower())))
            {
                throw new ServiceValidationException("User already exist");
            }

            var hashedPassword = HashPassword(userReg.Password);

            var user = _taskdbContext.Users.Add(new User
            {
                FirstName = userReg.FirstName,
                LastName = userReg.LastName,
                Email = userReg.Email.ToLower(),
                Password = hashedPassword,
                ConfirmPassword = hashedPassword,
                ImageUrl = string.Empty
            }).Entity;

            _taskdbContext.SaveChanges();

            var res = _mapper.Map<LoginUserResponse>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";

            return res;
        }

        public UserModel UpdateProfile(UserModel currentUser, UserModel request)
        {
            var user = _taskdbContext.Users
                                    .FirstOrDefault(a => a.Id == currentUser.Id)
                                    ?? throw new ServiceValidationException("User not found");

            var url = "";

            if (!string.IsNullOrWhiteSpace(request.ImageString))
            {
                url = Helper.SaveImage(request.ImageString, "profileimages");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44309/";
                user.ImageUrl = @$"{baseURL}/api/v1/user/fileretrive/profilepic?filename={url}";
            }

            _taskdbContext.SaveChanges();
            return _mapper.Map<UserModel>(user);
        }

        public void DeleteUser(UserModel currentUser, int id)
        {
            if (currentUser.Id == id)
            {
                throw new ServiceValidationException("You have no access to delete your self");
            }

            var user = _taskdbContext.Users
                                    .FirstOrDefault(a => a.Id == id)
                                    ?? throw new ServiceValidationException("User not found");
            // for soft delete
            user.Archived = true;
            _taskdbContext.SaveChanges();
        }

        #endregion public 

        #region private 

        private static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        private static bool VerifyHashPassword(string password, string HashedPasword)
        {
            return BCrypt.Net.BCrypt.Verify(password, HashedPasword);
        }

        private string GenerateJWTToken(User user)
        {
            var jwtKey = "#test.key*&^vanthis%$^&*()$%^@#$@!@#%$#^%&*%^*";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("DateOfJoining", user.CreatedAt.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var issuer = "test.com";

            var token = new JwtSecurityToken(
                        issuer,
                        issuer,
                        claims,
                        expires: DateTime.Now.AddDays(20),
                        signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion private  
    }
}
