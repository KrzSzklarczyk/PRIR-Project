import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Observable, of, forkJoin } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { AuthenticatedResponse } from '../../../models/authenticated-response';
import { BanditResponseDTO } from '../../../models/banditDTO';
import { GameResponseDTO } from '../../../models/gameDTO';
import { ResultResponseDTO } from '../../../models/ResultDTO';
import { RouletteResponseDTO } from '../../../models/rouletteDTO'; // Ensure this DTO exists and is correctly imported

interface BanditHistoryEntry {
  date: string | Date;
  amount: number;
  position1: number;
  position2: number;
  position3: number;
  minbet: number;
  maxbet: number;
  betAmount: number;
}

interface RouletteHistoryEntry {
  date: string | Date;
  amount: number;
  red: boolean;
  black: boolean;
  rolled: number;
  minbet: number;
  maxbet: number;
  betAmount: number;
  selected:number
}

@Component({
  selector: 'app-match-history',
  templateUrl: './match-history.component.html',
  styleUrls: ['./match-history.component.css']
})

export class MatchHistoryComponent implements OnInit {
  iconMap = ["banana", "seven", "cherry", "plum", "orange", "bell", "bar", "lemon", "melon"];
  displayedColumnsBandit: string[] = ['date', 'amount', 'position1', 'position2', 'position3', 'minbet', 'maxbet', 'betAmount'];
  displayedColumnsRoulette: string[] = ['date', 'amount', 'red', 'black','betnumber', 'number', 'minbet', 'maxbet', 'betAmount'];
  cred: AuthenticatedResponse = { accessToken: '', refreshToken: '' };
  hisBandit: BanditHistoryEntry[] = [];
  hisRoulette: RouletteHistoryEntry[] = [];

  constructor(private http: HttpClient, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.getResultData();
  }

  banditData: BanditResponseDTO = {
    description: '',
    position1: 0,
    position2: 0,
    position3: 0
  };

  gameData: GameResponseDTO = {
    resultId: 0,
    blackJackId: null,
    rouletteId: null,
    banditId: 0,
    description: '',
    startDate: new Date(),
    endDate: new Date(),
    maxBet: 0,
    minBet: 0,
    amount: 0
  };

  rouletteData: RouletteResponseDTO = {
    red: false,
    black: false,
    number: 0,
    betnumber:0
  };

  getBanditData(id: number): Observable<BanditResponseDTO> {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      return of(this.banditData); // Return an observable of the default banditData
    } else {
      const url = `https://localhost:7063/Game/Bandit/${id}`;
      return this.http.get<BanditResponseDTO>(url, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      });
    }
  }

  getGameData(id: number): Observable<GameResponseDTO> {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      return of(this.gameData); // Return an observable of the default gameData
    } else {
      const url = `https://localhost:7063/Game/Game/${id}`;
      return this.http.get<GameResponseDTO>(url, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      });
    }
  }

  getRouletteData(id: number): Observable<RouletteResponseDTO> {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      return of(this.rouletteData); // Return an observable of the default rouletteData
    } else {
      const url = `https://localhost:7063/Game/Roulette/${id}`;
      return this.http.get<RouletteResponseDTO>(url, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      }).pipe(
        // Ensure that the response is not null or undefined
        map(data => data ?? this.rouletteData),
        catchError(() => of(this.rouletteData)) // Catch any HTTP errors and provide default data
      );
    }
  }
  

  getResultData(): void {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      return;
    }
  
    this.http.post<ResultResponseDTO[]>('https://localhost:7063/Result/GetUserResult', this.cred, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    }).subscribe(results => {
      // Create an array of observables to handle fetching game and specific game type data
      const dataObservables = results.map(result => {
        return this.getGameData(result.gameId).pipe(
          switchMap(gameData => {
            if (gameData.banditId) {
              return this.getBanditData(gameData.banditId).pipe(
                map(banditData => ({
                  type: 'bandit',
                  data: {
                    amount: result.amount,
                    betAmount: gameData.amount,
                    date: gameData.endDate,
                    maxbet: gameData.maxBet,
                    minbet: gameData.minBet,
                    position1: banditData.position1,
                    position2: banditData.position2,
                    position3: banditData.position3,
                  } as BanditHistoryEntry
                }))
              );
            } else if (gameData.rouletteId) {
              return this.getRouletteData(gameData.rouletteId).pipe(
                map(rouletteData => {
                  if (!rouletteData) {
                    throw new Error('Received null or undefined roulette data');
                  }
                  return {
                    type: 'roulette',
                    data: {
                      amount: result.amount,
                      betAmount: gameData.amount,
                      date: gameData.endDate,
                      maxbet: gameData.maxBet,
                      minbet: gameData.minBet,
                      red: rouletteData.red,
                      black: rouletteData.black,
                      rolled: rouletteData.number,
                      selected:rouletteData.betnumber
                      
                    } as RouletteHistoryEntry
                  };
                })
              );
            } else {
              return of(null); // No Bandit or Roulette data available
            }
          })
        );
      });
  
      // Combine all observables and update the `hisBandit` and `hisRoulette` arrays once all data is fetched
      forkJoin(dataObservables).subscribe(hisData => {
        this.hisBandit = [];
        this.hisRoulette = [];
        hisData.forEach(data => {
          if (data) {
            if (data.type === 'bandit') {
              this.hisBandit.push(data.data as BanditHistoryEntry);
            } else if (data.type === 'roulette') {
              this.hisRoulette.push(data.data as RouletteHistoryEntry);
            }
          }
        });
        console.log('Bandit Data:', this.hisBandit);
        console.log('Roulette Data:', this.hisRoulette);
        this.cd.detectChanges(); // Manually trigger change detection
      });
    });
  }
  
}
