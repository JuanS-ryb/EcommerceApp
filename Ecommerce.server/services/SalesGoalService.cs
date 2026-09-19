using Ecommerce.server.Context;
using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Ecommerce.server.services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.server.services;

public class SalesGoalService : ISalesGoalService
{
    private const decimal DefaultPesosGoal = 11_000_000m;
    private const int DefaultUnitsGoal = 6_000;

    private readonly AppDbContext _context;

    public SalesGoalService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SalesGoalDto> GetCurrentAsync(int userId)
    {
        var goal = await GetOrCreateCurrentAsync(userId);
        return BuildDto(goal);
    }

    public async Task<SalesGoalDto> UpdateExecutedAsync(int userId, UpdateExecutedRequestDto request)
    {
        var goal = await GetOrCreateCurrentAsync(userId);

        goal.ExecutedPesos = request.ExecutedPesos;
        goal.ExecutedUnits = request.ExecutedUnits;

        await _context.SaveChangesAsync();

        return BuildDto(goal);
    }

    /// <summary>
    /// Busca la meta del usuario para el trimestre/año actuales; si no existe
    /// todavía (primera vez que entra), la crea con las metas por defecto.
    /// </summary>
    private async Task<SalesGoal> GetOrCreateCurrentAsync(int userId)
    {
        var now = DateTime.UtcNow;
        var quarter = (byte)((now.Month - 1) / 3 + 1);
        var year = (short)now.Year;

        var goal = await _context.SalesGoals
            .FirstOrDefaultAsync(g => g.UserId == userId && g.Quarter == quarter && g.Year == year);

        if (goal is null)
        {
            goal = new SalesGoal
            {
                UserId = userId,
                Quarter = quarter,
                Year = year,
                PesosGoal = DefaultPesosGoal,
                UnitsGoal = DefaultUnitsGoal,
                ExecutedPesos = 0,
                ExecutedUnits = 0
            };

            _context.SalesGoals.Add(goal);
            await _context.SaveChangesAsync();
        }

        return goal;
    }

    private static SalesGoalDto BuildDto(SalesGoal goal)
    {
        var puntosPesos = SalesPointsCalculator.CalcularPuntosPesos(goal.ExecutedPesos, goal.PesosGoal);
        var puntosUnidades = SalesPointsCalculator.CalcularPuntosUnidades(goal.ExecutedUnits);
        var puntosTotal = puntosPesos + puntosUnidades;

        return new SalesGoalDto
        {
            Quarter = goal.Quarter,
            Year = goal.Year,
            PesosGoal = goal.PesosGoal,
            UnitsGoal = goal.UnitsGoal,
            ExecutedPesos = goal.ExecutedPesos,
            ExecutedUnits = goal.ExecutedUnits,
            CumplimientoPesosPct = SalesPointsCalculator.CalcularPorcentaje(goal.ExecutedPesos, goal.PesosGoal),
            CumplimientoUnidadesPct = SalesPointsCalculator.CalcularPorcentaje(goal.ExecutedUnits, goal.UnitsGoal),
            PuntosPesos = puntosPesos,
            PuntosUnidades = puntosUnidades,
            PuntosTotal = puntosTotal,
            ValorTotal = puntosTotal * SalesPointsCalculator.PointValue
        };
    }
}
