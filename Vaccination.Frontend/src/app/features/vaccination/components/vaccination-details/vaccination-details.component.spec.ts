import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VaccinationDetailsComponent } from './vaccination-details.component';

describe('VaccinationDetailsComponent', () => {
  let component: VaccinationDetailsComponent;
  let fixture: ComponentFixture<VaccinationDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VaccinationDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VaccinationDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
