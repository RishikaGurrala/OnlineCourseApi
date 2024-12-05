using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourseApi.Core.Models;
using OnlineCourseApi.Data;

namespace OnlineCourseApi.Service
{
    public class CourseCategoryService : ICourseCategoryService
    {
        private readonly ICourseCategoryRepository courseCategoryRepository;
        public CourseCategoryService(ICourseCategoryRepository courseCategoryRepository)
        {
            this.courseCategoryRepository = courseCategoryRepository; 
        }
        public async Task<CourseCategoryModel?> GetByIdAsync(int id)
        {
            var data = await courseCategoryRepository.GetByIdAsync(id);
            return data == null ? null : new CourseCategoryModel()
            {
                CategoryId = data.CategoryId,
                CategoryName = data.CategoryName,
                Description = data.Description,
            };
        }

        public async Task<List<CourseCategoryModel>> GetCourseCategory()
        {
            var data = await courseCategoryRepository.GetCourseCategoriesAsync();
            var modeldata = data.Select(x => new CourseCategoryModel() 
            { 
                CategoryId = x.CategoryId, 
                CategoryName = x.CategoryName, 
                Description = x.Description, 
            }).ToList();
            return modeldata;
        }
    }
}
