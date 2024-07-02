import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, output } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { UserResponseDTO } from '../../../models/user.models';
import { UserType } from '../../../models/UserRole';
import { AuthenticatedResponse } from '../../../models/authenticated-response';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit, OnDestroy {
  @Input() userName: string = '';
  @Input() credits: number = 0;
  @Input() userRole: UserType = UserType.User;
  @Input() avatar: string = '';
  userRoleEnum: typeof UserType = UserType
  @Output() newItemEvent = new EventEmitter<string>();
  isDropdownOpen = false;
  hasAvatar = false;

  constructor(private authService: AuthService, private http: HttpClient) {}
  intervalId: any;
  ngOnInit() {
    // Call the methods every second
    this.intervalId = setInterval(() => {
      this.getUserInfo();
      this.hasAvatar = !!this.avatar;
   
    }, 300);
  }


  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
  cred: AuthenticatedResponse = {
    accessToken: localStorage.getItem('accessToken') ?? '',
    refreshToken: localStorage.getItem('refreshToken') ?? '',
  };
  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  isUserAuthenticated = (): boolean => {
    return this.authService.isUserAuthenticated();
  };
  async getUserInfo(): Promise<void> {
    this.cred.accessToken = localStorage.getItem('accessToken') ?? '';
    this.cred.refreshToken = localStorage.getItem('refreshToken') ?? '';

    if (!this.cred.accessToken || !this.cred.refreshToken) {
      this.userName = 'anonymous';
      this.credits = 0;
      this.userRole = UserType.User;
      this.avatar = '';
      return;
    }

    try {
      const response: UserResponseDTO = await firstValueFrom(
        this.http.post<UserResponseDTO>(
          'https://localhost:7063/Account/getUserInfo',
          this.cred,
          {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
          }
        )
      );

      this.userName = response.nickName;
      this.credits = response.credits;
      this.userRole = response.userType;
      this.avatar = response.avatar;
    } catch (err) {
      console.error('Error fetching user info:', err);
      this.userName = 'anonymous';
      this.credits = 0;
      this.userRole = UserType.User;
      this.avatar = '';
      alert('An error occurred while fetching user information. Please try again.');
    }
  }

  logOut() {
    this.authService.logOut();
  }
}
