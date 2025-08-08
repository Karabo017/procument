import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TenderDetail } from './tender-detail';

describe('TenderDetail', () => {
  let component: TenderDetail;
  let fixture: ComponentFixture<TenderDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenderDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TenderDetail);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
