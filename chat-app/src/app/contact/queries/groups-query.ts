import { QueryBase } from "src/app/core/models/query-base";
import { Configuration } from "src/app/core/services/configuration";

export class GroupsQuery extends QueryBase{
    
    groupIds : any;
    
    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/Group/groups");
    }
}