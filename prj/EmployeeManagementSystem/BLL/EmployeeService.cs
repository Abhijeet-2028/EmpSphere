using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementSystem.DAL;
using EmployeeManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.BLL
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeDbContext dbContext;

        public EmployeeService(EmployeeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Employee>> GetAllAsync()
        {
            return await dbContext.Employees
                .OrderBy(employee => employee.LastName)
                .ThenBy(employee => employee.FirstName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            ValidateEmployeeId(employeeId);

            return await dbContext.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(employee => employee.EmployeeId == employeeId);
        }

        public async Task CreateAsync(Employee employee)
        {
            ValidateEmployee(employee);

            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            ValidateEmployee(employee);
            ValidateEmployeeId(employee.EmployeeId);

            var exists = await dbContext.Employees
                .AnyAsync(existingEmployee => existingEmployee.EmployeeId == employee.EmployeeId);

            if (!exists)
            {
                throw new InvalidOperationException("The requested employee does not exist.");
            }

            dbContext.Employees.Update(employee);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int employeeId)
        {
            ValidateEmployeeId(employeeId);

            var employee = await dbContext.Employees
                .FirstOrDefaultAsync(existingEmployee => existingEmployee.EmployeeId == employeeId);

            if (employee == null)
            {
                throw new InvalidOperationException("The requested employee does not exist.");
            }

            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync();
        }

        private static void ValidateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.FirstName = NormalizeRequiredText(employee.FirstName, nameof(employee.FirstName), 50);
            employee.LastName = NormalizeRequiredText(employee.LastName, nameof(employee.LastName), 50);
            employee.Department = NormalizeRequiredText(employee.Department, nameof(employee.Department), 100);
            employee.Designation = NormalizeRequiredText(employee.Designation, nameof(employee.Designation), 100);

            if (employee.Salary <= 0)
            {
                throw new InvalidOperationException("Salary must be greater than zero.");
            }

            if (employee.DateOfJoining == default(DateTime))
            {
                throw new InvalidOperationException("Date of joining is required.");
            }

            if (employee.DateOfJoining.Date > DateTime.Today)
            {
                throw new InvalidOperationException("Date of joining cannot be in the future.");
            }
        }

        private static string NormalizeRequiredText(string value, string fieldName, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(fieldName + " is required.");
            }

            var normalizedValue = value.Trim();

            if (normalizedValue.Length > maxLength)
            {
                throw new InvalidOperationException(fieldName + " cannot exceed " + maxLength + " characters.");
            }

            return normalizedValue;
        }

        private static void ValidateEmployeeId(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new InvalidOperationException("A valid employee id is required.");
            }
        }
    }
}
