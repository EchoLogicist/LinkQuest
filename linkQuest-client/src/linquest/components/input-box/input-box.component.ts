import { Component, Input, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-input-box',
  standalone: true,
  imports: [],
  templateUrl: './input-box.component.html',
  styleUrl: './input-box.component.scss',
  encapsulation: ViewEncapsulation.ShadowDom
})
export class InputBoxComponent {
@Input() value:any=''

}
