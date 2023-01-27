using CRUDWepAPI.Data;
using CRUDWepAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDWepAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext employeeDbContext;
        public EmployeeController(EmployeeDbContext employeeDbContext)
        {
            this.employeeDbContext = employeeDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees= await employeeDbContext.Employees.ToListAsync();
            return Ok(employees);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                return Ok(employee);               
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            employee.Id = Guid.NewGuid();
            await employeeDbContext.Employees.AddAsync(employee);
            await employeeDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, [FromBody] Employee employee)
        {
            var exsistingEmployee = await employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (exsistingEmployee != null)
            {
                exsistingEmployee.Name = employee.Name;
                exsistingEmployee.MobileNo = employee.MobileNo;
                exsistingEmployee.EmailID = employee.EmailID;

                await employeeDbContext.SaveChangesAsync();
                return Ok(exsistingEmployee);
            }
            return NotFound("Employee not found");
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var exsistingEmployee = await employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (exsistingEmployee != null)
            {
                employeeDbContext.Employees.Remove(exsistingEmployee);
                await employeeDbContext.SaveChangesAsync();
                return Ok(exsistingEmployee);
            }
            return NotFound("Employee not found");
        }

    }
}
