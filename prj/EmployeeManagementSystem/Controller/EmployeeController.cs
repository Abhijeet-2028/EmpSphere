using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EmployeeManagementSystem.BLL;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementSystem.Controller
{
    public class EmployeeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            this.employeeService = employeeService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var employees = await employeeService.GetAllAsync();
                return View(employees);
            }
            catch (Exception exception)
            {
                return DatabaseUnavailable("loading employees", exception);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            Employee employee;

            try
            {
                employee = await employeeService.GetByIdAsync(id);
            }
            catch (Exception exception)
            {
                return DatabaseUnavailable("loading employee details", exception);
            }

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Create()
        {
            return View(new Employee
            {
                DateOfJoining = DateTime.Today
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Department,Designation,Salary,DateOfJoining")] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            try
            {
                await employeeService.CreateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View(employee);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Employee employee;

            try
            {
                employee = await employeeService.GetByIdAsync(id);
            }
            catch (Exception exception)
            {
                return DatabaseUnavailable("loading the employee edit form", exception);
            }

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Department,Designation,Salary,DateOfJoining")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            try
            {
                await employeeService.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View(employee);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Employee employee;

            try
            {
                employee = await employeeService.GetByIdAsync(id);
            }
            catch (Exception exception)
            {
                return DatabaseUnavailable("loading the employee delete confirmation", exception);
            }

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await employeeService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                var employee = await employeeService.GetByIdAsync(id);
                return View(employee);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current != null ? Activity.Current.Id : HttpContext.TraceIdentifier
            });
        }

        private IActionResult DatabaseUnavailable(string operation, Exception exception)
        {
            logger.LogError(exception, "Database error while {Operation}.", operation);

            return View("DatabaseUnavailable", new DatabaseErrorViewModel
            {
                Operation = operation,
                Message = "Oracle 19c is not reachable or the EMS_APP.EMPLOYEES table has not been created. Verify the Oracle19cConnection connection string and run DAL/OracleSchema.sql."
            });
        }
    }
}
