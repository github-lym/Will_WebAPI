using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hw2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hw2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public DepartmentsController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            return await _context.Department.ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            _context.Department.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Department>> DeleteDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Department.Remove(department);
            await _context.SaveChangesAsync();

            return department;
        }

        // POST: api/Departments/spc
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("spc")]
        public async Task<ActionResult<Department>> PostDepartmentBySP(Department department)
        {

            //可以跑
            // await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE dbo.Department_Insert {department.Name},{department.Budget},{department.StartDate},{department.InstructorId}");

            var sql = _context.Department.FromSqlInterpolated($"EXECUTE dbo.Department_Insert {department.Name},{department.Budget},{department.StartDate},{department.InstructorId}");
            var result = await sql.Select(r => new Department() { DepartmentId = r.DepartmentId, RowVersion = r.RowVersion, DateModified = r.DateModified }).IgnoreQueryFilters().ToListAsync();

            department.DepartmentId = result[0].DepartmentId;
            department.RowVersion = result[0].RowVersion;
            department.DateModified = result[0].DateModified;

            return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);
        }

        // PUT: api/Departments/spu/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("spu/{id}")]
        public async Task<IActionResult> PutDepartmentBySP(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            var sql = _context.Department.FromSqlInterpolated($"EXECUTE dbo.Department_Update {department.DepartmentId},{department.Name},{department.Budget},{department.StartDate},{department.InstructorId},{department.RowVersion}");
            var result = await sql.Select(r => new Department() { RowVersion = r.RowVersion, DateModified = r.DateModified }).IgnoreQueryFilters().ToListAsync();
            return NoContent();
        }

        // DELETE: api/Departments/spd/5
        [HttpDelete("spd/{id}")]
        public async Task<ActionResult<Department>> DeleteDepartmentBySP(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE dbo.Department_Delete {department.DepartmentId},{department.RowVersion}");

            return department;
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DepartmentId == id);
        }
    }
}