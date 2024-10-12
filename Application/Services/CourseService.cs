using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;

namespace Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<CourseViewModel>> GetCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync();

        var courseViewModels = new List<CourseViewModel>();

        foreach (var course in courses)
        {
            courseViewModels.Add(new CourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Teacher = course.Teacher,
                Hours = course.Hours
            });
        }

        return courseViewModels;
    }
}