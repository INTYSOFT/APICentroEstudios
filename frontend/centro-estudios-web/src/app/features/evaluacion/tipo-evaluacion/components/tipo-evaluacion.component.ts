import { ChangeDetectionStrategy, Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AgGridModule } from 'ag-grid-angular';
import { ColDef, GridReadyEvent } from 'ag-grid-community';
import { Dialog, DialogModule } from '@angular/cdk/dialog';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { EMPTY, filter, finalize, startWith, switchMap, tap } from 'rxjs';

import { TipoEvaluacionService } from '../services/tipo-evaluacion.service';
import { TipoEvaluacion } from '../models/tipo-evaluacion.model';
import {
  TipoEvaluacionDialogComponent,
  TipoEvaluacionDialogData,
  TipoEvaluacionDialogResult
} from './tipo-evaluacion.dialog';

@Component({
  selector: 'app-tipo-evaluacion',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AgGridModule, DialogModule],
  templateUrl: './tipo-evaluacion.component.html',
  styleUrls: ['./tipo-evaluacion.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TipoEvaluacionComponent {
  private readonly tipoEvaluacionService = inject(TipoEvaluacionService);
  private readonly dialog = inject(Dialog);
  private readonly destroyRef = inject(DestroyRef);

  readonly searchControl = new FormControl('', { nonNullable: true });
  private readonly searchTerm = toSignal(
    this.searchControl.valueChanges.pipe(startWith(this.searchControl.value)),
    { initialValue: '' }
  );

  readonly loading = signal(false);
  readonly tipoEvaluaciones = signal<TipoEvaluacion[]>([]);
  readonly errorMessage = signal<string | null>(null);

  readonly filteredTipoEvaluaciones = computed(() => {
    const query = this.searchTerm()?.trim().toLowerCase() ?? '';
    const data = this.tipoEvaluaciones();

    if (!query) {
      return data;
    }

    return data.filter((tipo) => {
      const nombre = tipo.nombre?.toLowerCase() ?? '';
      const descripcion = tipo.descripcion?.toLowerCase() ?? '';
      return nombre.includes(query) || descripcion.includes(query);
    });
  });

  readonly columnDefs: ColDef<TipoEvaluacion>[] = [
    {
      headerName: 'Nombre',
      field: 'nombre',
      flex: 1,
      minWidth: 200,
      sortable: true,
      filter: 'agTextColumnFilter'
    },
    {
      headerName: 'Descripción',
      field: 'descripcion',
      flex: 1.5,
      minWidth: 250,
      sortable: true,
      filter: 'agTextColumnFilter'
    },
    {
      headerName: 'Estado',
      field: 'activo',
      maxWidth: 140,
      valueFormatter: (params) => (params.value ? 'Activo' : 'Inactivo')
    },
    {
      headerName: 'Última actualización',
      field: 'fechaActualizacion',
      minWidth: 200,
      valueFormatter: (params) =>
        params.value ? new Date(params.value).toLocaleString() : '—'
    }
  ];

  readonly defaultColDef: ColDef<TipoEvaluacion> = {
    resizable: true,
    suppressMovable: true,
    sortable: true,
    filter: true
  };

  constructor() {
    this.loadTipoEvaluaciones();
  }

  onGridReady(event: GridReadyEvent<TipoEvaluacion>): void {
    event.api.sizeColumnsToFit();
  }

  onCreate(): void {
    this.openDialog('create');
  }

  onEditSelected(tipoEvaluacion: TipoEvaluacion): void {
    this.openDialog('update', tipoEvaluacion);
  }

  refresh(): void {
    this.loadTipoEvaluaciones();
  }

  private loadTipoEvaluaciones(): void {
    this.loading.set(true);
    this.errorMessage.set(null);

    this.tipoEvaluacionService
      .getAll()
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        tap((data) => this.tipoEvaluaciones.set(data)),
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        error: () => this.errorMessage.set('No se pudieron cargar los tipos de evaluación.')
      });
  }

  private openDialog(mode: 'create' | 'update', tipo?: TipoEvaluacion): void {
    const dialogRef = this.dialog.open<
      TipoEvaluacionDialogResult,
      TipoEvaluacionDialogData
    >(TipoEvaluacionDialogComponent, {
      data: {
        mode,
        tipoEvaluacion: tipo ?? null
      }
    });

    dialogRef.closed
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        filter((result): result is TipoEvaluacionDialogResult => !!result),
        switchMap((result) => {
          this.loading.set(true);

          if (result.mode === 'create') {
            return this.tipoEvaluacionService.create(result.payload).pipe(
              tap((created) => {
                this.errorMessage.set(null);
                this.tipoEvaluaciones.update((current) => [...current, created]);
              })
            );
          }

          if (!tipo) {
            return EMPTY;
          }

          return this.tipoEvaluacionService.update(tipo.id, result.payload).pipe(
            tap(() => {
              this.errorMessage.set(null);
              this.tipoEvaluaciones.update((current) =>
                current.map((item) =>
                  item.id === tipo.id
                    ? {
                        ...item,
                        ...result.payload,
                        fechaActualizacion: new Date().toISOString()
                      }
                    : item
                )
              );
            })
          );
        }),
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        error: () => this.errorMessage.set('No se pudo guardar la información. Intenta nuevamente.')
      });
  }
}
