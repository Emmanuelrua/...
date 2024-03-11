using API.FINANCE.Shared.DTOs.MySalaryRequest;
using API.FINANCE.Shared.Methods.PercentageMethods.Return;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.PercentageMethods.Operation
{
    public class OperationPutSalary
    {
        public async static Task<double> operation(MySalaryRequest ExistSalary, double Money)
        {
            double ValuePercentageCategory = (100 * Money) / ExistSalary.Salary;

            return ValuePercentageCategory;
        }
    }
}
