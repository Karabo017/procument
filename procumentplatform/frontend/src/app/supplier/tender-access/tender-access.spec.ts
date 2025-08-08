import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TenderAccess } from './tender-access';

describe('TenderAccess', () => {
  let component: TenderAccess;
  let fixture: ComponentFixture<TenderAccess>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenderAccess]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TenderAccess);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
