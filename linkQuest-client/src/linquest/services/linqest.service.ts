import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class LinqestService {

  private hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5271/linkquest')
      .build();

    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      console.log(`${user}: ${message}`);
    });

    this.hubConnection.on('StartGame', (user: string, message: string) => {
      console.log(`${user}: ${message}`);
    });

    this.hubConnection.on('ConnectedUser', (user: any) => {
      console.log(user);
    });

    this.hubConnection.on('messages', (user: any) => {
      console.log(user);
    });

    this.hubConnection.start().catch(err => console.error(err));
  }

  public joinRoom(userData : {user: string, roomname: string, color: string}): void {
    this.hubConnection.invoke('JoinRoom', userData).then((res) => console.log(res))
      .catch(err => console.error(err));
  }
}
