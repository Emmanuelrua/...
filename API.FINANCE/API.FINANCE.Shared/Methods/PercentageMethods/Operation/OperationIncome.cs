using API.FINANCE.Shared.Methods.PercentageMethods.Return;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.PercentageMethods.Operation
{
    public class OperationIncome
    {
        public async static Task<ReturnPercentageIncome> operationIncome(MySalary ExistSalary, double Money)
        {
            double ValuePercentageIncome = (ExistSalary.Percentage * Money) / ExistSalary.Salary;
            double ValuePercentageSalary = ExistSalary.Percentage + ValuePercentageIncome;

            return new ReturnPercentageIncome()
            {
                PercentageIncome = ValuePercentageIncome,
                PercentageSalary = ValuePercentageSalary
            };
        }
    }
}
