import { Component, OnInit } from '@angular/core';
import { LinqestService } from '../../services/linqest.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TimerComponent } from '../timer/timer.component';
import { ChatboxComponent } from "../chatbox/chatbox.component";
import { InputBoxComponent } from "../input-box/input-box.component";
import { ChipsComponent } from "../chips/chips.component";
import { SelectionModel } from '@angular/cdk/collections';
import { UpperCasePipe } from '@angular/common';

@Component({
  selector: 'app-join-room',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, TimerComponent, ChatboxComponent, InputBoxComponent, ChipsComponent,UpperCasePipe],

templateUrl: './join-room.component.html',
  styleUrl: './join-room.component.scss'
})
export class JoinRoomComponent implements OnInit {

  joinRoomForm : any
  createRoomForm : any
  hasJoined: boolean = false
  numberOfPlayers:number=6
  matrixList:{id:number,matrix:number}[]=[{id:0,matrix:2},{id:1,matrix:4},{id:2,matrix:6}]
  matrixSelectionModel:any=new SelectionModel(false,[]);
  playerSelectionModel:any=new SelectionModel(false,[])
  players:any=[]
  playerName:string='khanam'
  status:boolean=true;
  colorPalate=[{id:1,color:'#6C08EE'},{id:2,color:'#24034E'},{id:3,color:'#D5BEF3'},{id:4,color:'#5B506A'}]

  constructor(private _linqService: LinqestService, private _fb: FormBuilder){
    this.joinRoomForm = this._fb.group({
      name : ['',[Validators.required]],
      color : ['',[Validators.required]],
      roomname : ['',[Validators.required]],
    })

    this.createRoomForm = this._fb.group({
      dimension : ['',[Validators.required]],
      playersCount : ['',[Validators.required]],
    })
  }
  ngOnInit(): void {
    this._linqService.usersObject$.subscribe((res) => {
      this.players = res
      console.log(res)
    })  
    this.players=[{name:'mohsin',color:'red'},{name:'hemanth',color:'blue'},{name:'khanam',color:'pink'}]

    this.joinRoomForm.valueChanges.subscribe((joinRoom : {roomname : string, name: string, color: string}) => {
      if(joinRoom.name && joinRoom.roomname && joinRoom.roomname.length == 4){
       this.send({roomname : joinRoom.roomname, name : joinRoom.name})
      }
    })
  }
  
  send(joinRoom : any){
    this._linqService.joinRoom(joinRoom).then((res : string) =>{
      if(res.toLowerCase() == 'success' || res.toLowerCase() == 'rejoined'){
        this.hasJoined = true
        sessionStorage.setItem("playername", this.joinRoomForm.value.name);
        sessionStorage.setItem("color", this.joinRoomForm.value.color);
        sessionStorage.setItem("roomname", this.joinRoomForm.value.roomname);
        document.documentElement.style.setProperty('--player-hover-color', this.joinRoomForm.value.color);
      } 
      else console.log(res)
    })
  }

  CreateRoom(){
    this._linqService.createRoom(this.createRoomForm.value).subscribe({
      next: (res) => {
        console.log(res)
      },
      error: (err) => console.error(err)
    })
  }
  selectMatrix =(item:any)=>{
    this.matrixSelectionModel.select(item.id);
  }
  
}
