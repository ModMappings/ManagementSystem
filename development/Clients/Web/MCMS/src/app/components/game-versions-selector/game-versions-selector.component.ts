import {Component, OnDestroy, OnInit} from '@angular/core';
import {GameVersionsService} from '../../services/game-versions.service';
import {Subject} from 'rxjs';
import {takeUntil} from 'rxjs/operators';
import {GameVersion} from '../../services/game-version';

@Component({
  selector: 'app-game-versions-selector',
  templateUrl: './game-versions-selector.component.html',
  styleUrls: ['./game-versions-selector.component.scss']
})
export class GameVersionsSelectorComponent implements OnInit, OnDestroy {

  private readonly destroyed$ = new Subject();
  private versions: GameVersion[] | null = null;

  constructor(private readonly gameVersionsService: GameVersionsService) {
  }

  ngOnInit() {
    this.gameVersionsService.versions.pipe(
      takeUntil(this.destroyed$)
    ).subscribe(versions => this.versions = versions);
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

}
