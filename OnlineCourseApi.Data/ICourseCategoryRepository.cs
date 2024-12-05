using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourseApi.Core.Entities;

namespace OnlineCourseApi.Data
{
    public interface ICourseCategoryRepository
    {
        Task<CourseCategory?> GetByIdAsync(int id);
        Task<List<CourseCategory>> GetCourseCategoriesAsync();
    }
}
