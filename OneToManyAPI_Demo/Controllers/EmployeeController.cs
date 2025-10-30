using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneToManyAPI_Demo.Data;
using OneToManyAPI_Demo.Models;

namespace OneToManyAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        AppDbContext _db;

        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var list = await _db.employees.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _db.employees.FirstOrDefaultAsync(p=>p.EmployeeId==id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee person)
        {
            if(person == null)
            {
                return BadRequest("Employee object is null");
            }


            var department = await _db.departments.FindAsync(person.DepartmentId);
            if (department == null)
                return BadRequest($"Department with ID {person.DepartmentId} not found");


            _db.employees.Add(person);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeById), new { id = person.EmployeeId },person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id,[FromBody] Employee emp)
        {
            var employee = await _db.employees.Include(e => e.Department).FirstOrDefaultAsync(p => id == p.EmployeeId);
            if(employee == null)
            {
                return NotFound();
            }

            employee.EmployeeName = emp.EmployeeName;

            _db.employees.Update(employee);
            await _db.SaveChangesAsync();
            return Ok(employee);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _db.employees.Include(e => e.Department).FirstOrDefaultAsync(p => id == p.EmployeeId);
            if (employee == null)
            {
                return NotFound();
            }

            _db.employees.Remove(employee);
            await _db.SaveChangesAsync();
            return Ok("Employee deleted successfully");
        }
    }
}
