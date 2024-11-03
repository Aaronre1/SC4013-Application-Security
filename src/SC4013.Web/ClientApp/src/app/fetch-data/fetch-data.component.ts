import { Component } from '@angular/core';
import {WeatherForecast, WeatherForecastsClient} from "../web-api-client";

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  constructor(private client: WeatherForecastsClient) {
    client.getWeatherForecasts().subscribe({
      next: result => this.forecasts = result,
      error: error => console.error(error)
    })
  }
}

