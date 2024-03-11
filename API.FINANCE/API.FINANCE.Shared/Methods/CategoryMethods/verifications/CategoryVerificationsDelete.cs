using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.CategoryMethods.verifications
{
    public class CategoryVerificationsDelete
    {
        public async static Task<AuthResultCategory> verificationDelete(RefreshToken token, Category CategoryDelete, MySalary ExistSalary)
        {
            if (token == null)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                };

            if (ExistSalary == null)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have a salary" }


                };
            if (CategoryDelete == null)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have that category" }
                };

            return new AuthResultCategory()
            {
                Result = true
            };
        }
    }
}
