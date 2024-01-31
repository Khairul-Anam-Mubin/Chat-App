import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable()
export class ChatService{

    private openedChatType : string;

    constructor(private router: Router) {
        this.openedChatType = 'UserChat';
    }

    openChat(data: any) {
        this.openedChatType = data.type;
        this.router.navigateByUrl('chat/' + data.id);
    }

    getOpenedChatType() : string{
        return this.openedChatType;
    }
}