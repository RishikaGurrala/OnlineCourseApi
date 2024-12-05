using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourseApi.Core.Models;

namespace OnlineCourseApi.Data
{
    public interface ICourseRepository
    {
        Task<List<CourseModel>> GetAllCoursesAsync(int? categoryid = null);
        Task<CourseDetailsModel> GetCourseDetailsAsync(int courseId);
    }
}
