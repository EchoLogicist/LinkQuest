import { Routes } from '@angular/router';
import { JoinRoomComponent } from '../linquest/components/join-room/join-room.component';
import { LinkQuestComponent } from '../linquest/components/link-quest/link-quest.component';
import { ResultsComponent } from '../linquest/components/results/results.component';

export const routes: Routes = [
    {path: '', redirectTo: 'joinRoom', pathMatch: "full"},
    {path: 'joinRoom', component: JoinRoomComponent},
    {path: 'linkquest', component: LinkQuestComponent},
    {path: 'results', component: ResultsComponent}
];
