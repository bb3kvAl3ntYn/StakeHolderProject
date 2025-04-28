import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { StakeholderComponent } from './components/stakeholder/stakeholder.component';
import { StakeholderFormComponent } from './components/stakeholder/stakeholder-form/stakeholder-form.component';
import { VisitComponent } from './components/visit/visit.component';
import { VisitFormComponent } from './components/visit/visit-form/visit-form.component';
import { AuthGuard } from './services/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { 
    path: 'stakeholder', 
    component: StakeholderComponent, 
    canActivate: [AuthGuard] 
  },
  {
    path: 'stakeholder/add',
    component: StakeholderFormComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'stakeholder/edit/:id',
    component: StakeholderFormComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'stakeholder/:id',
    component: StakeholderFormComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'visit', 
    component: VisitComponent, 
    canActivate: [AuthGuard] 
  },
  {
    path: 'visit/add',
    component: VisitFormComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'visit/edit/:id',
    component: VisitFormComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'visit/:id',
    component: VisitFormComponent,
    canActivate: [AuthGuard]
  },
  // Default route for invalid URLs
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
