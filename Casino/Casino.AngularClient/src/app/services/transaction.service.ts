import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {FormGroup} from "@angular/forms";
import { AuthenticatedResponse } from '../models/authenticated-response';
import { Router } from '@angular/router';




@Injectable({
  providedIn: 'root'
})
export class TransactionService {
    constructor(private http :HttpClient,private router:Router){}
  validateHTML(form: FormGroup, field: string, error: string, excludeErrors: string[] = []): boolean {
    const hasError = form.get(field)?.hasError(error);
    const excludedErrors = excludeErrors.some(excludeError => form.get(field)?.hasError(excludeError));
    return !!hasError && !excludedErrors;
  }
  cred:AuthenticatedResponse ={accessToken:'',refreshToken:''}
  addCredits=(crdits: number):boolean=>
    {this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
        this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    
        if (this.cred.accessToken == '' || this.cred.refreshToken == '') {
         return false}
         const url = `https://localhost:7063/Transaction/New/${crdits}`;
        this.http.post<boolean>(url, this.cred, {
            headers: new HttpHeaders({ "Content-Type": "application/json"})
          })
          .subscribe({
            next: (response: boolean) => {
            
              this.router.navigate(["/"]);
              return response
            },
            error: (err: HttpErrorResponse) => {return false}
          });
          return false;
    }
}
