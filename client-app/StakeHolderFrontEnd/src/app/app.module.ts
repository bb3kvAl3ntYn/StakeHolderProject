import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { StakeholderComponent } from './components/stakeholder/stakeholder.component';
import { StakeholderFormComponent } from './components/stakeholder/stakeholder-form/stakeholder-form.component';
import { VisitComponent } from './components/visit/visit.component';
import { VisitFormComponent } from './components/visit/visit-form/visit-form.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { AuthInterceptor } from './services/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    StakeholderComponent,
    StakeholderFormComponent,
    VisitComponent,
    VisitFormComponent,
    NavMenuComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
