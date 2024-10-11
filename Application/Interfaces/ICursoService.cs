using Application.ViewModels;

namespace Application.Interfaces;

public interface ICursoService
{
    Task<IEnumerable<CourseViewModel>> GetCoursesAsync();
}