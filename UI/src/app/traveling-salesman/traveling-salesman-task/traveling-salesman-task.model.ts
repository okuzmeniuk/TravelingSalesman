import { Point } from '../point.model';

export interface TravelingSalesmanTask {
  id: string;
  inputPoints: Point[];
  createdAt: Date;
  status?: 'running' | 'complete' | 'canceled';
  path?: Point[];
  totalDistance?: number;
  computedAt?: Date;
}
