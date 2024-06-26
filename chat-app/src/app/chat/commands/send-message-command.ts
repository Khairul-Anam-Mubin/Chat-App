import { CommandBase } from "src/app/core/models/command-base";
import { Configuration } from "src/app/core/services/configuration";
import { ChatModel } from "../models/chat-model";

export class SendMessageCommand extends CommandBase {
    
    receiverId : any;
    messageContent : any;
    isGroupMessage: any;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi + "/chat/send"
    }
}