import { Routes } from '@angular/router';

export const EVALUACION_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'tipo-evaluacion'
  },
  {
    path: 'tipo-evaluacion',
    loadComponent: () =>
      import('./tipo-evaluacion/components/tipo-evaluacion.component').then(
        (m) => m.TipoEvaluacionComponent
      )
  }
];
