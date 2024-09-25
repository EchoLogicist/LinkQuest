import { Component } from '@angular/core';
import { LinqestService } from '../../services/linqest.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TimerComponent } from '../timer/timer.component';

@Component({
  selector: 'app-join-room',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, TimerComponent],
  templateUrl: './join-room.component.html',
  styleUrl: './join-room.component.scss'
})
export class JoinRoomComponent {

  joinRoomForm : any
  createRoomForm : any
  hasJoined: boolean = false

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
  
  send(){
    this._linqService.joinRoom(this.joinRoomForm.value).then((res : string) =>{
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
}
