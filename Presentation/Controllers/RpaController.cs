using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/rpa")]
public class RpaController : ControllerBase
{
    private readonly IRpaService _rpaService;

    public RpaController(IRpaService rpaService)
    {
        _rpaService = rpaService;
    }
    
    [HttpPost("execute-service")]
    public async Task<IActionResult> ExecuteAutomationCourses([FromBody] string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return BadRequest("O termo de busca não pode ser vazio.");
        }

        await _rpaService.ExecuteAsync(filter);
        return Ok("Automação executada com sucesso.");
    }


    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses([FromServices] ICourseService courseService)
    {
        var courses = await courseService.GetCoursesAsync();
        return Ok(courses);
    }
}