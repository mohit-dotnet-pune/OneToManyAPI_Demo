using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneToManyAPI_Demo.Data;
using OneToManyAPI_Demo.Models;

namespace OneToManyAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DepartmentController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _db.departments.Include(d => d.Employees).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _db.departments.Include(d => d.Employees).FirstOrDefaultAsync(e => e.DepartmentId == id);

            if (department == null)
                return NotFound();

            return department;
        }


        [HttpPost]
        public async Task<ActionResult> CreateDepartment(Department department)
        {
            _db.departments.Add(department);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Department>> UpdateDepartment(int id, [FromBody] Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest("Department ID mismatch");
            }

            var dept = await _db.departments.Include(d => d.Employees).FirstOrDefaultAsync(e => e.DepartmentId == id);

            if (department == null)
                return NotFound();

            dept.DepartName = department.DepartName;

            _db.departments.Update(dept);
            await _db.SaveChangesAsync();

            return dept;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            var department = await _db.departments.FindAsync(id);
            if (department == null)
                return NotFound();

            _db.departments.Remove(department);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
