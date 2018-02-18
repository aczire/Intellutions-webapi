import { Component, OnInit } from '@angular/core';

import { HomeDetails } from '../models/home.details.interface';
import { DashboardService } from '../services/dashboard.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  homeDetails: HomeDetails;
  public username: string;

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {

    this.dashboardService.getHomeDetails()
    .subscribe((homeDetails: HomeDetails) => {
		//console.log(homeDetails);
		this.username = homeDetails.username;
		console.log(this.username);
    },
    error => {
      //this.notificationService.printErrorMessage(error);
    });
    
  }

}
