import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VaccinationCalendarInfoComponent } from './vaccination-calendar-info.component';

describe('VaccinationCalendarInfoComponent', () => {
  let component: VaccinationCalendarInfoComponent;
  let fixture: ComponentFixture<VaccinationCalendarInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VaccinationCalendarInfoComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(VaccinationCalendarInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
