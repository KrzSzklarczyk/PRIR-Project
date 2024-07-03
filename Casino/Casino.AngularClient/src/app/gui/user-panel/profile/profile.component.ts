import { Component, Input } from '@angular/core';
import { AuthenticatedResponse } from '../../../models/authenticated-response';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { UserType } from '../../../models/UserRole';
import { UserResponseDTO } from '../../../models/user.models';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { UserChangeDTO } from '../../../models/userChangeDto';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import * as addr from '../../../../addres'
@Component({
  selector: 'app-user-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent {
  @Input() userName: string = '';
  @Input() credits: number = 0;
  @Input() userRole: UserType = UserType.User;
  @Input() avatar: string = '';
  showChangePasswordForm = false;
  showChangeAvatarForm = false;
  changePasswordForm: FormGroup;
  showDeleteForm = false;

  //form!: FormGroup;
  //validateHTML = this.userService.validateHTML.bind(this.userService)

  constructor(
    private fb: FormBuilder,
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
  ) {
    this.changePasswordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      newAvatar: [''],
    });
  }

  ngOnInit() {
    this.getUserInfo();
    // this.form.get('password')?.setValidators([
    //   Validators.required,
    //   Validators.minLength(6),
    //   this.userService.passwordValidator()
    // ])

    // this.form.addControl('confirmedPassword', this.formBuilder.control(
    //   'User1234!',
    //   [Validators.required,
    //     this.userService.passwordMatchValidator(this.form)
    //   ]));
  }

  cred: AuthenticatedResponse = {
    accessToken: localStorage.getItem('accessToken') ?? '',
    refreshToken: localStorage.getItem('refreshToken') ?? '',
  };

  async changeAvatar(): Promise<void> {
    // Get the new avatar value from the form
    const awa = this.changePasswordForm.get('newAvatar')?.value;
  
    // Retrieve the accessToken and refreshToken from localStorage
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
  
    // Create the DTO object for the request
    const us: UserChangeDTO = { token: this.cred, cos: awa };
  
    // Check if either token is missing
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      return;
    }
  
    // Perform the PUT request and await the result
    try {
      await firstValueFrom(
        this.http.put<boolean>(`${addr.ipaddr}/Account/ChangeAvatar`, us, {
          headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        })
      );
      console.log('Avatar changed successfully');
      // Call getUserInfo after the avatar is changed successfully
      await this.getUserInfo();
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        console.error('An error occurred:', error.message);
      } else {
        console.error('An unexpected error occurred:', error);
      }
    }
  }
  toggleChangePasswordForm() {
    this.getUserInfo();
    this.showChangePasswordForm = !this.showChangePasswordForm;
    this.showChangeAvatarForm = false;
    this.showDeleteForm = false;
  }

  toggleChangeAvatarForm() {
    this.getUserInfo();
    this.showChangeAvatarForm = !this.showChangeAvatarForm;
    this.showChangePasswordForm = false;
    this.showDeleteForm = false;
  }
  toggleDeleteForm() {
    this.getUserInfo();
    this.showDeleteForm = !this.showDeleteForm;
    this.showChangePasswordForm = false;
    this.showChangeAvatarForm = false;
  }

  async changePassword(): Promise<void> {
    // Check if the form is valid
    if (!this.changePasswordForm.valid) {
      alert("Invalid Password!!! Password should have: small letters, UpperCase, digits, and special characters. Please try again.");
      return;
    }
  
    // Get the new password value from the form
    const newPassword = this.changePasswordForm.get('newPassword')?.value;
  
    // Retrieve the accessToken and refreshToken from localStorage
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
  
    // Create the DTO object for the request
    const us: UserChangeDTO = { token: this.cred, cos: newPassword };
  
    // Check if either token is missing
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      return;
    }
  
    try {
      // Perform the PUT request and await the result
      await firstValueFrom(
        this.http.put<boolean>(`${addr.ipaddr}/Account/ChangePasswd`, us, {
          headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        })
      );
      console.log('Password changed successfully');
      // Call getUserInfo after the password is changed successfully
      await this.getUserInfo();
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        console.error('An error occurred:', error.message);
      } else {
        console.error('An unexpected error occurred:', error);
      }
    }
  }

  async deleteAccount() {
    // Retrieve tokens from local storage
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
  
    // Check if tokens are available
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      // Handle the case where tokens are not available
      console.error('Access token or refresh token is missing');
      return;
    }
  
    try {
      // Perform the HTTP PUT request to delete the account
      await firstValueFrom(
        this.http.put<boolean>(`${addr.ipaddr}/Account/RemoveAcc`, this.cred, {
          headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        })
      );
      // Log out the user
      this.logOut();
      // Navigate to the home page
      this.router.navigate(['/']);
    } catch (error) {
      // Handle potential errors
      console.error('Failed to delete the account:', error);
    }
  }

  logOut() {
    // Logic to log out

    this.getUserInfo();
    this.authService.logOut();
  }

  async getUserInfo() {
    // Retrieve tokens from local storage
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';
  
    // Check if tokens are available
    if (this.cred.accessToken === '' || this.cred.refreshToken === '') {
      console.error('Access token or refresh token is missing');
      return;
    }
  
    try {
      // Perform the HTTP POST request to get user information
      const response = await firstValueFrom(
        this.http.post<UserResponseDTO>(
          `${addr.ipaddr}/Account/getUserInfo`,
          this.cred,
          {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
          }
        )
      );
  
      // Handle the response
      this.userName = response.nickName;
      this.credits = response.credits;
      this.userRole = response.userType;
      this.avatar = response.avatar;
    } catch (error) {
      // Handle potential errors
      console.error('Failed to get user info:', error);
    }
  }
}
