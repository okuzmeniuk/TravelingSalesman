import { Point } from '../point.model';

export interface TravelingSalesmanResult {
  Id: string;
  Path: Point[];
  TotalDistance: number;
  ComputedAt: Date;
}
