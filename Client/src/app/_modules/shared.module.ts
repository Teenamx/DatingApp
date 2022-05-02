import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import {TabsModule} from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    NgxGalleryModule ,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot(
      {
        positionClass:'toast-bottom-right'
      }
    ),
    TabsModule.forRoot(),

  ],
  exports:[
    NgxGalleryModule,
    BsDropdownModule,
    ToastrModule,
    TabsModule,

  ]
})
export class SharedModule { }
