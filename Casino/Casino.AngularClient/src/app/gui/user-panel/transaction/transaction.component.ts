import { Component } from '@angular/core';
import { TransactionService } from '../../../services/transaction.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrl: './transaction.component.css',
})
export class TransactionComponent {
  creditForm: FormGroup;
  amounts: number[] = [10, 50, 100, 200, 500, 1000, 5000, 10000];

  constructor(
    private transactionService: TransactionService,
    private fb: FormBuilder
  ) {
    this.creditForm = this.fb.group({
      cardNumber: [
        '',
        [Validators.required, Validators.pattern('^[0-9]{16}$')],
      ],
      expiry: [
        '',
        [Validators.required, Validators.pattern('^(0[1-9]|1[0-2])/[0-9]{2}$')],
      ],
      cvc: ['', [Validators.required, Validators.pattern('^[0-9]{3,4}$')]],
      ssn: ['', [Validators.required, Validators.pattern('^[0-9]{4}$')]],
      rememberCard: [false],
      amount: [Validators.required],
    });
  }

  credits: number = 0;

  ngOnInit(): void {
    this.creditForm.get('amount')?.valueChanges.subscribe((credits) => {
      console.log('Selected amount:', credits);
      this.credits = credits;
    });
  }

  addCredits() {
    if (this.credits !== null && this.credits > 0)
      this.transactionService.addCredits(this.credits);
  }
}
