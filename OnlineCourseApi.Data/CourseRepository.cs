using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCourseApi.Core.Entities;
using OnlineCourseApi.Core.Models;
using OnlineCourseApi.Data.Entities;

namespace OnlineCourseApi.Data
{
    public class CourseRepository : ICourseRepository
    {
        private readonly OnlineCourseDbContext _context;
        public CourseRepository(OnlineCourseDbContext context)
        {
            _context = context;   
        }
        public async Task<List<CourseModel>> GetAllCoursesAsync(int? categoryid = null)
        {
            var query = _context.Courses
                .Include(c => c.Category)
                .AsQueryable();
            if (categoryid.HasValue)
            {
                query = query.Where(c=> c.CategoryId == categoryid.Value);
            }
            var courses = await query
                .Select(c => new CourseModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    CourseType = c.CourseType,
                    SeatsAvailable = c.SeatsAvailable,
                    Duration = c.Duration,
                    CategoryId = c.CategoryId,
                    InstructorId = c.InstructorId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Category = new CourseCategoryModel()
                    {
                        CategoryId = c.Category.CategoryId,
                        CategoryName = c.Category.CategoryName,
                        Description = c.Category.Description,
                    },
                    UserRating = new UserRatingModel
                    {
                        CourseId = c.CourseId,
                        AverageRating = c.Reviews.Any() ? Convert.ToDecimal(c.Reviews.Average(r => r.Rating)) : 0,
                        TotalRating = c.Reviews.Count,
                    }
                }).ToListAsync();
            return courses;
        }

        public async Task<CourseDetailsModel> GetCourseDetailsAsync(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Reviews)
                .Include(c => c.SessionDetails)
                .Where(c => c.CourseId == courseId)
                .Select(c => new CourseDetailsModel()
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    CourseType = c.CourseType,
                    SeatsAvailable = c.SeatsAvailable,
                    Duration = c.Duration,
                    CategoryId = c.CategoryId,
                    InstructorId = c.InstructorId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Category = new CourseCategoryModel()
                    {
                        CategoryId = c.Category.CategoryId,
                        CategoryName = c.Category.CategoryName,
                        Description = c.Category.Description,
                    },
                    Reviews = c.Reviews.Select(r => new UserReviewModel
                    {
                        CourseId = r.CourseId,
                        UserName = r.User.DisplayName,
                        Rating = r.Rating,
                        Comments = r.Comments,
                        ReviewDate = r.ReviewDate
                    }).OrderByDescending(o => o.Rating).Take(10).ToList(),
                    UserRating = new UserRatingModel
                    {
                        CourseId = c.CourseId,
                        AverageRating = c.Reviews.Any() ? Convert.ToDecimal(c.Reviews.Average(r => r.Rating)) : 0,
                        TotalRating = c.Reviews.Count,
                    }
                }).FirstOrDefaultAsync();
            return course;
        }
    }
}
