import { Component, OnInit } from '@angular/core';
import { LinqestService } from '../../services/linqest.service';

@Component({
  selector: 'app-results',
  standalone: true,
  imports: [],
  templateUrl: './results.component.html',
  styleUrl: './results.component.scss'
})
export class ResultsComponent implements OnInit {

  constructor(private linqService: LinqestService) {}
  users : any

  ngOnInit(): void {
    this.users = this.linqService.result.users.sort((a : any, b : any) => b.count - a.count)
  }

}
