import { CommandBase } from "src/app/core/models/command-base";
import { Configuration } from "src/app/core/services/configuration";

export class RefreshTokenCommand extends CommandBase{

    appId : string | undefined;
    accessToken: string | undefined | null;
    refreshToken: string | undefined | null;

    constructor() {
        super();
        this.apiUrl = Configuration.identityApi.concat("/Auth/refresh-token");
    }
}