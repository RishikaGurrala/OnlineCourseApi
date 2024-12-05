using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourseApi.Core.Models;

namespace OnlineCourseApi.Service
{
    public interface ICourseService
    {
        Task<List<CourseModel>> GetAllCoursesAsync(int? categoryId = null);
        Task<CourseDetailsModel> GetCourseDetailsAsync(int courseId);

    }
}
