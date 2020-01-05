import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {GameVersion} from './game-version';

function v(name: string, maintained: boolean = false): GameVersion {
  return {
    displayName: name,
    maintained
  };
}

@Injectable({
  providedIn: 'root'
})
export class GameVersionsService {

  private readonly versions$: Observable<GameVersion[]> = of(
    [v('1.12'), v('1.13'), v('1.14', true), v('1.15', true)]
  );

  constructor() {
  }

  get versions(): Observable<GameVersion[]> {
    return this.versions$;
  }
}
