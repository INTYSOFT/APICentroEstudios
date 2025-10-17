import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { DialogRef, DIALOG_DATA } from '@angular/cdk/dialog';
import { NgIf } from '@angular/common';

import { TipoEvaluacion, TipoEvaluacionUpsertRequest } from '../models/tipo-evaluacion.model';

export interface TipoEvaluacionDialogData {
  mode: 'create' | 'update';
  tipoEvaluacion: TipoEvaluacion | null;
}

export interface TipoEvaluacionDialogResult {
  mode: 'create' | 'update';
  payload: TipoEvaluacionUpsertRequest;
}

@Component({
  selector: 'app-tipo-evaluacion-dialog',
  standalone: true,
  templateUrl: './tipo-evaluacion.dialog.html',
  styleUrls: ['./tipo-evaluacion.dialog.scss'],
  imports: [CommonModule, ReactiveFormsModule, NgIf],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TipoEvaluacionDialogComponent {
  private readonly dialogRef = inject(DialogRef<TipoEvaluacionDialogResult>);
  private readonly data = inject<TipoEvaluacionDialogData>(DIALOG_DATA);
  private readonly formBuilder = inject(FormBuilder);

  readonly title = this.data.mode === 'create' ? 'Registrar tipo de evaluación' : 'Editar tipo de evaluación';
  readonly submitLabel = this.data.mode === 'create' ? 'Registrar' : 'Actualizar';

  readonly form: FormGroup<{
    nombre: FormControl<string>;
    descripcion: FormControl<string | null>;
    activo: FormControl<boolean>;
  }> = this.formBuilder.group({
    nombre: this.formBuilder.nonNullable.control('', [Validators.required, Validators.maxLength(150)]),
    descripcion: this.formBuilder.control<string | null>(null, [Validators.maxLength(250)]),
    activo: this.formBuilder.nonNullable.control(true)
  });

  constructor() {
    if (this.data.tipoEvaluacion) {
      this.form.patchValue({
        nombre: this.data.tipoEvaluacion.nombre,
        descripcion: this.data.tipoEvaluacion.descripcion ?? null,
        activo: this.data.tipoEvaluacion.activo
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload: TipoEvaluacionUpsertRequest = {
      nombre: this.form.value.nombre ?? '',
      descripcion: this.form.value.descripcion ?? null,
      activo: this.form.value.activo ?? true
    };

    this.dialogRef.close({
      mode: this.data.mode,
      payload
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
