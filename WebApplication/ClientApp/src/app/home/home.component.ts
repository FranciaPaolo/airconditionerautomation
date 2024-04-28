import { Component } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { ClimaService, Clima_AcStatus, Clima_EnvironmentStatus } from '../services/clima.service';
import { LoadingService } from '../services/loading.service';
import { forkJoin } from 'rxjs';
import { Point } from 'highcharts';
import * as Highcharts from 'highcharts';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  chartEnvStatus: Highcharts.Chart | undefined;
  chartEnvStatus_count: number = 0;

  ac_mode: number = 1; // 1 heat, 3 cool
  ac_on: number = 0; // 0 spento, 1 acceso
  ac_temp: number = 0;
  ac_changed: boolean = false;
  last_date: string = "";
  last_temperature: number = 0;
  last_humidity: number = 0;

  env_status_list: Clima_EnvironmentStatus[] = [];

  constructor(
    private climaService: ClimaService,
    private loadingService: LoadingService
  ) {
  }

  ngOnInit() {

    // aggiorno lo stato del clima: ambiente e stato aria condizionata
    this.getClimtaStatus();

    // imposto il grafico
    this.chartEnvStatus = Highcharts.chart('chart-line', {
      chart: {
        type: 'line',
      },
      title: {
        text: '',
      },
      credits: {
        enabled: false,
      },
      legend: {
        enabled: true
      },
      yAxis: [{
        title: {
          text: '°C',
        }
      }, {
        title: {
          text: '%',
          opposite: true
        }
      }],
      xAxis: {
        type: 'category',
      },
      tooltip: {
        headerFormat: `<div>Date: {point.key}</div>`,
        pointFormat: `<div>{series.name}: {point.y}</div>`,
        shared: true,
        useHTML: true,
      },
      series: [{
        name: 'Temperatura °C',
        yAxis: 0,
        showInLegend: true,
      }, {
        name: 'Umidità %',
        yAxis: 1,
        showInLegend: true,
      }],
    } as any);
  }

  btnRefreshClick() {
    this.getClimtaStatus();
  }

  btnSendCommandClick() {

    this.loadingService.loadingOn();
    this.climaService.setAcStatus(new Clima_AcStatus(this.ac_on == 1, this.ac_mode, this.ac_temp))
      .subscribe((data) => {
        this.loadingService.loadingOff();

        console.log(data);
        if(data==null){
          alert("Error connecting with device");
        }
      });
  }

  getClimtaStatus() {
    this.loadingService.loadingOn();
    // carico lo stato del condizionatore
    forkJoin({
      requestOne: this.climaService.getAcStatus(),
      requestTwo: this.climaService.getEnvStatusList()
    })
      .subscribe(({ requestOne, requestTwo }) => {
        this.ac_on = requestOne.power ? 1 : 0;
        this.ac_mode = requestOne.mode;
        this.ac_temp = requestOne.temperature;
        this.last_date = requestOne.date;

        this.env_status_list = requestTwo;

        let evnStatus = this.climaService.getEnvStatus();
        this.last_temperature = evnStatus.temperature;
        this.last_humidity = evnStatus.humidity;
        this.last_date = evnStatus.date;

        this.setEnvStatus_ChartData(this.env_status_list);

        this.loadingService.loadingOff();
      });
  }

  ac_on_selectChange($event: any) {
    this.ac_changed = true;
  }

  ac_temp_selectChange($event: any) {
    this.ac_changed = true;
  }

  ac_mode_selectChange($event: any) {
    this.ac_changed = true;
  }

  setEnvStatus_ChartData(dataArray: Clima_EnvironmentStatus[]) {

    for (let i = 0; i < this.chartEnvStatus_count; i++) {
      this.chartEnvStatus?.series[0].removePoint(0,false);
      this.chartEnvStatus?.series[1].removePoint(0, false);
    }

    dataArray.forEach((data, index) => {
      this.chartEnvStatus?.series[0].addPoint([data.date, data.temperature], false);
      this.chartEnvStatus?.series[1].addPoint([data.date, data.humidity], false);
    });
    this.chartEnvStatus_count = dataArray.length;
    this.chartEnvStatus?.redraw();
  }

}
