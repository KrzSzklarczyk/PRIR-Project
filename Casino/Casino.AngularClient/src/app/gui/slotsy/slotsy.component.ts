import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SlotsyService } from './slotsy.service';
import { NgModel } from '@angular/forms';
import { AuthenticatedResponse } from '../../models/authenticated-response';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserResponseDTO } from '../../models/user.models';
import { firstValueFrom } from 'rxjs';
import * as addr from '../../../addres'
@Component({
  selector: 'app-root',
  templateUrl: './slotsy.component.html',
  styleUrl: './slotsy.component.css'
})
export class SlotsyComponent implements OnInit {
  debugText: string = 'Click "Start Rolling" to begin';
  rolling: boolean = false;
  betAmount: number = 25;
  cred: AuthenticatedResponse={accessToken:'',refreshToken:''};
  isAuthorized: boolean = false;

  constructor(private slotMachineService: SlotsyService,private http:HttpClient) {}

  ngOnInit(): void {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

    if (!(this.cred.accessToken == '' || this.cred.refreshToken == '')) {
      this.isAuthorized = true
    }
  }

  startRolling(): void {
    if(this.betAmount >= 25){
      if (!this.rolling) {
        this.rolling = true;
        this.debugText = 'Rolling...';

      const reelsList = document.querySelectorAll<HTMLElement>('.slots > .reel');
     Promise
      .all(Array.from(reelsList).map((reel, i) => this.slotMachineService.roll(reel, i)))
      .then((deltas) => {
        deltas.forEach((delta, i) => this.slotMachineService.indexes[i] = (this.slotMachineService.indexes[i] + delta) % this.slotMachineService.num_icons);
      
        this.SendGame(this.slotMachineService.indexes[0],this.slotMachineService.indexes[1],this.slotMachineService.indexes[2],this.betAmount);
       
        
        if (this.slotMachineService.indexes[0] == this.slotMachineService.indexes[1] || this.slotMachineService.indexes[1] == this.slotMachineService.indexes[2]) {
          const winCls = this.slotMachineService.indexes[0] == this.slotMachineService.indexes[2] ? "win2" : "win1";
          const slotsEl = document.querySelector(".slots") as HTMLElement;
          slotsEl.classList.add(winCls);
          setTimeout(() => slotsEl.classList.remove(winCls), 2000);
        }
        else{
          const winCls = "lose";
          const slotsEl = document.querySelector(".slots") as HTMLElement;
          slotsEl.classList.add(winCls);
          setTimeout(() => slotsEl.classList.remove(winCls), 2000);
        }
        
        this.rolling = false;
      });
    }
  }
}
SendGame = async (pos1: number, pos2: number, pos3: number, amount: number): Promise<void> => {
  // Retrieve the accessToken and refreshToken from localStorage
  this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
  this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

  // Check if either token is missing
  if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
    return;
  }

  // Construct the URL for the POST request
  const url = `${addr.ipaddr}/Game/PlayBandit/${pos1}/${pos2}/${pos3}/${amount}`;
console.log(url)
  // Perform the POST request and await the result
  try {
    const response = await firstValueFrom(
      this.http.post<boolean>(
        url,
        this.cred,
        {
          headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        }
      )
    );
    // Handle the response if needed
    console.log(response); // Optional: Log the response or handle it as needed
  } catch (error) {
    // Handle errors if any
    console.error('An error occurred:', error);
  }
}
  setMinBet(): void {
    this.betAmount = 25;
  }

  increase25Bet(): void {
    this.betAmount += 25;
  }

  increase50Bet(): void {
    this.betAmount += 50;
  }

  increase100Bet(): void {
    this.betAmount += 100;
  }

  decrease25Bet(): void {
    this.betAmount -= 25;
  }

  decrease50Bet(): void {
    this.betAmount -= 50;
  }

  decrease100Bet(): void {
    this.betAmount -= 100;
  }

  async setMaxBet(): Promise<void> {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
  
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      // Handle the case when tokens are missing
      return;
    } else {
      try {
        const response: UserResponseDTO = await firstValueFrom(
          this.http.post<UserResponseDTO>(
`${addr.ipaddr}/Account/getUserInfo`, // Make sure to include 'http://' or 'https://'
            this.cred,
            {
              headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
            }
          )
        );
        
        this.betAmount = response.credits;
      } catch (error) {
        console.error('Error fetching user info:', error);
      }
    }
  }
}