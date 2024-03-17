import { QueryBase } from "src/app/core/models/query-base";
import { Configuration } from "src/app/core/services/configuration";

export class ChatByIdsQuery extends QueryBase {
    
    messageIds: any;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi + "/chat/chats-by-ids";
        this.Offset = 0;
        this.limit = 30;
    }
}