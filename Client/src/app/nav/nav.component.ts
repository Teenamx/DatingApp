import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private accountService:AccountService,private router:Router,
    private toastr:ToastrService) { }
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
        const user:User=JSON.parse(localStorage.getItem('user'));

        
         this.router.navigateByUrl("/members");


      }

      );
  }

  logOut()
  {

    this.accountService.logout();
    this.router.navigateByUrl("/");
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
