import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageSearchComponent } from './page-search.component';

describe('PageSearchComponent', () => {
  let component: PageSearchComponent;
  let fixture: ComponentFixture<PageSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
