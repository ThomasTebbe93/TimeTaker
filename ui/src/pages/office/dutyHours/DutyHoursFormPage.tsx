import React, { useEffect, useState } from "react";
import {
    makeStyles,
    Theme,
    createStyles,
    Fab,
    IconButton,
} from "@material-ui/core";
import CloseIcon from "@material-ui/icons/Close";
import SaveIcon from "@material-ui/icons/Save";

import { useTranslation } from "react-i18next";

import { List } from "immutable";

import { DateTimePicker, MuiPickersUtilsProvider } from "@material-ui/pickers";
import DateFnsUtils from "@date-io/date-fns";
import deLocale from "date-fns/locale/de";
import { DutyHour } from "../../../types/dutyHour";
import { Rights } from "../../../helpers/rights";
import { authenticationService } from "../../../services/authenticationService";
import { requestService } from "../../../services/requestService";
import {
    RequestResult,
    StatusCode,
    ValidationFailure,
} from "../../../types/requestResult";
import SlideIn from "../../../components/layout/SlideIn";
import LoadingBars from "../../../components/loadingAnimations/LoadingBars";
import FormSection from "../../../components/layout/formpage/FormSection";
import FormInput from "../../../components/layout/formpage/FormInput";
import { differenceInSeconds } from "date-fns/esm";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        slideInHeader: {
            padding: 10,
            borderBottomStyle: "solid",
            borderBottomColor: theme.palette.text.primary,
            borderBottomWidth: 1,
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
        },
        slideInHeaderLeft: {
            display: "flex",
            alignItems: "center",
        },
        slideInHeaderTitle: {
            fontSize: 22,
            marginLeft: 10,
        },
        slideInBody: {
            padding: 10,
        },
        extendedIcon: {
            marginRight: theme.spacing(1),
        },
        formControl: {
            margin: theme.spacing(1),
            minWidth: 300,
            display: "flex",
        },
        dutyHourSelectIcon: {
            height: 39,
            margin: "-10px 10px -10px 0px",
        },
    })
);

interface Props {
    slideInIsOpen: boolean;
    closeSlideIn: () => void;
    selectedDutyHour: DutyHour | null;
    refresh: () => void;
}

export default function DutyHoursFormPage(props: Props) {
    const currentUser = authenticationService.currentUserValue;
    const canEdit =
        currentUser?.rights?.some(
            (x) => x === Rights.DUTY_HOURS_BOOKING_EDIT
        ) ?? false;
    const { t } = useTranslation();
    const classes = useStyles();
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [dutyHour, setDutyHour] = useState<DutyHour | null>();
    const [validationFailures, setValidationFailures] =
        useState<List<ValidationFailure> | null>(null);
    const [hasInternalError, setHasInternalError] = useState<boolean>(false);

    useEffect(() => {
        if (
            props.selectedDutyHour !== undefined &&
            props.selectedDutyHour !== null
        ) {
            requestService
                .get<DutyHour>(
                    `/dutyhours/getByIdent/${props.selectedDutyHour?.ident?.ident}`
                )
                .then((res) => {
                    setDutyHour(res);
                    setIsLoading(false);
                });
        }
        if (!props.selectedDutyHour) {
            setIsLoading(false);
        }
    }, [props.selectedDutyHour]);

    const onCloseSlideIn = (
        event: React.MouseEvent<HTMLButtonElement, MouseEvent>
    ) => {
        event.stopPropagation();
        event.preventDefault();
        onClose();
    };
    const onClose = () => {
        setDutyHour(null);
        setValidationFailures(null);
        props.closeSlideIn();
    };

    const onSave = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        event.stopPropagation();
        event.preventDefault();
        requestService
            .post<RequestResult>(`/dutyhours/update`, [
                {
                    ident: dutyHour?.ident?.ident,
                    start: dutyHour?.signInBooking.bookingTime,
                    end: dutyHour?.signOutBooking.bookingTime,
                },
            ])
            .then((res) => {
                if (res.statusCode === StatusCode.validationError)
                    setValidationFailures(res.validationFailures);
                if (res.statusCode === StatusCode.internalServerError)
                    setHasInternalError(true);
                if (res.statusCode === StatusCode.ok) {
                    props.refresh();
                    setDutyHour(null);
                    setValidationFailures(null);
                    setHasInternalError(false);
                    props.closeSlideIn();
                }
            });
    };

    const onChangeStartDateTime = (date: Date | null) => {
        const newDutyHour = {
            ...dutyHour,
            signInBooking: { ...dutyHour?.signInBooking, bookingTime: date },
        } as DutyHour;
        setDutyHour(newDutyHour);
    };

    const onChangeEndtDateTime = (date: Date | null) => {
        const newDutyHour = {
            ...dutyHour,
            signOutBooking: { ...dutyHour?.signOutBooking, bookingTime: date },
        } as DutyHour;
        setDutyHour(newDutyHour);
    };

    return (
        <SlideIn slideInIsOpen={props.slideInIsOpen} onCloseSlidein={onClose}>
            {isLoading && <LoadingBars />}
            {!isLoading && (
                <>
                    <div className={classes.slideInHeader}>
                        <div className={classes.slideInHeaderLeft}>
                            <IconButton
                                color="secondary"
                                aria-label="add"
                                size="small"
                                onClick={onCloseSlideIn}
                            >
                                <CloseIcon />
                            </IconButton>
                            <span className={classes.slideInHeaderTitle}>
                                {dutyHour && dutyHour.ident?.ident
                                    ? t("action.updateUserDutyHours", {
                                          userFirstname:
                                              dutyHour.signInBooking.user
                                                  .firstName,
                                          userLastname:
                                              dutyHour.signInBooking.user
                                                  .lastName,
                                      })
                                    : t("action.createSomething", {
                                          something: t("common.dutyHour"),
                                      })}
                            </span>
                        </div>
                        {canEdit && (
                            <Fab
                                color="primary"
                                variant="extended"
                                size="small"
                                onClick={onSave}
                            >
                                <SaveIcon className={classes.extendedIcon} />
                                {t("action.save")}
                            </Fab>
                        )}
                    </div>
                    <div className={classes.slideInBody}>
                        <FormSection>
                            <FormInput label={t("common.beginning")}>
                                <MuiPickersUtilsProvider
                                    utils={DateFnsUtils}
                                    locale={deLocale}
                                >
                                    <DateTimePicker
                                        ampm={false}
                                        minutesStep={5}
                                        cancelLabel={t("common.cancelLabel")}
                                        okLabel={t("common.ok")}
                                        disabled={!canEdit}
                                        style={{ paddingLeft: 8 }}
                                        format="dd.MM.yyyy HH:mm"
                                        margin="normal"
                                        id="date-picker-inline"
                                        openTo="year"
                                        views={[
                                            "year",
                                            "month",
                                            "date",
                                            "hours",
                                            "minutes",
                                        ]}
                                        value={
                                            dutyHour?.signInBooking
                                                .bookingTime ?? null
                                        }
                                        maxDate={
                                            dutyHour?.signOutBooking
                                                .bookingTime ?? undefined
                                        }
                                        onChange={onChangeStartDateTime}
                                    />
                                </MuiPickersUtilsProvider>
                            </FormInput>
                            <FormInput label={t("common.end")}>
                                <MuiPickersUtilsProvider
                                    utils={DateFnsUtils}
                                    locale={deLocale}
                                >
                                    <DateTimePicker
                                        ampm={false}
                                        minutesStep={5}
                                        cancelLabel={t("common.cancelLabel")}
                                        okLabel={t("common.ok")}
                                        disabled={!canEdit}
                                        style={{ paddingLeft: 8 }}
                                        format="dd.MM.yyyy HH:mm"
                                        margin="normal"
                                        id="date-picker-inline"
                                        openTo="year"
                                        views={[
                                            "year",
                                            "month",
                                            "date",
                                            "hours",
                                            "minutes",
                                        ]}
                                        value={
                                            dutyHour?.signOutBooking
                                                .bookingTime ?? null
                                        }
                                        minDate={
                                            dutyHour?.signInBooking
                                                .bookingTime ?? undefined
                                        }
                                        onChange={onChangeEndtDateTime}
                                    />
                                </MuiPickersUtilsProvider>
                            </FormInput>
                            <div style={{ height: 20, width: "100%" }}></div>
                            <FormInput label={t("common.duration")}>
                                <div style={{ paddingLeft: 8 }}>
                                    {!!dutyHour?.signInBooking.bookingTime &&
                                    !!dutyHour?.signOutBooking.bookingTime
                                        ? `${(
                                              differenceInSeconds(
                                                  new Date(
                                                      dutyHour?.signOutBooking.bookingTime
                                                  ),
                                                  new Date(
                                                      dutyHour?.signInBooking.bookingTime
                                                  )
                                              ) /
                                              60 /
                                              60
                                          ).toFixed(2)} ${t("common.hours")}`
                                        : ""}
                                </div>
                            </FormInput>
                        </FormSection>
                    </div>
                </>
            )}
        </SlideIn>
    );
}
