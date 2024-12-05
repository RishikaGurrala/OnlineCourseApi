using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourseApi.Core.Models;

namespace OnlineCourseApi.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseService _courseService;
        public CourseService(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public Task<List<CourseModel>> GetAllCoursesAsync(int? categoryId = null)
        {
            throw new NotImplementedException();
        }

        public Task<CourseDetailsModel> GetCourseDetailsAsync(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}
