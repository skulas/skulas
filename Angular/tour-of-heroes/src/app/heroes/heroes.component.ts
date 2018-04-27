import { Component, OnInit } from '@angular/core';
import { Hero } from './../hero';
import {NgModel} from '@angular/forms';

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
    id : 1,
    name : 'Tito Gomez'
  };

  constructor() { }

  ngOnInit() {
  }

}
