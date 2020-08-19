using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hw2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static hw2.Models.ContosoUniversityContext;

namespace hw2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSPController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;
        public CSPController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET api/csp
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CourseDepartmentPerson>>> GetCourseDepartmentPersons()
        {
            var result = await _context.CourseDepartmentPersons.FromSqlRaw(@"SELECT C.CourseID,C.Title,C.Credits,C.DateModified AS CourseDateModified,D.Name,D.DateModified AS DepartmentDateModified,D.InstructorID,P.FirstName,P.LastName FROM dbo.Course AS C 
LEFT JOIN dbo.Department D ON D.DepartmentID=C.DepartmentID 
LEFT JOIN dbo.Person P ON P.ID=D.InstructorID").ToListAsync();
            return result;
        }

        // GET api/csp/5
        [HttpGet("{id}")]
        public ActionResult<CourseDepartmentPerson> GetCourseDepartmentPersonById(int id)
        {
            return null;
        }

        // POST api/csp
        [HttpPost("")]
        public void PostCourseDepartmentPerson(CourseDepartmentPerson value) { }

        // PUT api/csp/5
        [HttpPut("{id}")]
        public void PutCourseDepartmentPerson(int id, CourseDepartmentPerson value) { }

        // DELETE api/csp/5
        [HttpDelete("{id}")]
        public void DeleteCourseDepartmentPersonById(int id) { }
    }
}