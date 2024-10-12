using Application.ViewModels;

namespace Application.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseViewModel>> GetCoursesAsync();
}