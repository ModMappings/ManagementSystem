import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalSearchBoxComponent } from './global-search-box.component';

describe('GlobalSearchBoxComponent', () => {
  let component: GlobalSearchBoxComponent;
  let fixture: ComponentFixture<GlobalSearchBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GlobalSearchBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GlobalSearchBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
