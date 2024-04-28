import { Component, ContentChild, Input, TemplateRef } from '@angular/core';
import { LoadingService } from '../services/loading.service';
import { Observable, tap } from 'rxjs';
import { RouteConfigLoadEnd, RouteConfigLoadStart, Router } from '@angular/router';

@Component({
  selector: 'app-loading-indicator',
  templateUrl: './loading-indicator.component.html',
  styleUrls: ['./loading-indicator.component.css']
})
export class LoadingIndicatorComponent {
  loading$: Observable<boolean>;

  @Input()
  detectRouteTransitions = false;

  @ContentChild("loading")
  customLoadingIndicator: TemplateRef<any> | null = null;

  constructor(
    private loadingService: LoadingService,
    private router: Router) {
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit() {
    if (this.detectRouteTransitions) {
      this.router.events
        .pipe(
          tap((event) => {
            if (event instanceof RouteConfigLoadStart) {
              this.loadingService.loadingOn();
              console.log("loadingOn");
            } else if (event instanceof RouteConfigLoadEnd) {
              this.loadingService.loadingOff();
              console.log("loadingOff");
            }
          })
        )
        .subscribe();
    }
  }
}
