using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCourseApi.Core.Entities;
using OnlineCourseApi.Data.Entities;

namespace OnlineCourseApi.Data
{
    public class CourseCategoryRepository/*(OnlineCourseDbContext dbcontext)*/:ICourseCategoryRepository
    {
        private readonly OnlineCourseDbContext _dbcontext;
        public CourseCategoryRepository(OnlineCourseDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        

        public Task<CourseCategory?> GetByIdAsync(int id)
        {
           var data = _dbcontext.CourseCategories.FindAsync(id).AsTask();
            return data;
        }
        public Task<List<CourseCategory>> GetCourseCategoriesAsync()
        {
            var data = _dbcontext.CourseCategories.ToListAsync();
            return data;
        }
    }
}
