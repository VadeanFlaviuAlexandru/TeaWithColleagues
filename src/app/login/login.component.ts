import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { UserL } from '../models/UserL';
import { IdUserService } from '../id-user.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  mailAddress = new FormControl('');
  password = new FormControl('');

  constructor(private router: Router, private http: HttpClient,private IdUserService: IdUserService) { }

  ngOnInit(): void {
  }

  LogInForm = new FormGroup({
    mailAddress: new FormControl(),
    password: new FormControl()
  })

  LoginUser() {
    let UserL: UserL = {
      mailAddress: this.LogInForm.get('mailAddress')?.value,
      password: this.LogInForm.get('password')?.value,
    }
    this.SignUserL(UserL).subscribe((response) => {
      if (response.statusText == "OK" && response.body) {
        const responseBody = JSON.parse(response.body);
        this.IdUserService.setIdUser(responseBody.idUser);
        console.log("Log-in-ing in with id:")
        console.log(this.IdUserService.getIdUser())
        this.router.navigate(['schedule']);
      }
    });
  }

  SignUserL(UserL: any) {
    return this.http.post(`${environment.BaseUrl}/Login/login`, UserL, {
      observe: 'response',
      responseType: 'text',
    });
  }

  SignupUser() {
    this.router.navigate(['register']);
  }


}