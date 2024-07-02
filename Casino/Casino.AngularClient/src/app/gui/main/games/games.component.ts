import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { GameService } from '../../services/game.service';
// import { GameDTO } from '../../models/game.model';
import { InfoDialogComponent } from './info/info.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-games',
  templateUrl: './games.component.html',
  styleUrls: ['./games.component.css'],
})
export class GamesComponent { //implements OnInit 
  // games: GameDTO[] = [];

  // constructor(private gameService: GameService) {}

  // ngOnInit(): void {
  //   this.fetchGames();
  // }

  // fetchGames(): void {
  //   this.gameService.getGames().subscribe((games) => {
  //     this.games = games;
  //   });
  // }
  constructor(public dialog: MatDialog) {}

  openInfoDialog(gameType: string): void {
    let title = '';
    let content = '';

    if (gameType === 'game-slots') {
      title = 'Information about the Slot Machine Game';
      content = 'In the Slot Machine game, your goal is to get at least 2 identical symbols next to each other \n';
      content += '\n The minimum bet value is 25 \n';
      content += '\n You must place a bet to play \n';
      content += '\n The more repetitions of the same symbol, the greater the reward \n';
      content += '\n The types of symbols affect the size of the win \n';
    }
    
    if (gameType === 'game-bandit') {
      title = 'Information about the Roulette Game';
      content = 'In the Roulette game, your goal is to correctly predict the outcome of the draw \n';
      content += '\n The minimum bet value is 25 \n';
      content += '\n Place the amount you want to bet \n';
      content += '\n You must choose a color and place a bet to play \n';
      content += '\n You can choose only one color \n';
      content += '\n You can bet on an exact number to win even more \n';
    }
    

    const dialogRef = this.dialog.open(InfoDialogComponent, {
      maxWidth: '90vw',
      data: {
        title: title,
        content: content
      }
    });
  }

}
