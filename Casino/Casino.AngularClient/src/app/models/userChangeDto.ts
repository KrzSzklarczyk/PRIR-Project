import { AuthenticatedResponse } from "./authenticated-response";

export interface UserChangeDTO {
    token: AuthenticatedResponse;
    cos: string;
}