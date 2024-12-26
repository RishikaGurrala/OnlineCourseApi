using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseApi.Core.Models;
using OnlineCourseApi.Data;
using OnlineCourseApi.Service;

namespace OnlineCourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseService;
        public CourseController(ICourseRepository courseService)
        {
            _courseService = courseService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CourseModel>>> GetAllCoursesAsync()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }
        [HttpGet("Category/{categoryId}")]
        public async Task<ActionResult<List<CourseModel>>> GetAllCoursesByIdAsync([FromRoute] int categoryId)
        {
            var courses = await _courseService.GetAllCoursesAsync(categoryId);
            return Ok(courses);
        }

        [HttpGet("Details/{courseId}")]
        public async Task<ActionResult<CourseDetailsModel>> GetCourseDetailsAsync(int courseId)
        {
            var courseDetail = await _courseService.GetCourseDetailsAsync(courseId);
            if (courseDetail == null)
            {
                return NotFound();
            }
            return Ok(courseDetail);
        }
    }
}
