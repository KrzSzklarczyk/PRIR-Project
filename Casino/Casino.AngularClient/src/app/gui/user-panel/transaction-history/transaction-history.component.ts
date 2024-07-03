import { Component, OnInit } from '@angular/core';
import { AuthenticatedResponse } from '../../../models/authenticated-response';
import { TransactionsResponseDTO } from '../../../models/TransactionDTO';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import * as addr from '../../../../addres'
@Component({
  selector: 'app-transaction-history',
  templateUrl: './transaction-history.component.html',
  styleUrl: './transaction-history.component.css'
})
export class TransactionHistoryComponent implements OnInit  {
  ngOnInit(): void {
   
    this.GetHistory();
    
   }
   displayedColumns: string[] = [ 'date', 'amount'];
constructor(private http:HttpClient){}
async GetHistory(): Promise<void> {
  // Retrieve tokens from local storage
  this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
  this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

  // Check if tokens are available
  if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
    this.his = [];
    return;
  }

  const url = `https://${addr.ipaddr}:7063/Transaction/History`;

  try {
    // Perform the HTTP POST request to get transaction history
    const response = await firstValueFrom(
      this.http.post<TransactionsResponseDTO[]>(url, this.cred, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      })
    );

    // Handle the response
    console.log(response);
    this.his = response;
  } catch (error) {
    // Handle potential errors
    if (error instanceof HttpErrorResponse) {
      console.error('Failed to get transaction history:', error.message);
    } else {
      console.error('Unexpected error:', error);
    }
    this.his = [];
  }
}
  cred: AuthenticatedResponse={ accessToken:'',refreshToken:''};
  his: TransactionsResponseDTO[]=[];
}
