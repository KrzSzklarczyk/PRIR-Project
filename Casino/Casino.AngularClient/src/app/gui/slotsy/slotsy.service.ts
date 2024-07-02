import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SlotsyService {
  iconMap = ["banana", "seven", "cherry", "plum", "orange", "bell", "bar", "lemon", "melon"];
  icon_width = 79;
  icon_height = 79;
  num_icons = 9;
  time_per_icon = 100;
  indexes: number[] = [0, 0, 0];

  constructor() {}

  roll(reel: HTMLElement, offset = 0): Promise<number> {
    const delta = (offset + 2) * this.num_icons + Math.round(Math.random() * this.num_icons);

    return new Promise((resolve) => {
      const style = getComputedStyle(reel);
      const backgroundPositionY = parseFloat(style.backgroundPositionY);
      const targetBackgroundPositionY = backgroundPositionY + delta * this.icon_height;
      const normTargetBackgroundPositionY = targetBackgroundPositionY % (this.num_icons * this.icon_height);

      setTimeout(() => {
        reel.style.transition = `background-position-y ${(8 + 1 * delta) * this.time_per_icon}ms cubic-bezier(.41,-0.01,.63,1.09)`;
        reel.style.backgroundPositionY = `${backgroundPositionY + delta * this.icon_height}px`;
      }, offset * 150);

      setTimeout(() => {
        reel.style.transition = `none`;
        reel.style.backgroundPositionY = `${normTargetBackgroundPositionY}px`;
        resolve(delta % this.num_icons);
      }, (8 + 1 * delta) * this.time_per_icon + offset * 150);
    });
  }
}
