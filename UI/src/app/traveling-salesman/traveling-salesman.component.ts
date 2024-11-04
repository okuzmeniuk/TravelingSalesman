import { Component } from '@angular/core';
import { TravelingSalesmanInputComponent } from './traveling-salesman-input/traveling-salesman-input.component';
import { TravelingSalesmanTaskListComponent } from './traveling-salesman-task-list/traveling-salesman-task-list.component';

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
export class TravelingSalesmanComponent {}
