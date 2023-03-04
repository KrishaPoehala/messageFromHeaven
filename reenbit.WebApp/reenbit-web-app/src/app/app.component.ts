import { ResponseType } from './enums/response-type';
import { environment } from './../environments/environment';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpEventType, HttpHeaders, HttpRequest } from '@angular/common/http'
import { catchError, Subscription, throwError } from 'rxjs';
import { TitleStrategy } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'reenbit-web-app';
  constructor(private fb:FormBuilder, private http:HttpClient){
  }

  fileForm = this.fb.group({
    'email':['',[Validators.required,Validators.email]],
  })

  file:File | null = null;
  fileName:string | null = null;
  progress = 0;
  result:ResponseType | null = null;
  responseType = ResponseType;
  request:Subscription | null = null;
  onSubmit(){
    this.submited = true;
    if(!this.file || this.fileForm.invalid){
      return;
    }

    const email = this.fileForm.get('email')?.value;
        const data = new FormData();
    data.append(this.file!.name, this.file!);
    console.log(this.file!.name);

    this.result = ResponseType.InProgress;
    this.request = this.http.post(environment.api +`Files/upload/${email}`,data)
    .pipe(
      catchError(_ => {
        this.result = ResponseType.Error;
        return throwError(() => new Error('Something went wrong'));
      })
    )
    .subscribe(_ => {
      this.result = ResponseType.Success;
      setTimeout(() => this._reset(),4000);
  });
  }
  
  submited = false;
  onFileChange(event:any){
    console.log(event.target.files);
    this.file = event.target.files[0];
    if(!this.file){
      return;
    }

    this.progress = 0;
    this.fileName = this.file!.name;
  }

  onCancel(){
    this.request?.unsubscribe();
    this.result = ResponseType.Canceled;
    setTimeout(() => this.result = null,3000);
  } 

  private _reset(){
    this.fileForm.reset();
    this.fileName = null;
    this.file = null;
    this.submited = false;
    this.result = null;
  }
}
