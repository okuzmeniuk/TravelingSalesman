import { Component, output, signal } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { TravelingSalesmanService } from '../traveling-salesman.service';
import { TravelingSalesmanInput } from './traveling-salesman-input.model';

@Component({
  selector: 'app-traveling-salesman-input',
  standalone: true,
  imports: [ReactiveFormsModule, MatInputModule, MatButtonModule],
  templateUrl: './traveling-salesman-input.component.html',
  styleUrl: './traveling-salesman-input.component.css',
})
export class TravelingSalesmanInputComponent {
  error = signal<boolean>(false);
  onRequestSend = output<TravelingSalesmanInput>();
  inputForm: FormGroup;
  pointsArray: FormArray;

  constructor(
    public travelingSalesmanService: TravelingSalesmanService,
    private fb: FormBuilder
  ) {
    this.inputForm = this.fb.group({
      n: [2, [Validators.required, Validators.min(2), Validators.max(30)]],
      points: this.fb.array([]),
    });

    this.pointsArray = this.inputForm.get('points') as FormArray;
    this.updatePointsFields(2);
  }

  updatePointsFields(n: number): void {
    this.pointsArray.clear();

    for (let i = 0; i < n; i++) {
      this.pointsArray.push(
        this.fb.group({
          X: [null, [Validators.required]],
          Y: [null, [Validators.required]],
        })
      );
    }
  }

  onPointsCountChange(): void {
    const n = this.inputForm.get('n')?.value;
    if (n >= 2 && n <= 15) {
      this.updatePointsFields(n);
    }
  }

  onSubmit(): void {
    if (this.inputForm.valid) {
      this.travelingSalesmanService
        .postSolve(this.inputForm.value.points)
        .subscribe({
          next: (value) => {
            this.onRequestSend.emit(value);
          },
          error: (err) => {
            this.error.set(true);
            console.log(err);
          },
        });
    }
  }
}
