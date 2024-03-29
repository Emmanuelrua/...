﻿using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.CategoryMethods.verifications
{
    public static class CategoryVerificationsPut
    {
        public async static Task<AuthResultCategory> verificationPut(RefreshToken token, CategoryRequestPost category, MySalary ExistSalary, Category categoryUpdate, List<Category> categoryList)
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
                    Errors = new List<string>() { "you don't have that category" }
                };

            if (ExistSalary.Salary + categoryUpdate.Money < category.Money)
                return new AuthResultCategory()
                {
                    Result = true,
                    Errors = new List<string>() { "It is recommended to lower expenses, since I am having losses" }
                };

            if (categoryList == null)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "You don't have categories yet" }
                };

            return new AuthResultCategory()
            {
                Result = true
            };

        }
    }
}
