import { Component, OnInit, OnChanges, Output, EventEmitter, OnDestroy } from '@angular/core';
import { AlertifyService } from 'src/services/alertify.service';
import { User } from 'src/models/user';
import { AuthService } from 'src/services/auth.service';
import { Router } from '@angular/router';
import { SignalrService } from 'src/services/signalr.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  providers: [AuthService]
})
export class NavComponent implements OnInit, OnDestroy {
  model: any = {};
  photoUrl: string | undefined;
  user: User | undefined;
  @Output() isloginEvent = new EventEmitter<boolean>(false);
  constructor(public auth: AuthService, private alertify: AlertifyService,private router:Router, public signalrService: SignalrService) {
  }
  ngOnInit() {
    this.auth.authMeListenerSuccess();
    this.auth.authMeListenerFail();
    this.logOutLis();
    this.auth.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
    this.auth.user?.subscribe(d => {
      this.user = d;
      this.user.photoUrl = d.photoUrl;
      this.isloginEvent.emit(true);
    }, e => {
      console.log(e);
    }
    )
  }

  login(){
      if(this.model)
       this.auth.login(this.model).subscribe((d:any) =>{
         this.alertify.success('Logged in successfully');
          this.user = d.user;
          this.photoUrl = d.user.photoUrl;
          this.isloginEvent.emit(true);
          this.auth.authMe(d.id);
          console.log('I connet')
        }, e => {
         console.log(e);
       } );
       setTimeout(() => {
        location.reload();
       }, 1000);
       this.router.navigateByUrl('list-users');

  }
  loggedIn(){
    return this.auth.user;
  }
  logout(){
    this.auth.logout();
    this.alertify.success('logout ...!!! welcome back');
    this.logOut();
    this.router.navigateByUrl('');
    location.reload();
    }

  logOut(): void {
    this.signalrService.hubConnection?.invoke("logOut", this.user?.id)
    .catch(err => console.error(err));
  }
  logOutLis() {
    this.signalrService.hubConnection?.on("logoutResponse", () => {
      localStorage.removeItem("user");
      console.log('logout success');
    });
  }
  ngOnDestroy(): void {
    this.signalrService.hubConnection?.off("authMeResponseSuccess");
    this.signalrService.hubConnection?.off("authMeResponseFail");
  }
}
