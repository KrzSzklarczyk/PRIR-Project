import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {FormGroup} from "@angular/forms";
import { AuthenticatedResponse } from '../models/authenticated-response';
import { Router } from '@angular/router';
import { UserResponseDTO } from '../models/user.models';
import { firstValueFrom } from 'rxjs';
import * as addr from '../../addres'




@Injectable({
  providedIn: 'root'
})
export class ModsService {
    constructor(private http :HttpClient,private router:Router){}
  validateHTML(form: FormGroup, field: string, error: string, excludeErrors: string[] = []): boolean {
    const hasError = form.get(field)?.hasError(error);
    const excludedErrors = excludeErrors.some(excludeError => form.get(field)?.hasError(excludeError));
    return !!hasError && !excludedErrors;
  }
  Users:UserResponseDTO[]=[];
  cred:AuthenticatedResponse ={accessToken:'',refreshToken:''}
  async GetAllUsers(): Promise<UserResponseDTO[]> {
    // Initialize the Users array
    this.Users = [];
    
    // Get tokens from localStorage
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
    
    // Check if tokens are present
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
        return this.Users;
    }
    
    try {
        // Make the HTTP POST request and await the response
        const response = await firstValueFrom(this.http.post<UserResponseDTO[]>(
          `${addr.ipaddr}/Account/getAllUsers`,
          this.cred,
          {
            headers: new HttpHeaders({ "Content-Type": "application/json" })
          }
        ));
        
        // Log the response and update the Users array
        console.log(response);
        this.Users = response;
        return this.Users;

    } catch (err) {
        // Handle the error by resetting the Users array
        console.error(err);
        this.Users = [];
        return this.Users;
    }
  }
}


