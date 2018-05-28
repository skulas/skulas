import { HEROES } from './moc-heroes';
import { Hero } from './hero';
import { Injectable } from '@angular/core';

@Injectable()
export class HeroService {

  constructor() { }

  getHeroes(): Hero[] {
    return HEROES;
  }

}
