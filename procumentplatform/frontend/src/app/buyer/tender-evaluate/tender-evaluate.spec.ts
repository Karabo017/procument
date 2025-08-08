import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TenderEvaluate } from './tender-evaluate';

describe('TenderEvaluate', () => {
  let component: TenderEvaluate;
  let fixture: ComponentFixture<TenderEvaluate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenderEvaluate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TenderEvaluate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
