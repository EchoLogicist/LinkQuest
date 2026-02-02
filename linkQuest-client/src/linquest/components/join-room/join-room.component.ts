import { Component, OnInit } from '@angular/core';
import { LinqestService } from '../../services/linqest.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TimerComponent } from '../timer/timer.component';
import { ChatboxComponent } from "../chatbox/chatbox.component";
import { InputBoxComponent } from "../input-box/input-box.component";
import { ChipsComponent } from "../chips/chips.component";
import { SelectionModel } from '@angular/cdk/collections';
import { AsyncPipe, UpperCasePipe } from '@angular/common';
import { Router } from '@angular/router';
import { Observable, of, tap } from 'rxjs';

@Component({
  selector: 'app-join-room',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, TimerComponent, ChatboxComponent, InputBoxComponent, ChipsComponent, UpperCasePipe, AsyncPipe],

templateUrl: './join-room.component.html',
  styleUrl: './join-room.component.scss'
})
export class JoinRoomComponent implements OnInit {

  joinRoomForm : any
  createRoomForm : any
  hasJoined: boolean = false
  numberOfPlayers:number=6
  matrixList:{id:number,matrix:number}[]=[{id:0,matrix:2},{id:1,matrix:4},{id:2,matrix:6},{id:3,matrix:8},{id:4,matrix:10}]
  matrixSelectionModel : SelectionModel<any> =new SelectionModel<any>(false,[]);
  playerSelectionModel : SelectionModel<any> =new SelectionModel<any>(false,[])
  players:any=[]
  playerName: string=''
  status: boolean = true
  isHost: boolean = false
  colorPalettes : {color: string, picked: boolean}[] = []
  //roomInformation : any 
  roomCode : number = 0
  hostName: string = '';
  colorPalate=[{id:1,color:'#6C08EE'},{id:2,color:'#24034E'},{id:3,color:'#D5BEF3'},{id:4,color:'#5B506A'}]

  constructor(private _linqService: LinqestService, private _fb: FormBuilder, private router: Router){
    this.joinRoomForm = this._fb.group({
      name : ['',[Validators.required]],
      color : ['',[Validators.required]],
      roomname : ['',[Validators.required]],
    })

    this.createRoomForm = this._fb.group({
      dimension : ['', [Validators.required]],
      playersCount : ['', [Validators.required]],
      colorPalette : ['#780000,#023047', [Validators.required]] //,#2a9d8f
    })
  }
  ngOnInit(): void {
    this._linqService.usersObject$.subscribe((res) => {
      this.players = res
      res.forEach((user : any) => {
        if(user.isHost){
          if(user.name == sessionStorage.getItem('playername')) this.isHost = true
          this.hostName = user.name
        }
      });
      console.log(res)
    })  

    this._linqService.colorObject$.subscribe((res) => this.colorPalettes = res)
    //.pipe(tap((res) => console.log(res))) 

    //this.players=[{name:'mohsin',color:'red', isHost: false},{name:'hemanth',color:'blue', isHost: false},{name:'khanam',color:'pink', isHost: true}]

    this.joinRoomForm.valueChanges.subscribe((joinRoom : {roomname : string, name: string, color: string}) => {
      if(joinRoom.name && joinRoom.roomname && joinRoom.roomname.length == 4){
       this.send({roomname : joinRoom.roomname, name : joinRoom.name})
      }
    })
  }
  
  send(joinRoom : any){
    this._linqService.joinRoom(joinRoom).then((res : any) =>{
      if(res && !['No Room Present','rejoined'].includes(res.status)){
        this.hasJoined = true
        //this.roomInformation = res.data
        this.playerName = this.joinRoomForm.value.name
        sessionStorage.setItem("playername", this.joinRoomForm.value.name);
        sessionStorage.setItem("roomname", this.joinRoomForm.value.roomname);
        this.roomCode = this.joinRoomForm.value.roomname
      }
      else if(res.status === 'rejoined'){
        sessionStorage.setItem("playername", this.joinRoomForm.value.name);
        sessionStorage.setItem("color", this.joinRoomForm.value.color);
        this.roomCode = this.joinRoomForm.value.roomname
        document.documentElement.style.setProperty('--player-hover-color', res.data.color);
        this.router.navigateByUrl("linkquest")
      }
      console.log(res)
    })
  }

  CreateRoom(){
    sessionStorage.setItem("playername", this.playerName);
    this._linqService.CreateRoomAndJoin({ Dimension: this.matrixList.find((res) => res.id === this.matrixSelectionModel.selected[0])?.matrix, PlayerName: this.playerName, playersCount: this.playerSelectionModel.selected[0] + 1, colorPalette : 'red,blue,green', }).then((res) => {
      if(res){
        sessionStorage.setItem("roomname", res.roomCode);
        //this.roomInformation = res.data
        this.hostName = this.playerName
        this.roomCode = res.roomCode
        this.RetrunToJoin()
        this.hasJoined = true
      }
    })
  }

  selectMatrix =(item:any)=>{
    this.matrixSelectionModel.select(item.id);
  }
  
  RetrunToJoin(){
    this.matrixSelectionModel.clear()
    this.playerSelectionModel.clear()
  }

  pickColor(color : any){
    if(!color.picked){
    this._linqService.pickColor({ PlayerName : this.playerName, Color : color.color, RoomName : this.roomCode.toString() })
    .then((res) => {
      document.documentElement.style.setProperty('--player-hover-color', color.color);
    })
  }
  }
}
