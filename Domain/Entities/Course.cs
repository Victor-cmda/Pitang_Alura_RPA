namespace Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Teacher { get; set; }
    public string Hours { get; set; }
    public string Description { get; set; }
}