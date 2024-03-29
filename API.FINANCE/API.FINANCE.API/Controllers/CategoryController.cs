﻿using API.FINANCE.Data;
using API.FINANCE.Data.Migrations;
using API.FINANCE.Shared;
using API.FINANCE.Shared.Auth;
using API.FINANCE.Shared.DTOs.CategoriesRequest;
using API.FINANCE.Shared.Methods.CategoryMethods.verifications;
using API.FINANCE.Shared.Methods.PercentageMethods.Category;
using API.FINANCE.Shared.Methods.PercentageMethods.Return;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.FINANCE.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly APIFinanceContext _context;

        public CategoryController(APIFinanceContext context)
        {
            _context = context;
        }
        [HttpGet("{Token}")]
        public async Task<IActionResult> Get(string Token)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == Token);

            List<Category>? categoryGet = null;

            if (token != null)
                 categoryGet = await _context.Categories.Where(a => a.UserId == token.UserId).ToListAsync();
 
            var result = await CategoryVerificationsGet.verificationGet(token);

            if(result.Result) 
                return Ok(categoryGet);
                
            return BadRequest(result);

        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> Post(CategoryRequestPost category)
        {     
            
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == category.Token);

            MySalary? ExistSalary = null; List<Category>? CategoryList = null; bool CategoryPost = false;

            if (token != null)
            {
                CategoryList = await _context.Categories.Where(a => a.UserId == token.UserId).ToListAsync();
                ExistSalary = await _context.Salaries.FirstOrDefaultAsync(f => f.UserId == token.UserId);

            }

            var result = await CategoryVerificationsPost.verificationPost(category,token, ExistSalary,CategoryList,CategoryPost);

            if (result.Result == true)
            {
                var Percentage = await OperationCategoty.operationCategory(ExistSalary,category.Money);
                var newCategory = new Category()
                {
                    Token = token.Token,
                    UserId = token.UserId,
                    NameCategory = category.NameCategory,
                    Money = category.Money,
                    DescriptionCategory = category.DescriptionCategory,
                    AddedDate = DateTime.UtcNow,
                    IsExpired = DateTime.UtcNow.AddDays(30),
                    Percentage = Percentage.PercentageCategory
                };

                ExistSalary.Salary -= category.Money;
                ExistSalary.Percentage = Percentage.PercentageSalary;
                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> Delete(CategoryRequestDelete category)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == category.Token);

            Category? CategoryDelete = null;  List<Category>? CategoryList = null; MySalary? ExistSalary = null;

            if (token != null)
            {
                CategoryList = await _context.Categories.Where(a => a.UserId == token.UserId).ToListAsync();

                ExistSalary = await _context.Salaries.FirstOrDefaultAsync(f => f.UserId == token.UserId);
                
                foreach (var CategoryName in CategoryList)
                {
                    if (CategoryName.NameCategory == category.NameCategory)
                        CategoryDelete = CategoryName;
                }

            }

            var result = await CategoryVerificationsDelete.verificationDelete(token,CategoryDelete,ExistSalary);

            if (result.Result)
            {
                var Percentage = await OperationCategoty.operationCategory(ExistSalary, CategoryDelete.Money);

                ExistSalary.Salary += CategoryDelete.Money;
                ExistSalary.Percentage += Percentage.PercentageCategory;
                _context.Categories.Remove(CategoryDelete);
                await _context.SaveChangesAsync();

                return Ok(result);
            }

            return BadRequest(result);

        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> Put(CategoryRequestPost category)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(f => f.Token == category.Token);

            List<Category>? categoryList = null;     MySalary? ExistSalary = null;     Category? CategoryUpdate = null;

            if (token != null)
            {
                categoryList = await _context.Categories.Where(a => a.UserId == token.UserId).ToListAsync();
                ExistSalary = await _context.Salaries.FirstOrDefaultAsync(f => f.UserId == token.UserId);
        
                foreach (var CategoryName in categoryList)
                {
                    if (CategoryName.NameCategory == category.NameCategory)
                        CategoryUpdate = CategoryName;
                }
            }

            var result = await CategoryVerificationsPut.verificationPut(token,category, ExistSalary, CategoryUpdate, categoryList);

            if (result.Result)
            {
                var Percentage = await OperationCategoty.operationCategory(ExistSalary, category.Money);
                var Totalcategories = CategoryUpdate.Money - category.Money; 
                var TotalPercentege= CategoryUpdate.Percentage - Percentage.PercentageCategory; 

                ExistSalary.Salary += Totalcategories;
                ExistSalary.Percentage += TotalPercentege;

                CategoryUpdate.Money = category.Money;
                CategoryUpdate.Percentage = Percentage.PercentageCategory;
                CategoryUpdate.DescriptionCategory = category.DescriptionCategory;

                await _context.SaveChangesAsync();

                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}