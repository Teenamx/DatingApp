import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { MemberService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  m=[1,1,2];
  members:Member[]=[];
  constructor(private memberService:MemberService) { }

  ngOnInit(): void {

    const user:User=JSON.parse(localStorage.getItem('user'));
       console.log("hi");

         this.loadMembers();



  }

  loadMembers()
  {
    this.memberService.getMembers().subscribe(member=>
      {
      this.members=member;
      console.log(this.members);
      })
  }


}
