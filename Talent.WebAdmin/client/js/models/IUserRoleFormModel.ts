import { PositionDropdownModel, CategoryDropdownModel } from "../services/NSwagService";

export interface IUserRoleFormModel {
    userRoleName: string,
    roleDescription: string,
    typeOfPeople: boolean,
    position: PositionDropdownModel,
    dealerCategory: CategoryDropdownModel
}
