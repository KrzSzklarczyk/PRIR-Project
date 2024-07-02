import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import { AuthenticatedResponse } from '../../models/authenticated-response';
import { UserRegisterRequestDTO } from '../../models/register-model';
import { UserService } from '../../services/user.service';

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

  rejestracja = ( data: UserRegisterRequestDTO ) => {
      this.credentials.login = data.login;
      this.credentials.password = data.password;
      this.credentials.email = data.email;
      this.credentials.avatar = "";
      this.credentials.nickName = data.login;
      this.http.post<AuthenticatedResponse>("https://localhost:7063/Account/register", this.credentials, {
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
          console.error("register error:", err);
          this.invalidLogin = true;
          alert("Error creating user. Given username already exists or there is no connection with database. Please try again.");
        }
      })
  }
}