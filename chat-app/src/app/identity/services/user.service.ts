import { Injectable, OnInit } from "@angular/core";
import { UserProfile } from "../models/user-profile";
import { Router } from "@angular/router";
import { UserProfileQuery } from "../queries/user-profile-query";
import { QueryService } from "src/app/core/services/query-service";
import { take } from "rxjs";
import { ResponseStatus } from "src/app/core/constants/response-status";

@Injectable({
    providedIn: 'root',
})
export class UserService{

    constructor (
        private router : Router,
        private queryService: QueryService) {}

    getCurrentUserProfile() {
        const user = localStorage.getItem("userProfile");
        let userProfile = new UserProfile();
        if (user === null) return userProfile;
        userProfile = JSON.parse(user);
        return userProfile;
    }

    getCurrentUserId() {
        return this.getCurrentUserProfile()?.id;
    }

    getCurrentOpenedChatUserId() {
        if (this.router.url.includes('chat')) {
            let splittedUrl = this.router.url.split('/');
            if (splittedUrl.length === 0) {
                splittedUrl = this.router.url.split('\\');
            }
            if (splittedUrl.length === 0) return '';
            splittedUrl = splittedUrl.reverse();
            return splittedUrl[0];
        }
        return '';
    }

    getCurrentOpenedProfileUserId() {
        if (this.router.url.includes('user')) {
            let splittedUrl = this.router.url.split('/');
            if (splittedUrl.length === 0) {
                splittedUrl = this.router.url.split('\\');
            }
            if (splittedUrl.length === 0) return '';
            splittedUrl = splittedUrl.reverse();
            return splittedUrl[0];
        }
        return '';
    }

    setUserProfileToStore(userProfile: any) {
        localStorage.setItem('userProfile', JSON.stringify(userProfile));
    }

    getUserProfileByEmail(email: any) {
        var userProfileQuery = new UserProfileQuery();
        userProfileQuery.emails = [email];
        return this.queryService.execute(userProfileQuery);
    }

    getUserProfileById(userId : any) {
        var userProfileQuery = new UserProfileQuery();
        userProfileQuery.userIds = [userId];
        return this.queryService.execute(userProfileQuery);
    }

    getUserProfilesByIds(userIds : any) {
        const userProfileQuery = new UserProfileQuery();
        userProfileQuery.userIds = userIds;
        return this.queryService.execute(userProfileQuery);
    }
}
