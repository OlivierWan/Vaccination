import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VaccinationListComponent } from './vaccination-list.component';

describe('VaccinationComponent', () => {
  let component: VaccinationListComponent;
  let fixture: ComponentFixture<VaccinationListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VaccinationListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(VaccinationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
