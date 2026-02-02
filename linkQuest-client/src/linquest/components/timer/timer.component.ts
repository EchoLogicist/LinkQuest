import { AsyncPipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { interval, map, Observable, shareReplay, takeWhile } from 'rxjs';
import { LinqestService } from '../../services/linqest.service';

@Component({
  selector: 'app-timer',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './timer.component.html',
  styleUrl: './timer.component.scss'
})
export class TimerComponent implements OnInit{

  public timeLeft$!: Observable<string>;
  @Input() time : any
  @Input() user : any

  constructor(private _linqService: LinqestService) {}

  ngOnInit(): void {
    if(this.time){
      console.log(this.time)
      this.timeLeft$ = interval(1000).pipe(
        map(x => calcDateDiff(this.time)),
        takeWhile(n => {
          if(n == '00'){
            //if(this.user.name === sessionStorage.getItem('playername')) this._linqService.SwitchTurns(this.user.roomName)
            return false
          }
          else return true
        })
      );
    }
    //   this.timeLeft$ = interval(1000).pipe(
    //     map(x => calcDateDiff(new Date(this.time) ?? new Date())),
    //     takeWhile(n => {
    //       if(n.daysToDday == 0 && n.hoursToDday ==0 && n.minutesToDday == 0 && n.secondsToDday == 0){
    //         if(this.user.name === sessionStorage.getItem('playername')) this._linqService.SwitchTurns(this.user.roomName)
    //         return false
    //       }
    //       else return true
    //     })
    //   );
    // }
  }
}

interface timeComponents {
  secondsToDday: number;
  minutesToDday: number;
  hoursToDday: number;
  daysToDday: number;
}

// function calcDateDiff(endDay: Date ): timeComponents {
//   const dDay = endDay.valueOf();

//   const milliSecondsInASecond = 1000;
//   const hoursInADay = 24;
//   const minutesInAnHour = 60;
//   const secondsInAMinute = 60;

//   const timeDifference = dDay - Date.now();

//   const daysToDday = Math.floor(
//     timeDifference /
//       (milliSecondsInASecond * minutesInAnHour * secondsInAMinute * hoursInADay)
//   );

//   const hoursToDday = Math.floor(
//     (timeDifference /
//       (milliSecondsInASecond * minutesInAnHour * secondsInAMinute)) %
//       hoursInADay
//   );

//   const minutesToDday = Math.floor(
//     (timeDifference / (milliSecondsInASecond * minutesInAnHour)) %
//       secondsInAMinute
//   );

//   const secondsToDday =
//     Math.floor(timeDifference / milliSecondsInASecond) % secondsInAMinute;

//   return { secondsToDday, minutesToDday, hoursToDday, daysToDday };
// }

function calcDateDiff(counter: number ): string {
  var count = counter - 1
  if(count < 10) return `0${count}`
  else return count.toString()
  //return { secondsToDday, minutesToDday, hoursToDday, daysToDday };
}