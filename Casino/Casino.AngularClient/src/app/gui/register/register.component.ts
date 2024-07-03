import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import { AuthenticatedResponse } from '../../models/authenticated-response';
import { UserRegisterRequestDTO } from '../../models/register-model';
import { UserService } from '../../services/user.service';
import { firstValueFrom } from 'rxjs';
import * as addr from '../../../addres'
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})

export class RegisterComponent {
  form!: FormGroup;
  invalidLogin: boolean = false;
  credentials: UserRegisterRequestDTO = {login: '', password :'', email:'', nickName:'', avatar:''};
  validateHTML = this.userService.validateHTML.bind(this.userService)

  constructor(private router: Router, private formBuilder: FormBuilder, private http: HttpClient, private userService: UserService) { }

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

      this.form.addControl('email', this.formBuilder.control(
        'user0@my-account.com',
        [
          Validators.required,
          Validators.email
        ]));
      this.form.addControl('confirmedPassword', this.formBuilder.control(
        'User1234!',
        [Validators.required,
          this.userService.passwordMatchValidator(this.form)
        ]));
  }

  async rejestracja(data: UserRegisterRequestDTO): Promise<void> {
    try {
      this.credentials.login = data.login;
      this.credentials.password = data.password;
      this.credentials.email = data.email;
      this.credentials.avatar = '';
      this.credentials.nickName = data.login;

      // Convert Observable to Promise using firstValueFrom
      const response: AuthenticatedResponse = await firstValueFrom(
        this.http.post<AuthenticatedResponse>(`${addr.ipaddr}/Account/register`, this.credentials, {
          headers: new HttpHeaders({ "Content-Type": "application/json" })
        })
      );

      // Handle successful response
      const token = response.accessToken;
      const refreshToken = response.refreshToken;
      localStorage.setItem("accessToken", token);
      localStorage.setItem("refreshToken", refreshToken);
      this.invalidLogin = false;
      this.router.navigate(["/"]);

    } catch (err) {
      if (err instanceof HttpErrorResponse) {
        console.error("Register error:", err);
        this.invalidLogin = true;
        alert("Error creating user. Given username already exists or there is no connection with the database. Please try again.");
      } else {
        console.error("Unexpected error:", err);
        this.invalidLogin = true;
        alert("An unexpected error occurred. Please try again.");
      }
    }
  }
}