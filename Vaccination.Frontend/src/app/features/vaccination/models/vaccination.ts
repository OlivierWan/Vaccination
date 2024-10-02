export interface IVaccinationList {
  totalCount: number;
  vaccinations: IVaccination[];
}

export interface IVaccination {
  id: string;
  vaccineName: string;
  vaccinationDate: Date;
  vaccineDescription: string;
  vaccineCalendarId: string;
}
