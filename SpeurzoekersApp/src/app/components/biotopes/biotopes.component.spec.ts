import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BiotopesComponent } from './biotopes.component';

describe('BiotopesComponent', () => {
  let component: BiotopesComponent;
  let fixture: ComponentFixture<BiotopesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BiotopesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BiotopesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
