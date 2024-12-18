import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { TravelingSalesmanInput } from './traveling-salesman-input/traveling-salesman-input.model';
import { TravelingSalesmanResult } from './traveling-salesman-task/traveling-salesman-result.model';
import { Point } from './point.model';

@Injectable({
  providedIn: 'root',
})
export class TravelingSalesmanService {
  private apiUrl = 'https://localhost:8071/api/traveling-salesman';

  constructor(private http: HttpClient) {}

  getInputHistory(): Observable<TravelingSalesmanInput[]> {
    return this.http.get<TravelingSalesmanInput[]>(`${this.apiUrl}/history`);
  }

  getProgress(id: string): Observable<number> {
    return this.http
      .get<{ id: string; progress: number }>(`${this.apiUrl}/progress/${id}`)
      .pipe(map((response) => response.progress));
  }

  getResult(id: string): Observable<TravelingSalesmanResult> {
    return this.http.get<TravelingSalesmanResult>(
      `${this.apiUrl}/result/${id}`
    );
  }

  postSolve(points: Point[]): Observable<TravelingSalesmanInput> {
    return this.http.post<TravelingSalesmanInput>(
      `${this.apiUrl}/solve`,
      points
    );
  }
}
