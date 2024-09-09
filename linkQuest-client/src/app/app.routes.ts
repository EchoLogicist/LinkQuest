import { Routes } from '@angular/router';
import { JoinRoomComponent } from '../linquest/components/join-room/join-room.component';

export const routes: Routes = [
    {path: '', redirectTo: 'joinRoom', pathMatch: "full"},
    {path: 'joinRoom', component: JoinRoomComponent}
];
