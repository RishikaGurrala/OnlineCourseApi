using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourseApi.Core.Models;
using OnlineCourseApi.Data;

namespace OnlineCourseApi.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;
        public CourseService(Data.ICourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
        }
        public Task<List<CourseModel>> GetAllCoursesAsync(int? categoryId = null)
        {
            return _courseRepo.GetAllCoursesAsync(categoryId);
        }

        public Task<CourseDetailsModel> GetCourseDetailsAsync(int courseId)
        {
            return _courseRepo.GetCourseDetailsAsync(courseId);
        }
    }
}
