export interface INotification {
  name: string;
  description: string;
  message: string;
  monthAge: number;
  calendarVaccinationId: string;
}

export interface ApiResponse {
  success: boolean;
  data: INotification[];
  message: string;
}
