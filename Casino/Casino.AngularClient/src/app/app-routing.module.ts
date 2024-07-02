import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './gui/login/login.component';
import { HelpsiteComponent } from './gui/helpsite/helpsite.component';
import { RegisterComponent } from './gui/register/register.component';
import { MainComponent } from './gui/main/main.component';
import { SlotsyComponent } from './gui/slotsy/slotsy.component';
import { AdminPanelUserComponent } from './gui/admin-panel/admin-panel-user/admin-panel-user.component';
import { AdminPanelFinanceComponent } from './gui/admin-panel/admin-panel-finance/admin-panel-finance.component';
import { ProfileComponent } from './gui/user-panel/profile/profile.component';
import { TransactionComponent } from './gui/user-panel/transaction/transaction.component';
import { MatchHistoryComponent } from './gui/user-panel/match-history/match-history.component';
import { RouletteComponent } from './gui/roulette/roulette.component';
import { TransactionHistoryComponent } from './gui/user-panel/transaction-history/transaction-history.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent},
  { path: 'helpsite', component: HelpsiteComponent },
  { path: 'slotsy', component: SlotsyComponent },
  { path: 'admin-user', component: AdminPanelUserComponent },
  { path: 'admin-financial', component: AdminPanelFinanceComponent },
  { path: 'user-profile', component: ProfileComponent },
  { path: 'user-transaction', component: TransactionComponent },
  { path: 'user-history', component: MatchHistoryComponent },
  { path: 'roulette', component: RouletteComponent },
  { path: '', component: MainComponent },
  {path:'transaction-history',component:TransactionHistoryComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
