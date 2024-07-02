import { Component, OnInit } from '@angular/core';
import { AuthenticatedResponse } from '../../../models/authenticated-response';
import { TransactionsResponseDTO } from '../../../models/TransactionDTO';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

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
  GetHistory=():void=>
    {this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
        this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
        
   
        if (this.cred.accessToken == '' || this.cred.refreshToken == '') {
          this.his=[];
         return
        
         }
        
        this.http.post<TransactionsResponseDTO[]>("https://localhost:7063/Transaction/History", this.cred, {
            headers: new HttpHeaders({ "Content-Type": "application/json"})
          })
          .subscribe({
            next: (response: TransactionsResponseDTO[]) => {
            console.log(response);
              this.his=response;
             return
            },
            error: (err: HttpErrorResponse) => {this.his=[]}
          });
         
    }
  cred: AuthenticatedResponse={ accessToken:'',refreshToken:''};
  his: TransactionsResponseDTO[]=[];
}
