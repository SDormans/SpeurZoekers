import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarMInfoComponent } from './avatar-m-info.component';

describe('AvatarMInfoComponent', () => {
  let component: AvatarMInfoComponent;
  let fixture: ComponentFixture<AvatarMInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvatarMInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvatarMInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
