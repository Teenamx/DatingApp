import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {

  ngOnInit(): void {
    this.getPhotoForApproval();
  }
  constructor(private adminService:AdminService) { }


  photos:Photo[];
  getPhotoForApproval()
  {
    this.adminService.getPhotosForApproval().subscribe(photos=>
      {
        this.photos=photos
        console.log(this.photos);
      })
  }

  approvePhoto(photoId)
  {
    this.adminService.approvePhoto(photoId).subscribe(()=>
    {
      this.photos.splice(this.photos.findIndex(p=>p.id===photoId),1);
    })
  }

  rejectPhoto(photoId)
  {
    this.adminService.rejectPhoto(photoId).subscribe(()=>{
      this.photos.splice(this.photos.findIndex(p=>p.id===photoId),1);
    })
  }

}
