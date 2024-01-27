import { Injectable } from "@angular/core";
import { NotificationData } from "../models/notification-data";
import { Observable, Subject } from "rxjs";
import { SignalRService } from "./signalr-service";

@Injectable()
export class SocketService {

    private subscribedSubjects: any = {};
    private subscriberCount: any = {};

    constructor(
        private signalRService: SignalRService) {

        this.signalRService.notificationObservable().subscribe(notification => {
            console.log('SocketService-Constructor()-Notification Received', notification);
            if (notification && notification.topic && this.subscribedSubjects[notification.topic]) {
                console.log('SocketService-Constructor-Notification Sending to subscribed Topic', notification);
                this.subscribedSubjects[notification.topic].next(notification);
            }
        });
    }

    subscribeToTopic(topic: string) : Observable<NotificationData> {
        console.log('SocketService-subscribeToTopic()-subscribing to Topic ', topic);
        if (this.subscribedSubjects[topic]) {
            this.subscriberCount[topic]++;
            console.log('SocketService-subscribeToTopic()-existing Topic subject found for Topic: ', topic);
            return this.subscribedSubjects[topic].asObservable();
        }
        this.subscriberCount[topic] = 1;
        this.subscribedSubjects[topic] = new Subject<any>();
        console.log('SocketService-subscribeToTopic()- create and return newly created Topic subject ', topic);
        return this.subscribedSubjects[topic].asObservable();
    }

    // subscription needs to be unsubscribing
    unsubscribeTopic(topic: string) {
        console.log('SocketService-unsubscribeTopic()-unsubscribing to Topic ', topic);
        if (!this.subscribedSubjects[topic]) {
            console.log('SocketService-unsubscribeTopic()-No subject found', topic);
            return;
        }
        this.subscriberCount[topic]--;
        if (this.subscriberCount[topic] === 0) {
            delete this.subscribedSubjects[topic];
            console.log('SocketService-unsubscribeTopic()-delete subject ', this.subscribedSubjects);
        }
    }
}