import {
  Component,
  input,
  OnInit,
  WritableSignal,
  signal,
} from '@angular/core';
import { TravelingSalesmanTask } from './traveling-salesman-task.model';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatListModule } from '@angular/material/list';
import { DatePipe } from '@angular/common';
import { TravelingSalesmanService } from '../traveling-salesman.service';
import { catchError, interval, switchMap, takeWhile, tap } from 'rxjs';

@Component({
  selector: 'app-traveling-salesman-task',
  standalone: true,
  imports: [MatCardModule, MatProgressBarModule, DatePipe, MatListModule],
  templateUrl: './traveling-salesman-task.component.html',
  styleUrls: ['./traveling-salesman-task.component.css'],
})
export class TravelingSalesmanTaskComponent implements OnInit {
  taskInput = input.required<TravelingSalesmanTask>();
  task!: WritableSignal<TravelingSalesmanTask>;
  progress = signal<number>(0);

  constructor(public travelingSalesmanService: TravelingSalesmanService) {}

  ngOnInit(): void {
    this.task = signal<TravelingSalesmanTask>(this.taskInput());
    if (this.task()?.status === 'running') {
      this.startProgressUpdate();
    }
  }

  startProgressUpdate() {
    interval(500)
      .pipe(
        switchMap(() =>
          this.travelingSalesmanService.getProgress(this.task().id)
        ),
        tap((progress: number) => {
          this.progress.set(progress);
        }),
        takeWhile((progress: number) => progress < 100, true)
      )
      .subscribe({
        next: (progress: number) => {
          if (progress >= 100) {
            this.fetchResult();
          }
        },
        error: (err) => console.error('Error fetching progress:', err),
      });
  }

  fetchResult() {
    this.travelingSalesmanService.getResult(this.task().id).subscribe({
      next: (result) => {
        const updatedTask: TravelingSalesmanTask = {
          ...this.task(),
          status: 'complete',
          path: result.path,
          totalDistance: result.totalDistance,
          computedAt: result.computedAt,
        };
        this.task.set(updatedTask);
      },
      error: (err) => console.error('Error fetching result:', err),
    });
  }
}
