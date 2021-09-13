using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public class BonusPoolService
    {
        private readonly AppDbContext _dbContext;

        public BonusPoolService()
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "HrDb");

            _dbContext = new AppDbContext(dbContextOptionBuilder.Options);
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            return (await _dbContext.Employees.Include(e => e.Department).ToListAsync()).Select(e => new EmployeeDto
            {
                Fullname = e.Fullname,
                JobTitle = e.JobTitle,
                Salary = e.Salary,
                Department = new DepartmentDto
                {
                    Title = e.Department.Title,
                    Description = e.Department.Description
                }
            }).ToList();
        }

        //Get Bonus for employee based on available bonus pool
        public async Task<BonusPoolCalculatorResultDto> GetBonusAsync(int bonusPoolAmount, int selectedEmployeeId)
        {
            if (bonusPoolAmount <= 0)
            {
                throw new ExpectedException(ExpectedException.BonusPoolNotProvided);
            }

            //load the details of the selected employee using the Id
            Employee employee = await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(item => item.Id == selectedEmployeeId);

            if (employee == null)
            {
                throw new ExpectedException(ExpectedException.EmployeeNotFound);
            }

            //get the total salary budget for the company
            decimal salariesTotal = GetSalariesTotal();

            //calculate the bonus allocation for the employee
            decimal bonusPercentage = GetBonusPercentage((decimal) employee.Salary, salariesTotal);
            int bonusAllocation = GetBonusAllocation(bonusPoolAmount, bonusPercentage);

            return new BonusPoolCalculatorResultDto
            {
                Employee = new EmployeeDto
                {
                    Fullname = employee.Fullname,
                    JobTitle = employee.JobTitle,
                    Salary = employee.Salary,
                    Department = new DepartmentDto
                    {
                        Title = employee.Department.Title,
                        Description = employee.Department.Description
                    }
                },

                Amount = bonusAllocation
            };
        }

        //Calculate bonus allocation by percentage of bonus pool
        public static int GetBonusAllocation(int bonusPoolAmount = 0, decimal bonusPercentage = 0)
        {
            if (bonusPoolAmount == 0 && bonusPercentage == 0)
            {
                return 0;
            }

            return (int)(bonusPercentage * bonusPoolAmount);
        }

        //Get the sum of salaries for all employees
        public decimal GetSalariesTotal()
        {
            return _dbContext.Employees.Any() ? (decimal)_dbContext.Employees.Sum(item => item.Salary) : 0M;
        }

        //Get Bonus Percentage of employee salary in comparison to the sum of all salaries
        public static decimal GetBonusPercentage(decimal employeeSalary, decimal salariesTotal)
        {
            return Math.Round(employeeSalary / salariesTotal, 3);
        }
    }
}
