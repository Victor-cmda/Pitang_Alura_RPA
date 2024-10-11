namespace Application.Interfaces;

public interface IRpaService
{
    Task ExecuteAsync(string filter);
}