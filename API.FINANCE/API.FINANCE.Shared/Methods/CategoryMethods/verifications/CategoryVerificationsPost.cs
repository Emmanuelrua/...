using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;


namespace API.FINANCE.Shared.Methods.CategoryMethods.verifications
{
    public static class CategoryVerificationsPost
    {
        public async static Task<AuthResultCategory> verificationPost(CategoryRequestPost category, RefreshToken token, MySalary ExistSalary, bool CategoryPost)
        {
            if (CategoryPost)
            {
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "You already have a category with that name" }
                };
            }

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


            if (ExistSalary == null)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have a salary created" }
                };

            if (ExistSalary.Salary < category.Money)
                return new AuthResultCategory()
                {
                    Result = false,
                    Errors = new List<string>() { "you don't have Money" }
                };

            return new AuthResultCategory()
            {
                Result = true
            };
        }
    }
}
