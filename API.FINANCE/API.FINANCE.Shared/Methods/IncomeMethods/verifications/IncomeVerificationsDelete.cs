using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.IncomeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.IncomeMethods.verifications
{
    public class IncomeVerificationsDelete
    {
        public async static Task<AuthResultIncome> verificationDelete(RefreshToken token, MySalary ExistSalary, List<Income> incomeList, Income incomeDelete)
        {
            if (token == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                };

            if (incomeList == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have incomes created" }
                };

            if (ExistSalary == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have a salary" }

                };

            if (incomeDelete == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "you do not have that income created" }

                };

            return new AuthResultIncome()
            {
                Result = true
            };
        }
    }
}
