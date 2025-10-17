import { Routes } from '@angular/router';

export const appRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'evaluacion/tipo-evaluacion'
  },
  {
    path: 'evaluacion',
    loadChildren: () =>
      import('./features/evaluacion/evaluacion.routes').then((m) => m.EVALUACION_ROUTES)
  },
  {
    path: '**',
    redirectTo: 'evaluacion/tipo-evaluacion'
  }
];
