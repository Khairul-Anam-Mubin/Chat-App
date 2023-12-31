import { QueryBase } from "src/app/core/models/query-base";
import { Configuration } from "src/app/core/services/configuration";

export class UserProfileQuery extends QueryBase{

    emails : any;
    userIds : any;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/User/profiles");
    }
}
