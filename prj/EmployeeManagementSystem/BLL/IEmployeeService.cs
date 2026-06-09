using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.BLL
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<Employee>> GetAllAsync();

        Task<Employee> GetByIdAsync(int employeeId);

        Task CreateAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(int employeeId);
    }
}
