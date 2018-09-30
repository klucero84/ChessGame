import { Component, OnInit, HostListener, ViewChild, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../_models/user';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  @Input() user: User;

  @ViewChild('userTabs') userTabs: TabsetComponent;

 constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.route.queryParams.subscribe(params => {
      const selectedTab = params['tab'];
      this.userTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
    });
  }

  updateUser() {

  }

  selectTab(tabId: number) {
    this.userTabs.tabs[tabId].active = true;
  }

}
