import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {FormGroup} from "@angular/forms";
import { AuthenticatedResponse } from '../models/authenticated-response';
import { Router } from '@angular/router';
import { UserResponseDTO } from '../models/user.models';




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
 GetAllUsers=():UserResponseDTO[]=>
    {this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
        this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
        
    this.Users=[];
        if (this.cred.accessToken == '' || this.cred.refreshToken == '') {
         
          return this.Users
         }
        
        this.http.post<UserResponseDTO[]>("https://localhost:7063/Account/getAllUsers", this.cred, {
            headers: new HttpHeaders({ "Content-Type": "application/json"})
          })
          .subscribe({
            next: (response: UserResponseDTO[]) => {
              console.log(response);
              return response;
             
            },
            error: (err: HttpErrorResponse) => {this.Users=[]}
          });
          return this.Users
    }
}
