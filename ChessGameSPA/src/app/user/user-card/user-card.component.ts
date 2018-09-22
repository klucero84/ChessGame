import { Component, OnInit, Input } from '@angular/core';
import { User } from '../../_models/user';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() user: User;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
  }

}
