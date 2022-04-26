import { List } from "immutable";

export enum Rights {
    DASHBOARD = "dashboard",

    ADMINISTRATION = "administration",
    ADMINISTRATION_USERS = "administrationUsers",
    ADMINISTRATION_USERS_CREATE = "administrationUsersCreate",
    ADMINISTRATION_USERS_EDIT = "administrationUsersEdit",
    ADMINISTRATION_USERS_DELETE = "administrationUsersDelete",
    ADMINISTRATION_FILES = "administrationFiles",
    ADMINISTRATION_FILES_EDIT = "administrationFilesEdit",
    ADMINISTRATION_SETTINGS = "administrationSettings",
    ADMINISTRATION_SETTINGS_EDIT = "administrationSettingsEdit",
    ADMINISTRATION_ROLES = "administrationRoles",
    ADMINISTRATION_ROLES_CREATE = "administrationRolesCreate",
    ADMINISTRATION_ROLES_EDIT = "administrationRolesEdit",
    ADMINISTRATION_ROLES_DELETE = "administrationRolesDelete",
    ADMINISTRATION_IMPORT = "administrationImport",

    DUTY_HOURS = "dutyHours",
    DUTY_HOURS_DISPLAY_ALL = "dutyHoursDisplayAll",
    DUTY_HOURS_DISPLAY_SELF = "dutyHoursDisplaySelf",
    DUTY_HOURS_BOOKING_CREATE = "dutyHoursCreateBooking",
    DUTY_HOURS_BOOKING_EDIT = "dutyHoursEditBooking",
    DUTY_HOURS_BOOKING_GET_USER_BY_CHIPID = "dutyHoursGetUserByChipId",
}

export const DutyHoursRights = List([
    Rights.DUTY_HOURS,
    Rights.DUTY_HOURS_DISPLAY_ALL,
    Rights.DUTY_HOURS_DISPLAY_SELF,
    Rights.DUTY_HOURS_BOOKING_CREATE,
    Rights.DUTY_HOURS_BOOKING_EDIT,
    Rights.DUTY_HOURS_BOOKING_GET_USER_BY_CHIPID,
]);

export const AdministrationUserRights = List([
    Rights.ADMINISTRATION_USERS,
    Rights.ADMINISTRATION_USERS_CREATE,
    Rights.ADMINISTRATION_USERS_EDIT,
    Rights.ADMINISTRATION_USERS_DELETE,
]);
export const AdministrationFilesRights = List([
    Rights.ADMINISTRATION_FILES,
    Rights.ADMINISTRATION_FILES_EDIT,
]);
export const AdministrationSettingsRights = List([
    Rights.ADMINISTRATION_SETTINGS,
    Rights.ADMINISTRATION_SETTINGS_EDIT,
]);
export const AdministrationRolesRights = List([
    Rights.ADMINISTRATION_ROLES,
    Rights.ADMINISTRATION_ROLES_CREATE,
    Rights.ADMINISTRATION_ROLES_EDIT,
    Rights.ADMINISTRATION_ROLES_DELETE,
]);

export const AdministrationRights = List()
    .concat(Rights.ADMINISTRATION)
    .concat(Rights.ADMINISTRATION_IMPORT)
    .concat(AdministrationUserRights)
    .concat(AdministrationFilesRights)
    .concat(AdministrationSettingsRights)
    .concat(AdministrationRolesRights);

export const AllRights = List()
    .concat(DutyHoursRights)
    .concat(AdministrationRights);
