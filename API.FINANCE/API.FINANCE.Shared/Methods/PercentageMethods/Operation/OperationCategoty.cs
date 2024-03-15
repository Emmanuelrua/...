using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using API.FINANCE.Shared.Methods.PercentageMethods.Return;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Methods.PercentageMethods.Category
{
    public class OperationCategoty
    {
        public async static Task<ReturnPercentage> operationCategory(MySalary ExistSalary, double Money)
        {
            double ValuePercentageCategory = (ExistSalary.Percentage * Money) / ExistSalary.Salary;
            double ValuePercentageSalary = ExistSalary.Percentage - ValuePercentageCategory;

            return new ReturnPercentage()
            {
                PercentageCategory = ValuePercentageCategory,
                PercentageSalary = ValuePercentageSalary
            };
        }
    }
}
