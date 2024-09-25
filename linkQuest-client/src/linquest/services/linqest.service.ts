import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { filter, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LinqestService {

  private hubConnection: HubConnection;
  private connectedUsers : any;
  private gameobjectsSubject : Subject<any> = new Subject()
  gameObject$ = this.gameobjectsSubject.asObservable()
  private gameUsersSubject : Subject<any> = new Subject()
  usersObject$ = this.gameUsersSubject.asObservable()
  private chatSubject : Subject<any> = new Subject()
  chatObject$ = this.chatSubject.asObservable()


  constructor(private router: Router, private _httpClient : HttpClient) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://192.168.133.9:5271/linkquest')
      .build();

    this.hubConnection.on('GroupNotification', (user: string, message: string) => {
      console.log(`${user}: ${message}`);
    });

    this.hubConnection.on('StartGame', (user: string, message: string) => {
      this.router.navigateByUrl("linkquest")
    });

    this.hubConnection.on('ConnectedUser', (user: any) => {
      this.gameUsersSubject.next(user)
    });

    this.hubConnection.on('IndividualNotification', (user: any) => {
      console.log(user);
    });

    this.hubConnection.on('GameObjects', (gameObject: any) => {
      this.gameobjectsSubject.next(gameObject)
    })

    this.hubConnection.on('EndGame', (message: any) => {
      console.log(message)
    })

    this.hubConnection.on('sendTurn', (message: any) => {
      console.log(message)
    })

    this.hubConnection.on('RecieveChat', (message: any) => {
      this.chatSubject.next(message)
    })
    
    this.router.events.pipe(filter((rs): rs is NavigationEnd => rs instanceof NavigationEnd))
    .subscribe(event => {
      if (
        event.id === 1 &&
        event.url === event.urlAfterRedirects
      ) {
          sessionStorage.clear()
      }
    })

    this.hubConnection.start().catch(err => console.error(err));
  }

  joinRoom(userData : {user: string, roomname: string, color: string}) {
    return this.hubConnection.invoke('JoinRoom', {...userData, connectionId : this.hubConnection.connectionId}).then((res) => res)
      .catch(err => console.error(err));
  }

  getGameObject(roomName: string){
    return this.hubConnection.invoke('GameObject', roomName)
      .catch(err => console.error(err));
  }

  getConnectedUsers = () => this.connectedUsers

  updateCell(cellInfo : {rowIndex: number, columnIndex: number, cell: string}){
    return this.hubConnection.invoke('UpdateCell', cellInfo)
    .catch(err => console.error(err));
  }

  createRoom(obj : {name : string, dimension: string, playersCount : string}){
    return this._httpClient.post('http://192.168.133.9:5271/api/JoinRoom', obj)
  }

  SwitchTurns(roomName : string){
    this.hubConnection.invoke('SwitchTurns', roomName)
    .catch(err => console.error(err));
  }

  sendMessae(message : {RoomName : string | null, UserName: string | null, message: string}){
    this.hubConnection.invoke('SendMessage', message)
    .catch(err => console.error(err));
  }
}
