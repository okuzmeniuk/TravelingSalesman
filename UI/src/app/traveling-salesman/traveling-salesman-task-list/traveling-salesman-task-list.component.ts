import { Component, input } from '@angular/core';
import { TravelingSalesmanTask } from '../traveling-salesman-task/traveling-salesman-task.model';
import { TravelingSalesmanTaskComponent } from '../traveling-salesman-task/traveling-salesman-task.component';

@Component({
  selector: 'app-traveling-salesman-task-list',
  standalone: true,
  imports: [TravelingSalesmanTaskComponent],
  templateUrl: './traveling-salesman-task-list.component.html',
  styleUrl: './traveling-salesman-task-list.component.css',
})
export class TravelingSalesmanTaskListComponent {
  tasks = input<TravelingSalesmanTask[]>([]);
}
