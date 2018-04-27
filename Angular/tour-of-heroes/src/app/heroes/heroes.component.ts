import { HEROES } from './../moc-heroes';
import { Hero } from './../hero';
import { Component, OnInit } from '@angular/core';

@Component({
  // the component's CSS element selector
  selector: 'app-heroes',
  // the location of the component's template file
  templateUrl: './heroes.component.html',
  // the location of the component's private CSS styles
  styleUrls: ['./heroes.component.css']
})
export class HeroesComponent implements OnInit {
  hero: Hero = {
    id: 1,
    name: 'Tito Gomez'
  };
  selectedHero: Hero;
  heroes = HEROES;

  constructor() { }

  ngOnInit() {
    this.heroes.push(this.hero);
  }

  onSelect(hero: Hero): void {
    this.selectedHero = hero;
  }
}
