import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { User } from '../../_models/user';
import { ActivatedRoute } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap';


@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() user: User;


  @ViewChild('userCardTabs') userCardTabs: TabsetComponent;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {

    this.route.queryParams.subscribe(params => {
      const selectedTab = params['tab'];
      this.userCardTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
    });

  }


  selectTab(tabId: number) {
    this.userCardTabs.tabs[tabId].active = true;
  }
}
