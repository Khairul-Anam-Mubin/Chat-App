import { QueryBase } from "src/app/core/models/query-base";
import { Configuration } from "src/app/core/services/configuration";

export class UserGroupsQuery extends QueryBase{
    
    userId : string | undefined | null;
    
    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/Group/user-groups");
    }
}