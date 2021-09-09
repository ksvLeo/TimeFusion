import { EventEmitter } from "@angular/core"

export class ActionInfo {

    enable: boolean
    label: string
    event: EventEmitter<any> = new EventEmitter<any>();
}