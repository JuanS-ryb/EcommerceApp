namespace Ecommerce.server.services;

/// <summary>
/// Reglas de negocio de la "Prueba de análisis". Separado del servicio que
/// toca la base de datos a propósito: esto es lógica pura, fácil de probar
/// con un test unitario sin necesidad de un DbContext.
/// </summary>
public static class SalesPointsCalculator
{
    public const int PointValue = 1500; // 1 punto = $1.500

    /// <summary>
    /// Puntos por cumplimiento de la cuota EN PESOS, según % de cumplimiento.
    /// </summary>
    public static int CalcularPuntosPesos(decimal executedPesos, decimal pesosGoal)
    {
        var pct = CalcularPorcentaje(executedPesos, pesosGoal);

        return pct switch
        {
            >= 80 => 100,
            >= 50 => 70,
            >= 30 => 40,
            >= 10 => 20,
            _ => 0
        };
    }

    /// <summary>
    /// Puntos por cumplimiento de la cuota EN UNIDADES vendidas.
    ///
    /// NOTA: el enunciado original dice "-999 productos vendidos = 100 puntos",
    /// lo cual no tiene sentido (valdría más que vender 1.000-2.999 = 50 puntos).
    /// Se asume que es un error de digitación y se deja en 0. Si el evaluador
    /// confirma que el valor real es 100, solo hay que cambiar el "_ => 0" de abajo.
    /// </summary>
    public static int CalcularPuntosUnidades(int executedUnits)
    {
        return executedUnits switch
        {
            >= 4000 => 150,
            >= 2000 => 100,
            >= 1000 => 50,
            _ => 0
        };
    }

    public static double CalcularPorcentaje(decimal executed, decimal goal)
    {
        if (goal == 0) return 0;
        return Math.Round((double)(executed / goal) * 100, 2);
    }
}
