import { CommandBase } from "src/app/core/models/command-base";
import { UserModel } from "../models/user-model";
import { Configuration } from "src/app/core/services/configuration";

export class UpdateUserProfileCommand extends CommandBase {

    public firstName : any;
    public lastName : any;
    public birthDay : any;
    public about : any;
    public profilePictureId : any;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/User/update");
    }
}
