import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import {
  IonContent,
  IonHeader,
  IonTitle,
  IonToolbar,
  IonButtons,
  IonBackButton,
  IonButton,
  IonItem,
  IonInput,
  IonSpinner,
  IonLabel,
  IonCard,
  IonCardHeader,
  IonCardTitle,
  IonCardContent,
  IonList,
  IonNote,
  ToastController
} from '@ionic/angular';
import { SalesGoalService } from '../../core/services/sales-goal.service';
import { SalesGoal } from '../../core/models/sales-goal.model';

@Component({
  selector: 'app-points',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    IonContent,
    IonHeader,
    IonTitle,
    IonToolbar,
    IonButtons,
    IonBackButton,
    IonButton,
    IonItem,
    IonInput,
    IonSpinner,
    IonLabel,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardContent,
    IonList,
    IonNote
  ],
  templateUrl: './points.page.html',
  styleUrls: ['./points.page.scss']
})
export class PointsPage implements OnInit {
  form: FormGroup;

  // signals en vez de propiedades planas: en modo zoneless, mutar
  // "this.loading = false" no repinta la vista. .set() sí, porque
  // Angular está suscrito directamente a la señal, no depende de zone.js.
  result = signal<SalesGoal | null>(null);
  loading = signal(true);

  constructor(
    private fb: FormBuilder,
    private salesGoalService: SalesGoalService,
    private toastController: ToastController
  ) {
    this.form = this.fb.group({
      executedPesos: [0, [Validators.required, Validators.min(0)]],
      executedUnits: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.salesGoalService
      .getCurrent()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (data) => this.applyResult(data),
        error: () => this.showToast('No se pudo cargar tus puntos.', 'danger')
      });
  }

  async onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.salesGoalService.updateExecuted(this.form.value).subscribe({
      next: (data) => {
        this.applyResult(data);
        this.showToast('Puntos actualizados.', 'success');
      },
      error: () => this.showToast('No se pudo calcular los puntos. Intenta de nuevo.', 'danger')
    });
  }

  formatPesos(value: number): string {
    return value.toLocaleString('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 });
  }

  private applyResult(data: SalesGoal): void {
    this.result.set(data);
    this.form.patchValue(
      {
        executedPesos: data.executedPesos,
        executedUnits: data.executedUnits
      },
      { emitEvent: false }
    );
  }

  private async showToast(message: string, color: 'success' | 'danger'): Promise<void> {
    const toast = await this.toastController.create({ message, duration: 2000, color });
    await toast.present();
  }
}
