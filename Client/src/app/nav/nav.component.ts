import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private accountService:AccountService) { }
  model:any={};
  currentuser$: Observable<User>;
  ngOnInit(): void {
    this.currentuser$=this.accountService.currentUser$;
   // this.getCurrentUser();
  }
  login()
  {
    this.accountService.login(this.model).subscribe(response=>
      {
    //    this.loggedIn=true;
        console.log(response);
      },
      (error)=>{
        console.log(error);
      }
      );
  }

  logOut()
  {

    this.accountService.logout();
   // this.loggedIn=false;
  }

  getCurrentUser()
  {
    this.accountService.currentUser$.subscribe(user=>{
   //  this.loggedIn=!!user;
    },
    error=>{
      console.log(error);
    }
    )
  }

}
