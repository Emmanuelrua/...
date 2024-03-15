using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.IncomeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.IncomeMethods.verifications
{
    public class IncomeVerificationsPut
    {
        public async static Task<AuthResultIncome> verificationPut(RefreshToken token, IncomeRequestPost income, Income incomePut, List<Income> incomeList)
        {
            if (token == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                };

            if (income.Money <= 0)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "Money invalided" }
                };

            if (incomePut == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have that category" }
                };

            if (incomeList == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "You don't have categories yet" }
                };

            return new AuthResultIncome()
            {
                Result = true
            };
        }
    }
}
