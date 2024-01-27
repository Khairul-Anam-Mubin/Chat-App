import { CommandBase } from "src/app/core/models/command-base";
import { Configuration } from "src/app/core/services/configuration";

export class UpdateLastSeenCommand extends CommandBase{
    isActive: any;
    userId: any;
    constructor() {
        super();
        this.apiUrl = Configuration.activityApi + "/activity/track-last-seen";
    }
}