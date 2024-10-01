import { AfterViewInit, Component, ElementRef, OnInit, QueryList, ViewChildren, viewChildren } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LinqestService } from '../../services/linqest.service';
import {  BehaviorSubject, map, Observable, of, reduce, tap } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-chatbox',
  standalone: true,
  imports: [FormsModule,AsyncPipe],
  templateUrl: './chatbox.component.html',
  styleUrl: './chatbox.component.scss'
})
export class ChatboxComponent implements OnInit {

  constructor(private linqService: LinqestService) {  }
  myMessage:string=''
  userName: string|null=''
  chatsList$:Observable<any> = of([])
  localMessageList:BehaviorSubject<any>=new BehaviorSubject([
   
  ])
  localMessageList$:Observable<any>=this.localMessageList.asObservable().pipe()
  @ViewChildren('chat',{read:ElementRef}) chatElements:QueryList<ElementRef>=new QueryList<ElementRef>;
  // {userName:string,message:string}
  ngOnInit(): void {
    this.userName=sessionStorage.getItem('playername')
    this.getChats()
  }
getChats(){
  this.chatsList$=this.linqService.chatObject$.pipe(tap(i=>console.log(i)))
}

sendMessage(){
  this.myMessage.trim()!=''&&this.linqService.sendMessae({RoomName: sessionStorage.getItem('roomname'), UserName: sessionStorage.getItem('playername'),message:this.myMessage})
  // this.localMessageList.next([...this.localMessageList.value,{userName:user.value,message:this.myMessage}])
  this.myMessage=''
  // setTimeout(()=>this.chatElements.first.nativeElement.lastElementChild.focus(),100);
 // this.chatElements.last.nativeElement.focus()
}

}
