import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private http: HttpClient) { }
  data: any;
  title = 'SSPetAng';
  ngOnInit(): void {
    this.http.post<any>('https://localhost:7239/api/Stream/start', 1).subscribe((response) => {
      this.data = response;
      console.log(this.data);
    })
  }
}
