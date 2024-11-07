import { Component, OnInit, signal } from '@angular/core';
import { TravelingSalesmanInputComponent } from './traveling-salesman-input/traveling-salesman-input.component';
import { TravelingSalesmanTaskListComponent } from './traveling-salesman-task-list/traveling-salesman-task-list.component';
import { TravelingSalesmanService } from './traveling-salesman.service';
import { TravelingSalesmanInput } from './traveling-salesman-input/traveling-salesman-input.model';
import { TravelingSalesmanTask } from './traveling-salesman-task/traveling-salesman-task.model';

@Component({
  selector: 'app-traveling-salesman',
  standalone: true,
  imports: [
    TravelingSalesmanInputComponent,
    TravelingSalesmanTaskListComponent,
  ],
  templateUrl: './traveling-salesman.component.html',
  styleUrl: './traveling-salesman.component.css',
})
export class TravelingSalesmanComponent implements OnInit {
  tasks = signal<TravelingSalesmanTask[]>([]);
  constructor(public travelingSalesmanService: TravelingSalesmanService) {}

  ngOnInit(): void {
    this.travelingSalesmanService.getInputHistory().subscribe({
      next: (value) => {
        const fetchedData = value.map((input) => {
          const taskToAdd: TravelingSalesmanTask = {
            id: input.id,
            inputPoints: input.points,
            createdAt: input.createdAt,
            status: 'running',
          };

          this.travelingSalesmanService.getResult(input.id).subscribe({
            next: (result) => {
              taskToAdd.status = 'complete';
              taskToAdd.path = result.path;
              taskToAdd.totalDistance = result.totalDistance;
              taskToAdd.computedAt = result.computedAt;
            },
          });

          return taskToAdd;
        });

        fetchedData.sort(
          (a, b) =>
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
        );
        this.tasks.set(fetchedData);
      },
      error: (err) => console.log(err),
    });
  }

  onRequestSend(input: TravelingSalesmanInput): void {
    const taskToAdd: TravelingSalesmanTask = {
      id: input.id,
      inputPoints: input.points,
      createdAt: input.createdAt,
      status: 'running',
    };

    this.tasks.set([taskToAdd, ...this.tasks()]);
  }
}
