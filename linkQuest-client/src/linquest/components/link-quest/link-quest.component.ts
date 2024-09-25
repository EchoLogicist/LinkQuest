import { Component, OnInit } from '@angular/core';
import { LinqestService } from '../../services/linqest.service';
import { filter, map } from 'rxjs';
import { CellComponent } from "../cell/cell.component";
import { NavigationEnd, Router } from '@angular/router';
import { TimerComponent } from "../timer/timer.component";
import { ChatboxComponent } from "../chatbox/chatbox.component";

@Component({
  selector: 'app-link-quest',
  standalone: true,
  imports: [CellComponent, TimerComponent, ChatboxComponent],
  templateUrl: './link-quest.component.html',
  styleUrl: './link-quest.component.scss'
})
export class LinkQuestComponent implements OnInit{
  gameObject : any
  userObject: any
  blockEvent : boolean = false

  constructor(private linqService: LinqestService, private router: Router) {}

  ngOnInit(): void {
    this.linqService.gameObject$.pipe(map((j) => JSON.parse(j))).subscribe((res) => {
      if(res.recentUpdateCell){
        res.recentUpdateCell[res.cellInfo.Cell] = {... res.recentUpdateCell[res.cellInfo.Cell], isRecent : true}
        res.GameObject[res.cellInfo.RowIndex][res.cellInfo.ColumnIndex] = res.recentUpdateCell
      }
      this.gameObject = res.GameObject
      console.log(res)
    })

    this.linqService.usersObject$.subscribe((res) => {
      this.userObject = res
      console.log(res)
    })

    
  }

  sendMessage(message: string){
    this.linqService.sendMessae({RoomName: sessionStorage.getItem('roomname'), UserName: sessionStorage.getItem('playername'), message})
  }
}
