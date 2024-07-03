import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { AuthenticatedResponse } from '../../models/authenticated-response';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import * as addr from '../../../addres'
@Component({
  selector: 'app-roulette',
  templateUrl: './roulette.component.html',
  styleUrls: ['./roulette.component.css']
})
export class RouletteComponent implements OnInit {
  
  perfecthalf: number = ((1 / 37) * 360) / 2;
  currentLength: number = this.perfecthalf;
  spininterval: number = 0;
  selectedButton: string | null = null;
  selectedNumber: number | null = null;
  betAmount: number = 25;
  rolledNumber:number=-2137;
  cred: AuthenticatedResponse={accessToken:'',refreshToken:''};
  redNumbers: number[] = [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36];
  blackNumbers: number[] = [2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35];
  blueNumbers: number[] = [0];
  isAuthorized: boolean = false;
  constructor(private renderer: Renderer2, private el: ElementRef,private http:HttpClient) {}

  ngOnInit(): void {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

    if (!(this.cred.accessToken == '' || this.cred.refreshToken == '')) {
      this.isAuthorized = true
    }
    this.renderer.setStyle(this.el.nativeElement.querySelector('.wheel img'), 'transform', `rotate(${this.perfecthalf}deg)`);
  }

  spin(): void {
    if(this.betAmount >= 25 && (this.selectedButton == "red" || this.selectedButton == "blue" || this.selectedButton == "black")){
    this.renderer.setStyle(this.el.nativeElement.querySelector('.wheel img'), 'filter', 'blur(8px)');
this.rolledNumber=this.getRandomInt(0, 37);
    this.spininterval =this.rolledNumber  * (360 / 37) + this.getRandomInt(3, 4) * 360;
    this.currentLength += this.spininterval;

    const numofsecs = this.spininterval;

    console.log(this.currentLength);
    console.log(this.selectedButton);
    console.log(this.selectedNumber);
    this.renderer.setStyle(this.el.nativeElement.querySelector('.wheel img'), 'transform', `rotate(${this.currentLength}deg)`);

    setTimeout(() => {
      this.renderer.setStyle(this.el.nativeElement.querySelector('.wheel img'), 'filter', 'blur(0px)');
    }, numofsecs);
    this.sendGame()
  }
  }
  async sendGame(): Promise<void> {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

    if (!this.cred.accessToken || !this.cred.refreshToken) {
      console.error('No access or refresh token found');
      return;
    }

    const tmp = this.selectedNumber === null
      ? this.selectedButton === 'blue'
        ? 0
        : -1
      : this.selectedNumber;

    const r = this.selectedButton === 'red';
    const b = this.selectedButton === 'black';

    const url = `${addr.ipaddr}/Game/PlayRoullete/${tmp}/${r}/${b}/${this.betAmount}/${this.rolledNumber}`;

    try {
      console.log(url);
      const response: boolean = await firstValueFrom(
        this.http.post<boolean>(
          url,
          this.cred,
          {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
          }
        )
      );
      console.log(response);
      // Handle successful response
    } catch (err) {
      console.error('Error during game play:', err);
      // Optionally, handle the error
      alert('An error occurred while sending the game request. Please try again.');
    }
  }

  toggleButton(button: string): void {
    this.selectedButton = this.selectedButton === button ? null : button;
    this.selectedNumber = null;
  }

  getRandomInt(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  getNumbersForSelectedButton(): number[] {
    if (this.selectedButton === 'red') {
      return this.redNumbers;
    } else if (this.selectedButton === 'black') {
      return this.blackNumbers;
    } else if (this.selectedButton === 'blue') {
      return this.blueNumbers;
    } else {
      return [];
    }
  }
}
