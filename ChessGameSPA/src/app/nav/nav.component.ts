import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('logged in successfully');
    },
    error => {
      this.alertify.error(error);
      // console.log('Failed to Login');
    }, () => {
      this.router.navigate(['/home']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  loginDemo() {
    this.authService.login({'email': 'kenneth@kennethlucero.com', 'password': 'password'}).subscribe(next => {
      this.alertify.success('logged in with demo.');
    },
    error => {
      this.alertify.error(error);
    },
    () => {
      this.router.navigate(['/home']);
    });
  }

  logout() {
    this.authService.logout();
    this.alertify.message('loggged out');
    this.router.navigate(['/home']);
  }
}
