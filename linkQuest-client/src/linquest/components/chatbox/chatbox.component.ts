import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LinqestService } from '../../services/linqest.service';
import {  Observable, of, tap } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-chatbox',
  standalone: true,
  imports: [FormsModule,AsyncPipe],
  templateUrl: './chatbox.component.html',
  styleUrl: './chatbox.component.scss'
})
export class ChatboxComponent implements OnInit{

  constructor(private linqService: LinqestService) {  }
  myMessage:string=''
  userName: string|null=''
  chatsList$:Observable<any> = of([])
  ngOnInit(): void {
    this.userName=sessionStorage.getItem('playername')
    this.getChats()
  }
getChats(){
  this.chatsList$=this.linqService.chatObject$.pipe(tap(i=>console.log(i)))
}

sendMessage(){
  this.myMessage.trim()!=''&&this.linqService.sendMessae({RoomName: sessionStorage.getItem('roomname'), UserName: sessionStorage.getItem('playername'),message:this.myMessage})
  this.myMessage=''
}
}
