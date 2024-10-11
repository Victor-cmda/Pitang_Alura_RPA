using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RpaController : ControllerBase
{
    private readonly IRpaService _rpaService;

    public RpaController(IRpaService rpaService)
    {
        _rpaService = rpaService;
    }
    
    [HttpPost("execute")]
    public async Task<IActionResult> ExecutarAutomacao([FromBody] string termoBusca)
    {
        if (string.IsNullOrWhiteSpace(termoBusca))
        {
            return BadRequest("O termo de busca não pode ser vazio.");
        }

        await _rpaService.ExecuteAsync(termoBusca);
        return Ok("Automação executada com sucesso.");
    }


    [HttpGet("courses")]
    public async Task<IActionResult> ObterCursos([FromServices] ICursoService cursoService)
    {
        var cursos = await cursoService.GetCoursesAsync();
        return Ok(cursos);
    }
}