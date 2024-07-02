import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GamesComponent } from '../games.component';

@Component({
  selector: 'app-info-dialog',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.css']
})
export class InfoDialogComponent {
[x: string]: any;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {}
}