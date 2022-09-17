using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrganizationRepository.Models;
using OrganizationRepository.Services.UserServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrganizationWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IConfiguration _configuration;
        public UserController(IUserServices userServices, IConfiguration configuration)
        {
            _userServices = userServices;
            _configuration = configuration;
           
        }
        // Post endPoint "api/UserController/AuthenticateUser"
        /// <summary>    
        /// AuthenticateUser for login
        /// </summary> 
        /// <param name="login">EmailId and Password</param>
        /// <returns>AuthenticatedResponse: Token</returns> 
        [HttpPost]
        [AllowAnonymous]
        public AuthenticatedResponse AuthenticateUser(Credentials login)
        {
            if (_userServices.UserLogin(login))
            {
                var key = _configuration.GetValue<string>("JwtConfig:key");
                var keyBytes = Encoding.ASCII.GetBytes(key);

                var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration["JwtConfig:Issuer"],
                    audience: _configuration["JwtConfig:Audience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: SigningCredentials
                );

                var token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return new AuthenticatedResponse { Token = token };
            }
            return null;
        }
        // Post endPoint "api/UserController/RegisterUser"
        /// <summary>    
        /// Register NewUser
        /// </summary> 
        /// <param name="register">Enter User Details</param>
        /// <returns>Boolean: true/false</returns> 
        [HttpPost]
        [AllowAnonymous]
        public bool RegisterUser(UserRegistration register)
        {
            if (_userServices.AddNewUser(register))
                return true;
            else
                return false;
        }
        // Get endPoint "api/UserController/GetUsers"
        /// <summary>    
        /// GetUsers Details
        /// </summary> 
        /// <returns>UserData: Get Users Data</returns> 
        [HttpGet]
        public IEnumerable<UserData> GetUsers()
        {
            return _userServices.GetUsers();
        }
        // Get endPoint "api/UserController/GetUserByUsernameOrEmailId"
        /// <summary>    
        /// GetUser By Username Or EmailId
        /// </summary> 
        /// <param name="UsernameOrEmailId">Enter Username Or EmailId</param>
        /// <returns>UserData: Get Users Data</returns> 
        [HttpGet]
        public IEnumerable<UserData> GetUserByUsernameOrEmailId(string UsernameOrEmailId)
        {
            return _userServices.GetUserByUsernameOrEmailId(UsernameOrEmailId);
        }
        // Put endPoint "api/UserController/UpdateUser"
        /// <summary>    
        /// Update User Data
        /// </summary> 
        /// <param name="user">Enter User Details</param>
        /// <returns>Boolean: true/false</returns> 
        [HttpPut]
        public bool UpdateUser(UserRegistration user)
        {
            if (_userServices.UpdateUser(user))
                return true;
            return false;
        }
        // Delete endPoint "api/UserController/DeleteUserByEmailId"
        /// <summary>    
        /// Delete User By EmailId
        /// </summary> 
        /// <param name="EmailId">Enter EmailId</param>
        /// <returns>Boolean: true/false</returns> 
        [HttpDelete]
        public bool DeleteUserByEmailId(string EmailId)
        {
            if (_userServices.DeleteUserByEmailId(EmailId))
                return true;
            return false;
        }
    }
}
