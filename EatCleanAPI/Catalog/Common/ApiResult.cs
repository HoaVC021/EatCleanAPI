using EatCleanAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatCleanAPI.Catalog.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }

        public string Message { get; set; }
        public string JwtToken { get; set; }

        public UsersWithToken usersWithToken { get; set; }

        public T ResultObj { get; set; }
    }
}
