import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TenderSearch } from './tender-search';

describe('TenderSearch', () => {
  let component: TenderSearch;
  let fixture: ComponentFixture<TenderSearch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenderSearch]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TenderSearch);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
