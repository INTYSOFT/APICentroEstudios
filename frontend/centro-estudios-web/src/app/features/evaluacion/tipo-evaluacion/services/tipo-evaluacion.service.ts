import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { CENTRO_ESTUDIOS_API_ROUTES } from '@core/constants/centro-estudios-api.constants';
import { TipoEvaluacion, TipoEvaluacionUpsertRequest } from '../models/tipo-evaluacion.model';

@Injectable({ providedIn: 'root' })
export class TipoEvaluacionService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = CENTRO_ESTUDIOS_API_ROUTES.tipoEvaluacion;

  getAll(): Observable<TipoEvaluacion[]> {
    return this.http.get<TipoEvaluacion[]>(this.baseUrl);
  }

  create(payload: TipoEvaluacionUpsertRequest): Observable<TipoEvaluacion> {
    return this.http.post<TipoEvaluacion>(this.baseUrl, payload);
  }

  update(id: number, payload: TipoEvaluacionUpsertRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, { id, ...payload });
  }
}
