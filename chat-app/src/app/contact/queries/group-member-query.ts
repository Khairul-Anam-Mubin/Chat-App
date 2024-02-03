import { QueryBase } from "src/app/core/models/query-base";
import { Configuration } from "src/app/core/services/configuration";

export class GroupMembersQuery extends QueryBase{
    groupId : any;
    
    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/Group/group-members");
    }
}