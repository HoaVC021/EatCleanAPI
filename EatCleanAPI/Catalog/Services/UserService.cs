using EatCleanAPI.Models;
using EatCleanAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatCleanAPI.Catalog.Services
{
    public class UserService : IUserService
    {
        //private readonly Customer _userManager;

        private readonly IConfiguration _config;


        public UserService(IConfiguration config)
        {
            // _userManager = userManager;
            _config = config;
        }

        //public async Task<string> Authencate(LoginRequest request)
        //{
        //    var user = await _userManager.FindByNameAsync(request.Email);
        //    if (user == null) return null;

        //    var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
        //    if (!result.Succeeded)
        //    {
        //        return null;
        //    }
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var claims = new[]
        //    {
        //        new Claim( ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.GivenName,user.CustomerName),
        //        new Claim(ClaimTypes.Role, string.Join(";",roles))
        //    };
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(_config["Tokens:Issuer"],
        //        _config["Tokens:Issuer"],
        //        claims,
        //        expires: DateTime.Now.AddHours(3),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public async Task<bool> Register(RegisterRequest request)
        {
            //var user = await _userManager.FindByNameAsync(request.UserName);
            //if (user != null)
            //{
            //    return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
            //}
            //if (await _userManager.FindByEmailAsync(request.Email) != null)
            //{
            //    return new ApiErrorResult<bool>("Emai đã tồn tại");
            //}

            var user = new User()
            {
                Name = request.CustomerName,
                Email = request.Email,
                PhoneNumber = request.Phone,
                Password = request.Password,
                IsAdmin= request.IsAdmin

            };
            // var result = await _userManager.CreateAsync(user, request.Password);
            //if (result.Succeeded)
            //{
            //    return true;
            //}
            return false;
        }

    }
}