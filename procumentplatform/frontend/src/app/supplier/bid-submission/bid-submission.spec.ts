import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BidSubmission } from './bid-submission';

describe('BidSubmission', () => {
  let component: BidSubmission;
  let fixture: ComponentFixture<BidSubmission>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BidSubmission]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BidSubmission);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
