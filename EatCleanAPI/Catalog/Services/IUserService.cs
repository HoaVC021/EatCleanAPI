using EatCleanAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatCleanAPI.Catalog.Services
{
    public interface IUserService
    {
        // Task<string> Authencate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);
    }
}
