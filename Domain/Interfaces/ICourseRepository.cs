using Domain.Entities;

namespace Domain.Interfaces;

public interface ICourseRepository
{
    Task AddAsync(Course course);
    Task<IEnumerable<Course>> GetAllAsync();
    Task<bool> ExistsAsync(string title);
}