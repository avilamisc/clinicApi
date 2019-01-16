import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AuthComponent } from 'src/app/components/auth/auth.component';
import { AuthRoutingModule } from './auth-riuting.module';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AuthComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AuthRoutingModule
  ],
  providers: []
})
export class AuthModule { }
