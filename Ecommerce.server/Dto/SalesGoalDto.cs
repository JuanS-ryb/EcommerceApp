namespace Ecommerce.server.Dto;

public class SalesGoalDto
{
    public byte Quarter { get; set; }
    public short Year { get; set; }

    public decimal PesosGoal { get; set; }
    public int UnitsGoal { get; set; }

    public decimal ExecutedPesos { get; set; }
    public int ExecutedUnits { get; set; }

    public double CumplimientoPesosPct { get; set; }
    public double CumplimientoUnidadesPct { get; set; }

    public int PuntosPesos { get; set; }
    public int PuntosUnidades { get; set; }
    public int PuntosTotal { get; set; }
    public decimal ValorTotal { get; set; } // PuntosTotal * valor del punto
}

public class UpdateExecutedRequestDto
{
    public decimal ExecutedPesos { get; set; }
    public int ExecutedUnits { get; set; }
}
