export type ImportRequestSetting = {
    importServiceLogTypes: boolean;
    importServiceLogDescriptions: boolean;
    credentials: ImportRequestCredentials;
};

export type ImportRequestCredentials = {
    drkServerLogin: string;
    drkServerPassword: string;
};
