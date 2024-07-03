import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, of, forkJoin } from 'rxjs';
import { switchMap, map, catchError } from 'rxjs/operators';
import { UserResponseDTO } from '../../../models/user.models';
import { AuthenticatedResponse } from '../../../models/authenticated-response';
import { TransactionsResponseDTO } from '../../../models/TransactionDTO';
import { BanditResponseDTO } from '../../../models/banditDTO';
import { GameResponseDTO } from '../../../models/gameDTO';
import { ResultResponseDTO } from '../../../models/ResultDTO';
import { RouletteResponseDTO } from '../../../models/rouletteDTO'; // Ensure this DTO exists and is correctly imported
import * as addr from '../../../../addres'
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
  selected: number;
}

@Component({
  selector: 'app-admin-panel-user',
  templateUrl: './admin-panel-user.component.html',
  styleUrls: ['./admin-panel-user.component.css']
})
export class AdminPanelUserComponent implements OnInit {
  Users: UserResponseDTO[] = [];
  iconMap = ["banana", "seven", "cherry", "plum", "orange", "bell", "bar", "lemon", "melon"];
  cred: AuthenticatedResponse = { accessToken: '', refreshToken: '' };
  displayedColumns: string[] = ['userId', 'email', 'nickName', 'avatar', 'credits', 'userType', 'actions'];
  hisBandit: BanditHistoryEntry[] = [];
  hisRoulette: RouletteHistoryEntry[] = [];
  showMatchHistory: boolean = false;
  showTransactionHistory: boolean = false;
  selectedUserId: number | null = null;
  transactionHistory: TransactionsResponseDTO[] = [];
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.GetAllUsers();
  }
  funcshowTransactionHistory(userId: number): void {
    this.GetHistory(userId);
    this.showTransactionHistory = true;
    console.log(this.transactionHistory)
    this.showMatchHistory=false;
  }
  private getAuthHeaders() {
    return new HttpHeaders({
      "Content-Type": "application/json",
      "Authorization": `Bearer ${this.cred.accessToken}`
    });
  }

  private setCredentials() {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
  }

  GetHistory(id: number): void {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      this.transactionHistory = [];
      return;
    }
    const url = `https://${addr.ipaddr}/Transaction/History/${id}`;
    this.http.post<TransactionsResponseDTO[]>(url, this.cred, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).subscribe({
      next: (response: TransactionsResponseDTO[]) => {
        this.transactionHistory = response;
        this.showTransactionHistory = true;
        this.showMatchHistory = false;
        this.selectedUserId = id;
      },
      error: (err: HttpErrorResponse) => { this.transactionHistory = []; }
    });
  }
  RemoveUser(id: number): void {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      return;
    }
    const url = `https://${addr.ipaddr}/Account/RemoveUser/${id}`;
    this.http.post<boolean>(url, this.cred, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).subscribe({
      next: (response: boolean) => {
        if (response) {
          this.GetAllUsers();
        }
      },
      error: (err: HttpErrorResponse) => { }
    });
  }
  GetAllUsers(): void {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    this.Users = [];
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      return;
    }
    this.http.post<UserResponseDTO[]>(`https://${addr.ipaddr}/Account/getAllUsers`, this.cred, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).subscribe({
      next: (response: UserResponseDTO[]) => {
        this.Users = response;
      },
      error: (err: HttpErrorResponse) => { this.Users = []; }
    });
  }

  getBanditData(id: number): Observable<BanditResponseDTO> {
    this.setCredentials();
    if (!this.cred.accessToken || !this.cred.refreshToken) {
      return of({
        description: '',
        position1: 0,
        position2: 0,
        position3: 0
      });
    }
    const url = `https://${addr.ipaddr}/Game/Bandit/${id}`;
    return this.http.get<BanditResponseDTO>(url, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(() => of({
        description: '',
        position1: 0,
        position2: 0,
        position3: 0
      }))
    );
  }

  getGameData(id: number): Observable<GameResponseDTO> {
    this.setCredentials();
    if (!this.cred.accessToken || !this.cred.refreshToken) {
      return of({
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
      });
    }
    const url = `https://${addr.ipaddr}/Game/Game/${id}`;
    return this.http.get<GameResponseDTO>(url, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(() => of({
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
      }))
    );
  }

  getRouletteData(id: number): Observable<RouletteResponseDTO> {
    this.setCredentials();
    if (!this.cred.accessToken || !this.cred.refreshToken) {
      return of({
        red: false,
        black: false,
        number: 0,
        betnumber: 0
      });
    }
    const url = `https://${addr.ipaddr}/Game/Roulette/${id}`;
    return this.http.get<RouletteResponseDTO>(url, {
      headers: this.getAuthHeaders()
    }).pipe(
      catchError(() => of({
        red: false,
        black: false,
        number: 0,
        betnumber: 0
      }))
    );
  }

  getMatchHistory(userId: number): void {
    this.setCredentials();
    this.showMatchHistory=true;
    this.hisBandit=[];
    this.hisRoulette=[];
    this.showTransactionHistory=false;
    if (!this.cred.accessToken || !this.cred.refreshToken) {
      return;
    }
    const url = `https://${addr.ipaddr}/Result/GetUserResult/${userId}`;
    this.http.post<ResultResponseDTO[]>(url, this.cred, {
      headers: this.getAuthHeaders()
    }).pipe(
      switchMap(results => {
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
                  map(rouletteData => ({
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
                      selected: rouletteData.betnumber
                    } as RouletteHistoryEntry
                  }))
                );
              } else {
                return of(null); // No Bandit or Roulette data available
              }
            })
          );
        });

        return forkJoin(dataObservables);
      })
    ).subscribe(hisData => {
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
      this.showMatchHistory = true;
      this.showTransactionHistory = false;
    });
  }

  goBack(): void {
    this.showMatchHistory = false;
    this.showTransactionHistory = false;
    this.selectedUserId = null;
  }
}
