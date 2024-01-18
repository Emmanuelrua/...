using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Common
{
    public static class RandomGenerator
    {
        public static string GenerateRandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDRFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz$*.#_";

            return new string(Enumerable.Repeat(chars, length).Select(S=> S[random.Next(S.Length)]).ToArray());
        }
    }
}
