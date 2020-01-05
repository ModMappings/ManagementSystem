import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GameVersionsSelectorComponent } from './game-versions-selector.component';

describe('GameVersionsSelectorComponent', () => {
  let component: GameVersionsSelectorComponent;
  let fixture: ComponentFixture<GameVersionsSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameVersionsSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameVersionsSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
