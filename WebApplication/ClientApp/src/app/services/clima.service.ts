import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, delay, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClimaService {

  ac_status: Clima_AcStatus = new Clima_AcStatus();
  env_status: Clima_EnvironmentStatus = new Clima_EnvironmentStatus();
  env_status_list: Clima_EnvironmentStatus[] = []; // ultimi 100 dati
  baseUrl: string;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this.baseUrl = baseUrl;
  }

  getAcStatus(): Observable<Clima_AcStatus> {

    return this.http.get<Clima_AcStatus>(this.baseUrl + 'climate/getachistory?roomId=1');
    //fake data
    // this.ac_status = {
    //   date: "2024-04-01 20:30",
    //   mode: 1,
    //   temperature: 21,
    //   power: false
    // };
    // return of(this.ac_status).pipe(delay(2000));
  }

  getEnvStatusList(): Observable<Clima_EnvironmentStatus[]> {

    return this.http.get<Clima_EnvironmentStatus[]>(this.baseUrl + 'climate/getclimahistory?roomId=1')
      .pipe(tap(x => {
        this.env_status_list = x;
        if (this.env_status_list.length > 0)
          this.env_status = this.env_status_list[this.env_status_list.length - 1];
        else
          this.env_status = new Clima_EnvironmentStatus();
      }));
    //fake data
    // this.env_status_list = [
    //   {
    //     date: "2024-04-01 08:00",
    //     temperature: 23,
    //     humidity: 29
    //   },
    //   {
    //     date: "2024-04-01 08:15",
    //     temperature: 22.5,
    //     humidity: 29
    //   },
    //   {
    //     date: "2024-04-01 08:30",
    //     temperature: 22,
    //     humidity: 29
    //   },
    //   {
    //     date: "2024-04-01 08:45",
    //     temperature: 22,
    //     humidity: 29
    //   },
    //   {
    //     date: "2024-04-01 09:00",
    //     temperature: 22,
    //     humidity: 30
    //   },
    //   {
    //     date: "2024-04-01 09:15",
    //     temperature: 21,
    //     humidity: 31
    //   },
    //   {
    //     date: "2024-04-01 09:30",
    //     temperature: 21,
    //     humidity: 32
    //   },
    //   {
    //     date: "2024-04-01 09:45",
    //     temperature: 21,
    //     humidity: 31
    //   },
    //   {
    //     date: "2024-04-01 10:00",
    //     temperature: 20,
    //     humidity: 31
    //   },
    //   {
    //     date: "2024-04-01 10:00",
    //     temperature: 20,
    //     humidity: 30
    //   }
    // ];

    // if (this.env_status_list.length > 0)
    //   this.env_status = this.env_status_list[this.env_status_list.length - 1];
    // else
    //   this.env_status = new Clima_EnvironmentStatus();

    // return of(this.env_status_list).pipe(delay(2500));
  }

  getEnvStatus() {
    return this.env_status;
  }

  setAcStatus(acStatus: Clima_AcStatus): Observable<boolean> {

    let param = {
      room_id: 1,
      power: acStatus.power ? 1 : 0,
      mode: acStatus.mode,
      temperature: acStatus.temperature,
      date: ""
    };

    return this.http.post<boolean>(this.baseUrl + 'climate/setac', param);

    // fake
    //return of(true).pipe(delay(1500));;
  }

}


export class Clima_AcStatus {
  power: boolean = false;
  mode: number = 1; // 1 heat, 3 cool
  temperature: number = 20;
  date: string = "";

  constructor(power?: boolean, mode?: number, temperature?: number) {
    if (power != undefined) {
      this.power = power;
    }
    if (mode != undefined) {
      this.mode = mode;
    }
    if (temperature != undefined) {
      this.temperature = temperature;
    }
  }

}

export class Clima_EnvironmentStatus {
  temperature: number = 20;
  humidity: number = 30;

  date: string = "";
}