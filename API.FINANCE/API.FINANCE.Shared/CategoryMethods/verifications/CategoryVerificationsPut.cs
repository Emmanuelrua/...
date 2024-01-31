using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.CategoryMethods.verifications
{
    public static class CategoryVerificationsPut
    {
        public async static Task<AuthResultCategory> verificationPut(RefreshToken token, CategoryRequestPost category, MySalary ExistSalary, Category categoryUpdate)
        {
            if (token == null)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                };

            if (category.Money <= 0)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "Money invalided" }
                };

            if (categoryUpdate == null)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have a category" }
                };

            if (ExistSalary.Salary < category.Money)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have Money" }
                };

            return new AuthResultCategory()
            {
                Result = true,
            };

        }
    }
}
