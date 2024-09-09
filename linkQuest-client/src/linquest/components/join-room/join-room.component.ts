import { Component } from '@angular/core';
import { LinqestService } from '../../services/linqest.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-join-room',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './join-room.component.html',
  styleUrl: './join-room.component.scss'
})
export class JoinRoomComponent {

  joinRoomForm : any

  constructor(private _linqService: LinqestService, private _fb: FormBuilder){
    this.joinRoomForm = this._fb.group({
      name : ['',[Validators.required]],
      color : ['',[Validators.required]],
      roomname : ['',[Validators.required]],
    })
  }
  
  send(){
    this._linqService.joinRoom(this.joinRoomForm.value)
  }
}
