import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
    providedIn: "root",
})
export class LoadingService {
    private loadingSubject =
        new BehaviorSubject<boolean>(false);

    loading$ = this.loadingSubject.asObservable();

    constructor() {
        this.loadingOff();
    }

    loadingOn() {
        this.loadingSubject.next(true);

    }

    loadingOff() {
        this.loadingSubject.next(false);

    }
}
