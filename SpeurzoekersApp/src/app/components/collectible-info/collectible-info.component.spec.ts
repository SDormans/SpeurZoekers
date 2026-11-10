import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectibleInfoComponent } from './collectible-info.component';

describe('CollectibleInfoComponent', () => {
  let component: CollectibleInfoComponent;
  let fixture: ComponentFixture<CollectibleInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CollectibleInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CollectibleInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
