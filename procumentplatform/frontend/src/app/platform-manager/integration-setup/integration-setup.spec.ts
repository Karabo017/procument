import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IntegrationSetup } from './integration-setup';

describe('IntegrationSetup', () => {
  let component: IntegrationSetup;
  let fixture: ComponentFixture<IntegrationSetup>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IntegrationSetup]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IntegrationSetup);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
