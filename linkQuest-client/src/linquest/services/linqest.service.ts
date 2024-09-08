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

    this.hubConnection.start().catch(err => console.error(err));
  }

  public sendMessage(user: string, message: string): void {
    this.hubConnection.invoke('SendMessage', user, message)
      .catch(err => console.error(err));
  }
}
