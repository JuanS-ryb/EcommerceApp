using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Ecommerce.server.services;
using Ecommerce.server.services.interfaces;
using Ecommerce.server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.server.Controllers;

// [controller] toma "SalesGoals" del nombre de la clase -> ruta final: api/SalesGoals
// Coincide con lo que ya usas en Angular: ENDPOINT_LOGIN = "Auth/login" viene de AuthController.
[ApiController]
[Route("api/[controller]")]
[Authorize] // el usuario debe estar logueado: los puntos son SUYOS, salen del JWT
public class SalesGoalsController(ISalesGoalService salesGoalService, JwtUtils jwt) : ControllerBase
{
    private readonly ISalesGoalService _salesGoalService = salesGoalService;

    // GET /api/SalesGoals/current
    [HttpGet("current")]
    public async Task<ActionResult<SalesGoalDto>> GetCurrent()
    {
        int? userId = jwt.GetIdByToken();

        if (userId is null) return BadRequest("No usuario encontrado");

        var result = await _salesGoalService.GetCurrentAsync(userId.Value);
        return Ok(result);
    }

    // PUT /api/SalesGoals/current
    [HttpPut("current")]
    public async Task<ActionResult<SalesGoalDto>> UpdateCurrent(UpdateExecutedRequestDto request)
    {
        int? userId = jwt.GetIdByToken();

        if (userId is null) return BadRequest("No usuario encontrado");

        var result = await _salesGoalService.UpdateExecutedAsync(userId.Value, request);
        return Ok(result);
    }
}
