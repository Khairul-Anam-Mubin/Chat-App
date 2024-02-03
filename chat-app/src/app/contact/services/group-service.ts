import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { GroupsQuery } from "../queries/groups-query";
import { QueryService } from "src/app/core/services/query-service";
import { AddMemberToGroupCommand } from "../commands/add-memeber-to-group-command";
import { CommandService } from "src/app/core/services/command-service";
import { GroupMembersQuery } from "../queries/group-member-query";

@Injectable()
export class GroupService{
    private currentGroup: any;

    constructor(
        private router: Router,
        private queryService: QueryService,
        private commandService: CommandService) {
        
    }

    openGroupChat(group: any) : void {
        this.currentGroup = group;
        this.router.navigateByUrl('group/chat/' + this.currentGroup.id);
    }
    
    openGroup(group: any) {
        this.router.navigateByUrl('group/' + group.id);
    }

    getCurrentOpenedGroupId() : string | undefined{
        if (this.router.url.includes('group/chat')) {
            let splittedUrl = this.router.url.split('/');
            if (splittedUrl.length === 0) {
                splittedUrl = this.router.url.split('\\');
            }
            if (splittedUrl.length === 0) return '';
            splittedUrl = splittedUrl.reverse();
            return splittedUrl[0];
        } 
        if (this.router.url.includes('group/')) {
            let splittedUrl = this.router.url.split('/');
            if (splittedUrl.length === 0) {
                splittedUrl = this.router.url.split('\\');
            }
            if (splittedUrl.length === 0) return '';
            splittedUrl = splittedUrl.reverse();
            return splittedUrl[0];
        }
        return undefined;
    }

    getGroupsByIds(groupIds: string[]) {
        const groupQuery = new GroupsQuery();
        groupQuery.groupIds = groupIds;
        return this.queryService.execute(groupQuery);
    }

    addMemberToGroup(groupid: any, memberId: any, addedBy: any) {
        const command = new AddMemberToGroupCommand();
        command.addedBy = addedBy;
        command.groupId = groupid;
        command.memberId = memberId;
        return this.commandService.execute(command);
    }

    getGroupMembers(groupId: string) {
        const query = new GroupMembersQuery();
        query.groupId = groupId;
        return this.queryService.execute(query);
    }
}