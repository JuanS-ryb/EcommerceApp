export interface SalesGoal {
  quarter: number;
  year: number;
  pesosGoal: number;
  unitsGoal: number;
  executedPesos: number;
  executedUnits: number;
  cumplimientoPesosPct: number;
  cumplimientoUnidadesPct: number;
  puntosPesos: number;
  puntosUnidades: number;
  puntosTotal: number;
  valorTotal: number;
}

export interface UpdateExecutedRequest {
  executedPesos: number;
  executedUnits: number;
}
