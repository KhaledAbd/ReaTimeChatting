import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, AbstractControl, ValidationErrors, AsyncValidator, AsyncValidatorFn, Validator } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of, observable } from 'rxjs';
import { User } from 'src/models/user';
import { AlertifyService } from 'src/services/alertify.service';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  user: User | undefined
  registerForm: FormGroup | undefined;

  constructor(private authService: AuthService, private router: Router,
    private alertify: AlertifyService, private fb: FormBuilder) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      'gender' : new FormControl('', [Validators.required]),
      'username': new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(12)], this.isExist()),
      'knownAs': new FormControl('', [Validators.required, Validators.min(4), Validators.max(16)]),
      'dateOfBirth': new FormControl('', [Validators.required]),
      'city': new FormControl('', [Validators.required]),
      'country': new FormControl('', [Validators.required]),
      'password': new FormControl('',[Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      'passwordConfirm':new FormControl('', [Validators.required])
    }, null, [this.MustMatch(), ]);
  }


  register() {
    if (this.registerForm?.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      if(this.user)
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Registration successful');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/first']);
        });
      });
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
  isExist(): AsyncValidatorFn{
    return(control: AbstractControl): Observable<ValidationErrors| null> | Promise<ValidationErrors | null> => {
      this.authService.isExist(control.value).subscribe(d => {
        if(d){
          control.setErrors({isNotExist: true});
        }else{
          control.setErrors(null);
        }
      })
      console.log(control.errors);
      return of(null);
    }
  }
  MustMatch(): AsyncValidatorFn {
    return (): Observable<ValidationErrors | null>  |  Promise<ValidationErrors | null>=>{
      let isMismatch = false;
      if(this.registerForm?.get('password')?.value != this.registerForm?.get('passwordConfirm')?.value){
        isMismatch = true;
        this.registerForm?.get('passwordConfirm')?.setErrors({mismatch: true});
      }
      return of(null);
    }
  }
}
