import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YachtsListsComponent } from './yachts-lists.component';

describe('YachtsListsComponent', () => {
  let component: YachtsListsComponent;
  let fixture: ComponentFixture<YachtsListsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [YachtsListsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YachtsListsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
