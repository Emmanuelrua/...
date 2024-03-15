using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.IncomeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.IncomeMethods.verifications
{
    public class IncomeVerificationsGet
    {
        public async static Task<AuthResultIncome> verificationGet(RefreshToken token)
        {
            if (token == null)
                return new AuthResultIncome()
                {
                    Result = false,
                    Errors = new List<string>() { "Token invalided" }
                };

            return new AuthResultIncome()
            {
                Result = true
            };
        }
    }
}
