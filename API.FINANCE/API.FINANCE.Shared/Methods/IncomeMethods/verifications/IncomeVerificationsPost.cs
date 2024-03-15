using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using API.FINANCE.Shared.DTOs.IncomeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.IncomeMethods.verifications
{
    public class IncomeVerificationsPost
    {
        public async static Task<AuthResultIncome> verificationPost(IncomeRequestPost income,RefreshToken token, MySalary ExistSalary, List<Income> ListIncome, bool IncomeExists)
        {
            if (token == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                };

            if (ExistSalary == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "You don't have a salary" }

                };

            foreach (var NameIncome in ListIncome)
            {
                if (NameIncome.NameIncome == income.NameIncome)
                {
                    IncomeExists = true;
                }
            }

            if (IncomeExists)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "This income already exists" }

                };

            if (income.Money <= 0)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "Money must be more than cero" }

                };

            return new AuthResultIncome()
            {
                Result = true
            };

        }
     }
}
