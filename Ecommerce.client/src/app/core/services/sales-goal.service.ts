import { Injectable } from '@angular/core';
import { HttpService } from '../http/http.service';
import { SalesGoal, UpdateExecutedRequest } from '../models/sales-goal.model';

const ENDPOINT_CURRENT = 'SalesGoals/current';

@Injectable({
  providedIn: 'root',
})
export class SalesGoalService {
  constructor(private http: HttpService) {}

  getCurrent() {
    return this.http.get<SalesGoal>(ENDPOINT_CURRENT);
  }

  updateExecuted(payload: UpdateExecutedRequest) {
    return this.http.put<SalesGoal>(ENDPOINT_CURRENT, payload);
  }
}
