import { CommandBase } from "src/app/core/models/command-base";
import { Configuration } from "src/app/core/services/configuration";

export class AddMemberToGroupCommand extends CommandBase{
    
    groupId: any;
    memberId: any;
    addedBy: any;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/Group/add-memeber");
    }
}