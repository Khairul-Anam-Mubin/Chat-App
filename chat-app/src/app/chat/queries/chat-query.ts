import { QueryBase } from "src/app/core/models/query-base";
import { Configuration } from "src/app/core/services/configuration";

export class ChatQuery extends QueryBase {
    
    receiverId : any;
    isGroupMessage: any;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi + "/chat/get";
        this.Offset = 0;
        this.limit = 30;
    }
}