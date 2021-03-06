﻿using System;
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
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _context.Course.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            if (CourseNameCheck(course.Title))
                return BadRequest("Multiple Title!!");

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (CourseNameCheck(course.Title))
                return BadRequest("Multiple Title!!");

            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        #region View區
        // GET: vwCourseStudents
        [HttpGet("~/vwCourseStudents")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetCourseStudents()
        {
            return await _context.VwCourseStudents.ToListAsync();
        }

        // GET: vwCourseStudents/id
        [HttpGet("~/vwCourseStudents/{id}")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetCourseStudents(int id)
        {
            var result = await _context.VwCourseStudents.Where(d => d.CourseId == id).ToListAsync();
            if (result == null) return NotFound();
            return result;
        }

        // GET: vwCourseStudentCount
        [HttpGet("~/vwCourseStudentCount")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetCourseStudentCount()
        {
            return await _context.VwCourseStudentCount.ToListAsync();
        }

        // GET: vwCourseStudentCount/id
        [HttpGet("~/VwCourseStudentCount/{id}")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetCourseStudentCount(int id)
        {
            var result = await _context.VwCourseStudentCount.Where(d => d.CourseId == id).ToListAsync();
            if (result == null) return NotFound();
            return result;
        }

        // GET: vwDepartmentCourseCount 
        [HttpGet("~/vwDepartmentCourseCount")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetDepartmentCourseCount()
        {

            var sql = _context.VwDepartmentCourseCount.FromSqlRaw("SELECT * FROM dbo.vwDepartmentCourseCount");
            return await sql.ToListAsync();
        }

        // GET: vwDepartmentCourseCount/id
        [HttpGet("~/vwDepartmentCourseCount/{id}")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetDepartmentCourseCount(int id)
        {

            var result = await _context.VwDepartmentCourseCount.FromSqlInterpolated($"SELECT * FROM dbo.vwDepartmentCourseCount WHERE DepartmentID={id}").ToListAsync();
            if (result == null) return NotFound();
            return result;
        }
        #endregion

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }

        private bool CourseNameCheck(string title)
        {
            return _context.Course.Any(e => e.Title == title);
        }

    }
}