import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ScheduleComponent } from './schedule/schedule.component';

const routes: Routes = [{
  path: '',
  component: LoginComponent,
  title: 'TeaWithColleagues',
},
{
  path: 'schedule',
  component: ScheduleComponent,
  title: 'TeaWithColleagues',
},
{
  path: 'register',
  component: RegisterComponent,
  title: 'TeaWithColleagues',
},];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
