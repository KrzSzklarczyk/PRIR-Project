import { UserType } from "./UserRole";
export interface UserResponseDTO {
    userId: number;
    email: string;
    nickName: string;
    avatar: string;
    credits: number;
    userType: UserType;
}