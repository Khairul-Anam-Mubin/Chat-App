import { Injectable } from "@angular/core";
import { Observable, interval, mergeMap } from "rxjs";
import { CommandService } from "src/app/core/services/command-service";
import { UpdateLastSeenCommand } from "../commands/update-last-seen-command";
import { UserService } from "src/app/identity/services/user.service";

@Injectable()
export class ActivityService{

    private isRecurringTrackActivityRunning: boolean = false;
    
    constructor(
        private commandService: CommandService,
        private userService: UserService) {}

    startRecurringTrackActivity() {
        if (this.isRecurringTrackActivityRunning) {
            return;
        }
        this.isRecurringTrackActivityRunning = true;
        this.trackLastSeenActivity();
        
        interval(30 * 1000)
        .pipe(mergeMap(() => this.trackLastSeenActivity()))
        .subscribe(data => console.log(data));
    }

    trackLastSeenActivity() : Observable<any> {
        console.log('Track Last Seen Activity');
        const command = new UpdateLastSeenCommand();
        command.isActive = true;
        command.userId = this.userService.getCurrentUserId();
        return this.commandService.execute(command);
    }

}