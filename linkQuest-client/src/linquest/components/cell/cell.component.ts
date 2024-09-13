import { Component, Input, OnInit } from '@angular/core';
import { LinqestService } from '../../services/linqest.service';

@Component({
  selector: 'app-cell',
  standalone: true,
  imports: [],
  templateUrl: './cell.component.html',
  styleUrl: './cell.component.scss'
})
export class CellComponent implements OnInit{
  
  @Input() cellObject : any
  @Input() users : any = []
  @Input() rowIndex : number = 0
  @Input() columnIndex : number = 0
  
  constructor(private _linqService: LinqestService) {}

  ngOnInit(): void {
    console.log(this.cellObject)
    console.log(this.users)
  }

  markCell(cell: string){
    if(!this.cellObject[cell].Checked && this.users.find((j : any) => j.name === sessionStorage.getItem('playername')).myTurn){
      this.cellObject[cell].Checked = true
      this.cellObject[cell].UserName = sessionStorage.getItem("playername");
      this._linqService.updateCell({rowIndex : this.rowIndex, columnIndex : this.columnIndex, cell})
    }    
  }

  getColor = (cell: string) => this.cellObject[cell].Checked ? this.users.find((j : any) => j.name === this.cellObject[cell].UserName)?.color : ''
}
