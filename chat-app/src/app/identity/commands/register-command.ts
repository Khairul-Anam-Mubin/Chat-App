import { CommandBase } from "src/app/core/models/command-base";
import { Configuration } from "src/app/core/services/configuration";

export class RegisterCommand extends CommandBase {
    public firstName : any;
    public lastName : any;
    public birthDay : any;
    public email : any;
    public password : string | undefined;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/User/register");
    }
}
