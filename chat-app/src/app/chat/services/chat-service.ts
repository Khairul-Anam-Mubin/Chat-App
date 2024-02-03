import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable()
export class ChatService{

    private openedChatType : string;
    private openedGroupChatId: string | undefined;

    constructor(private router: Router) {
        this.openedChatType = 'UserChat';
    }

    openChat(data: any) {
        this.openedChatType = data.type;
        if (this.openedChatType === 'GroupChat') {
            this.openedGroupChatId = data.id;
        }
        this.router.navigateByUrl('chat/' + data.id);
    }

    getOpenedChatType() : string{
        return this.openedChatType;
    }

    getOpenedGroupChatId() : string | undefined{
        return this.openedGroupChatId;
    }
}