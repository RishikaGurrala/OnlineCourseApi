using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseApi.Core.Models
{
    public class CourseDetailsModel: CourseModel
    {
        public List<UserReviewModel> Reviews { get; set; }  = new List<UserReviewModel>();
        public List<SessionDetailsModel> SessionDetails { get; set; } = new List<SessionDetailsModel>();
    }
    public class CourseModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string CourseType { get; set; } = null!;
        public int? SeatsAvailable { get; set; }
        public decimal Duration { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CourseCategoryModel Category { get; set; } = null!;
        public UserRatingModel UserRating { get; set;}
    }

    public class UserRatingModel
    {
        public int CourseId { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRating { get; set; }
    }
    public class UserReviewModel
    {
        public int CourseId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime ReviewDate { get; set; }

    }
    public class SessionDetailsModel
    {
        public int SessionId { get; set; }
        public int CourseId {  get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
        public int VideoOrder { get; set; }

    }
}
