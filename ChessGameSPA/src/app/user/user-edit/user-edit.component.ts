import { Component, OnInit, ViewChild, HostListener, Input } from '@angular/core';
import { User } from '../../_models/user';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  @Input() user: User;
  @ViewChild('editForm') editForm: NgForm;

  @ViewChild('userEditTabs') userEditTabs: TabsetComponent;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }



 constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.route.queryParams.subscribe(params => {
      const selectedTab = params['tab'];
      this.userEditTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
    });
  }

  updateUser() {

  }

  selectTab(tabId: number) {
    this.userEditTabs.tabs[tabId].active = true;
  }

}
