import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { UserR } from '../models/UserR';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  name= new FormControl('');
  surname= new FormControl('');
  mailAddress=new FormControl('');
  password=new FormControl('');
  phoneNumber=new FormControl('');
  
  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
  }

  SignUpForm = new FormGroup({
    name: new FormControl(),
    surname: new FormControl(),
    mailAddress: new FormControl(),
    password: new FormControl(),
    phoneNumber: new FormControl(),
  })

  SignUpUser() {
    let UserR: UserR = {
      name: this.SignUpForm.get('name')?.value,
      surname: this.SignUpForm.get('surname')?.value,
      mailAddress: this.SignUpForm.get('mailAddress')?.value,
      password: this.SignUpForm.get('password')?.value,
      phoneNumber: this.SignUpForm.get('phoneNumber')?.value,
    }
    console.log(UserR)
    this.addUserL(UserR).subscribe((response) => {
      console.log(response);
      if(response.statusText == "OK")
            this.router.navigate(['']);
      });
  }
  addUserL(UserR: any) {
    return this.http.post(`${environment.BaseUrl}/Register/sign-up-user`, UserR, {
      observe: 'response',
      responseType: 'text',
    });
  }
  GoBack(){
    this.router.navigate(['']);
  }
}