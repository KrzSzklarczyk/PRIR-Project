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

  changeAvatar() {
    const awa = this.changePasswordForm.get('newAvatar')?.value;
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

    const us: UserChangeDTO = { token: this.cred, cos: awa };
    if (this.cred.accessToken == '' || this.cred.refreshToken == '') {
      return;
    }

    this.http
      .put<boolean>('https://localhost:7063/Account/ChangeAvatar', us, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      })
      .subscribe({
        next: (response: boolean) => {
          console.log('testtttt');
          this.getUserInfo();
          return;
        },
        error: (err: HttpErrorResponse) => {
          return;
        },
      });
    this.getUserInfo();
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

  changePassword() {
    if (this.changePasswordForm.valid) {
      const newPassword = this.changePasswordForm.get('newPassword')?.value;
      this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
      this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

      const us: UserChangeDTO = { token: this.cred, cos: newPassword };
      if (this.cred.accessToken == '' || this.cred.refreshToken == '') {
        return;
      }

      this.http
        .put<boolean>('https://localhost:7063/Account/ChangePasswd', us, {
          headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        })
        .subscribe({
          next: (response: boolean) => {
            return;
          },
          error: (err: HttpErrorResponse) => {
            return;
          },
        });
    }
    if(!this.changePasswordForm.valid) {
        alert("Invalid Password!!! Password should have : small letters, UpperCase, digits and special characters. Please try again."); 
    }
  }

  deleteAccount() {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

    if (this.cred.accessToken == '' || this.cred.refreshToken == '') {
    } else {
      this.http
        .put<boolean>('https://localhost:7063/Account/RemoveAcc', this.cred, {
          headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        })
        .subscribe({
          next: (response: boolean) => {},
        });
    }
    this.logOut();
    this.router.navigate(['/']);
  }

  logOut() {
    // Logic to log out

    this.getUserInfo();
    this.authService.logOut();
  }

  getUserInfo() {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

    if (this.cred.accessToken == '' || this.cred.refreshToken == '') {
    } else {
      this.http
        .post<UserResponseDTO>(
          'https://localhost:7063/Account/getUserInfo',
          this.cred,
          {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
          }
        )
        .subscribe({
          next: (response: UserResponseDTO) => {
            this.userName = response.nickName;
            this.credits = response.credits;
            this.userRole = response.userType;
            this.avatar = response.avatar;
          },
        });
    }
  }
}
