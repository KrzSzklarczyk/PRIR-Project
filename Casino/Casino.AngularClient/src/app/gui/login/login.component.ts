import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { NgForm } from '@angular/forms';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import{AuthenticatedResponse} from "../../models/authenticated-response";
import { LoginModel } from '../../models/login-model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  form!: FormGroup;
  invalidLogin: boolean = false;
  isLoginMode: boolean = true;
  credentials: LoginModel = {login:'', password:''};
  validateHTML = this.userService.validateHTML.bind(this.userService)

  constructor(private router: Router, private formBuilder: FormBuilder, private userService: UserService, private http: HttpClient) { }

  ngOnInit(){
    this.form = this.formBuilder.group({
      login: ['user0'],
      password: ['User1234!']
    })
    this.form.get('login')?.setValidators([
      Validators.required,
      Validators.minLength(4)
    ])
    this.form.get('password')?.setValidators([
      Validators.required,
      Validators.minLength(4),
      this.userService.passwordValidator()
    ])
  }

  login = ( data: LoginModel) => {
    this.credentials.login = data.login;
    this.credentials.password = data.password;
    this.http.post<AuthenticatedResponse>("https://localhost:7063/Account/Login", this.credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json"})
    })
    .subscribe({
      next: (response: AuthenticatedResponse) => {
        const token = response.accessToken;
        const refreshToken = response.refreshToken;
        localStorage.setItem("accessToken", token);
        localStorage.setItem("refreshToken", refreshToken);
        this.invalidLogin = false;
        this.router.navigate(["/"]);
      },
      error: (err: HttpErrorResponse) => {
        console.error("Login error:", err);
        this.invalidLogin = true;
        alert("Invalid username or password. Please try again.");
      }
    })  
  }
}
