import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarMineComponent } from './avatar-mine.component';

describe('AvatarMineComponent', () => {
  let component: AvatarMineComponent;
  let fixture: ComponentFixture<AvatarMineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvatarMineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvatarMineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
