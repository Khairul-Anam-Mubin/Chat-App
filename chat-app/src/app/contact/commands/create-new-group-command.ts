import { CommandBase } from "src/app/core/models/command-base";
import { Configuration } from "src/app/core/services/configuration";

export class CreateNewGroupCommand extends CommandBase {
    
    public userId : string | undefined;
    public groupName : string | undefined;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/Group/create");
    }
}