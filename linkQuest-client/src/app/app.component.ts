import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LinqestService } from '../linquest/services/linqest.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'linkQuest-client';

  constructor(private _linqService: LinqestService){
    
  }

  send(){
    this._linqService.sendMessage('hemanth', 'hi')
  }
}
