using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;

namespace Application.Services;

public class CourseService : ICursoService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<CourseViewModel>> GetCoursesAsync()
    {
        var cursos = await _courseRepository.GetAllAsync();

        var cursosViewModel = new List<CourseViewModel>();

        foreach (var curso in cursos)
        {
            cursosViewModel.Add(new CourseViewModel
            {
                Id = curso.Id,
                Title = curso.Title,
                Description = curso.Description,
                Teacher = curso.Teacher,
                Hours = curso.Hours
            });
        }

        return cursosViewModel;
    }
}