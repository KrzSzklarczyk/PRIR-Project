import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {FormGroup} from "@angular/forms";
import { AuthenticatedResponse } from '../models/authenticated-response';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';




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
  async addCredits(credits: number): Promise<boolean> {
    // Retrieve tokens from local storage
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
  
    // Check if tokens are available
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      console.error('Access token or refresh token is missing');
      return false;
    }
  
    const url = `https://localhost:7063/Transaction/New/${credits}`;
  
    try {
      // Perform the HTTP POST request to add credits
      const response = await firstValueFrom(
        this.http.post<boolean>(url, this.cred, {
          headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        })
      );
  
      // Navigate to the home page after adding credits
      this.router.navigate(['/']);
  
      // Return the response from the request
      return response;
    } catch (error) {
      // Handle potential errors
      if (error instanceof HttpErrorResponse) {
        console.error('Failed to add credits:', error.message);
      } else {
        console.error('Unexpected error:', error);
      }
      return false;
    }
  }
}
