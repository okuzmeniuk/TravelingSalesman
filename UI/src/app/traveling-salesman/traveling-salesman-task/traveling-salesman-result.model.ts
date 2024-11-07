import { Point } from '../point.model';

export interface TravelingSalesmanResult {
  id: string;
  path: Point[];
  totalDistance: number;
  computedAt: Date;
}
