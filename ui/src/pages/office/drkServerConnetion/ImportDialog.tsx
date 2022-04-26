import {
    Button,
    Checkbox,
    CircularProgress,
    createStyles,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Divider,
    FormControlLabel,
    makeStyles,
    TextField,
    Theme,
    useMediaQuery,
    useTheme,
} from "@material-ui/core";

import { List } from "immutable";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import FormInput from "../../../components/layout/formpage/FormInput";
import FormSection from "../../../components/layout/formpage/FormSection";

import { Rights } from "../../../helpers/rights";
import { authenticationService } from "../../../services/authenticationService";
import { requestService } from "../../../services/requestService";
import { ImportRequestSetting } from "../../../types/importRequestSetting";
import {
    RequestResult,
    StatusCode,
    ValidationFailure,
} from "../../../types/requestResult";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        header: {
            display: "flex",
            alignItems: "center",
        },
        content: {
            padding: "2px 24px 24px 24px",
        },
        dialogTitiel: {
            borderBottomStyle: "solid",
            borderBottomColor: theme.palette.text.primary,
            borderBottomWidth: 1,
            height: 32,
        },
        dialog: {
            padding: "16px 24px 10px 24px",
        },
        input: {
            color: theme.palette.getContrastText(
                theme.palette.background.default
            ),
        },
        formControl: {
            margin: theme.spacing(1),
            minWidth: 300,
            display: "flex",
        },
        overlay: {
            position: "absolute",
            width: "100%",
            height: "100%",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            zIndex: 1,
            backgroundColor: "#eeeeee70",
        },
    })
);

interface FormState {
    isImporting: boolean;
    importRequestSetting: ImportRequestSetting | null;
    validationFailures: List<ValidationFailure> | null;
    hasInternalError: boolean;
}

interface ImportDialogProps {
    isOpen: boolean;
    close: () => void;
}

export default function ImportDialog({ isOpen, close }: ImportDialogProps) {
    const { t } = useTranslation();
    const classes = useStyles();
    const theme = useTheme();
    const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));
    const currentUser = authenticationService.currentUserValue;
    const canImport =
        currentUser?.rights?.some((x) => x === Rights.ADMINISTRATION_IMPORT) ??
        false;

    const [state, setState] = useState<FormState>({
        isImporting: false,
        importRequestSetting: null,
        validationFailures: null,
        hasInternalError: false,
    });

    const onClose = () => {
        setState({
            ...state,
            isImporting: false,
            importRequestSetting: null,
            validationFailures: null,
            hasInternalError: false,
        });
        close();
    };

    const save = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.stopPropagation();
        e.preventDefault();
        setState({ ...state, isImporting: true });
        requestService
            .post<RequestResult>(`/drkServerConnector/import`, {
                ...state.importRequestSetting,
            })
            .then((res) => {
                if (res.statusCode === StatusCode.validationError)
                    setState({
                        ...state,
                        isImporting: false,
                        validationFailures: res.validationFailures,
                    });
                if (res.statusCode === StatusCode.internalServerError)
                    setState({
                        ...state,
                        isImporting: false,
                        hasInternalError: true,
                    });
                if (res.statusCode === StatusCode.ok) {
                    onClose();
                }
            });
    };

    const onChangeLogin = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newState = {
            ...state,
            importRequestSetting: {
                ...state.importRequestSetting,
                credentials: {
                    ...state.importRequestSetting?.credentials,
                    drkServerLogin: e.target.value,
                },
            },
        } as FormState;
        setState(newState);
    };

    const onChangePassword = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newState = {
            ...state,
            importRequestSetting: {
                ...state.importRequestSetting,
                credentials: {
                    ...state.importRequestSetting?.credentials,
                    drkServerPassword: e.target.value,
                },
            },
        } as FormState;
        setState(newState);
    };

    const onChangeImportServiceLogDescriptions = (
        e: React.MouseEvent<HTMLButtonElement>
    ) => {
        const newState = {
            ...state,
            importRequestSetting: {
                ...state.importRequestSetting,
                importServiceLogDescriptions:
                    !state.importRequestSetting?.importServiceLogDescriptions,
            },
        } as FormState;
        setState(newState);
    };

    const onChangeImportServiceLogTypes = (
        e: React.MouseEvent<HTMLButtonElement>
    ) => {
        const newState = {
            ...state,
            importRequestSetting: {
                ...state.importRequestSetting,
                importServiceLogTypes:
                    !state.importRequestSetting?.importServiceLogTypes,
            },
        } as FormState;
        setState(newState);
    };

    return (
        <Dialog
            fullScreen={fullScreen}
            open={isOpen}
            onClose={onClose}
            aria-labelledby="form-dialog-title"
            className={!fullScreen ? classes.dialog : undefined}
        >
            {state.isImporting && (
                <div className={classes.overlay}>
                    <CircularProgress />
                </div>
            )}
            <DialogTitle id="form-dialog-title">
                {t("common.importFromDrkServer")}
            </DialogTitle>
            <DialogContent className={classes.content}>
                <FormControlLabel
                    control={
                        <Checkbox
                            id={"importServiceLogTypes"}
                            checked={
                                state.importRequestSetting
                                    ?.importServiceLogTypes
                            }
                            onClick={onChangeImportServiceLogTypes}
                            name={t("common.importServiceLogTypes")}
                            color="primary"
                        />
                    }
                    label={t("common.importServiceLogTypes")}
                />
                <FormControlLabel
                    control={
                        <Checkbox
                            id={"importServiceLogDescriptions"}
                            checked={
                                state.importRequestSetting
                                    ?.importServiceLogDescriptions
                            }
                            onClick={onChangeImportServiceLogDescriptions}
                            name={t("common.importServiceLogDescriptions")}
                            color="primary"
                        />
                    }
                    label={t("common.importServiceLogDescriptions")}
                />
                <Divider />
                <FormSection>
                    <FormInput label={t("common.loginDrkServer")}>
                        <TextField
                            InputProps={{
                                classes: {
                                    input: classes.input,
                                },
                            }}
                            autoComplete="off"
                            error={state.validationFailures?.some(
                                (failure: ValidationFailure) =>
                                    failure.propertyName === "DrkServerLogin"
                            )}
                            helperText={t(
                                state.validationFailures?.find(
                                    (failure: ValidationFailure) =>
                                        failure.propertyName ===
                                        "DrkServerLogin"
                                )?.errorMessage ?? ""
                            )}
                            id="drkServerLogin"
                            className={classes.formControl}
                            value={
                                state.importRequestSetting?.credentials
                                    ?.drkServerLogin ?? ""
                            }
                            onChange={onChangeLogin}
                        />
                    </FormInput>
                    <FormInput label={t("common.passwordDrkServer")}>
                        <TextField
                            InputProps={{
                                classes: {
                                    input: classes.input,
                                },
                            }}
                            autoComplete="off"
                            error={state.validationFailures?.some(
                                (failure: ValidationFailure) =>
                                    failure.propertyName === "DrkServerPassword"
                            )}
                            helperText={t(
                                state.validationFailures?.find(
                                    (failure: ValidationFailure) =>
                                        failure.propertyName ===
                                        "DrkServerPassword"
                                )?.errorMessage ?? ""
                            )}
                            id="drkServerPassword"
                            className={classes.formControl}
                            value={
                                state.importRequestSetting?.credentials
                                    ?.drkServerPassword ?? ""
                            }
                            onChange={onChangePassword}
                        />
                    </FormInput>
                </FormSection>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="primary">
                    {t("action.close")}
                </Button>
                {canImport && (
                    <Button onClick={save} color="primary">
                        {t("action.doImport")}
                    </Button>
                )}
            </DialogActions>
        </Dialog>
    );
}
