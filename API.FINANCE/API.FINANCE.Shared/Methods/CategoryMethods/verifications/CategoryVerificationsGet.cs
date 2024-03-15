using API.FINANCE.Shared.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.CategoryMethods.verifications
{
    public class CategoryVerificationsGet
    {
        public async static Task<AuthResultCategory> verificationGet(RefreshToken token)
        {
            if (token == null)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                };

            return new AuthResultCategory()
            {
                Result = true
            };

        }
    }
}
