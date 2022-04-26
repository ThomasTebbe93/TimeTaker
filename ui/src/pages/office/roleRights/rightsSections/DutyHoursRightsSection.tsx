import React from "react";
import { FormControlLabel, FormGroup, useTheme } from "@material-ui/core";
import { useTranslation } from "react-i18next";
import { List } from "immutable";
import RightCheckbox from "../../../../components/inputs/RightCheckbox";
import { Rights } from "../../../../helpers/rights";

interface Props {
    rights: List<string>;
    handleChange: (
        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
        rightKey: string,
        value: boolean
    ) => void;
    disabled: boolean;
}

export default function DutyHoursRightsSection(props: Props) {
    const { t } = useTranslation();
    const theme = useTheme();
    const { rights, handleChange, disabled } = props;

    return (
        <div>
            <FormControlLabel
                control={
                    <RightCheckbox
                        disabled={disabled}
                        handleChange={handleChange}
                        rigtKey={Rights.DUTY_HOURS}
                        value={rights.some((x) => x === Rights.DUTY_HOURS)}
                    />
                }
                label={t(`rights.${Rights.DUTY_HOURS}`)}
            />
            {rights.some((x) => x === Rights.DUTY_HOURS) && (
                <div style={{ marginLeft: 50 }}>
                    <FormGroup
                        aria-label="position"
                        style={{
                            background: theme.palette.background.paper,
                            padding: "0px 10px 10px 10px",
                            borderRadius: 4,
                        }}
                    >
                        <FormControlLabel
                            control={
                                <RightCheckbox
                                    disabled={disabled}
                                    handleChange={handleChange}
                                    rigtKey={Rights.DUTY_HOURS_DISPLAY_ALL}
                                    value={rights.some(
                                        (x) =>
                                            x === Rights.DUTY_HOURS_DISPLAY_ALL
                                    )}
                                />
                            }
                            label={t(`rights.${Rights.DUTY_HOURS_DISPLAY_ALL}`)}
                        />
                        <FormControlLabel
                            control={
                                <RightCheckbox
                                    disabled={disabled}
                                    handleChange={handleChange}
                                    rigtKey={Rights.DUTY_HOURS_DISPLAY_SELF}
                                    value={rights.some(
                                        (x) =>
                                            x === Rights.DUTY_HOURS_DISPLAY_SELF
                                    )}
                                />
                            }
                            label={t(
                                `rights.${Rights.DUTY_HOURS_DISPLAY_SELF}`
                            )}
                        />
                        <FormControlLabel
                            control={
                                <RightCheckbox
                                    disabled={disabled}
                                    handleChange={handleChange}
                                    rigtKey={Rights.DUTY_HOURS_BOOKING_CREATE}
                                    value={rights.some(
                                        (x) =>
                                            x ===
                                            Rights.DUTY_HOURS_BOOKING_CREATE
                                    )}
                                />
                            }
                            label={t(
                                `rights.${Rights.DUTY_HOURS_BOOKING_CREATE}`
                            )}
                        />
                        <FormControlLabel
                            control={
                                <RightCheckbox
                                    disabled={disabled}
                                    handleChange={handleChange}
                                    rigtKey={Rights.DUTY_HOURS_BOOKING_EDIT}
                                    value={rights.some(
                                        (x) =>
                                            x === Rights.DUTY_HOURS_BOOKING_EDIT
                                    )}
                                />
                            }
                            label={t(
                                `rights.${Rights.DUTY_HOURS_BOOKING_EDIT}`
                            )}
                        />
                        <FormControlLabel
                            control={
                                <RightCheckbox
                                    disabled={disabled}
                                    handleChange={handleChange}
                                    rigtKey={
                                        Rights.DUTY_HOURS_BOOKING_GET_USER_BY_CHIPID
                                    }
                                    value={rights.some(
                                        (x) =>
                                            x ===
                                            Rights.DUTY_HOURS_BOOKING_GET_USER_BY_CHIPID
                                    )}
                                />
                            }
                            label={t(
                                `rights.${Rights.DUTY_HOURS_BOOKING_GET_USER_BY_CHIPID}`
                            )}
                        />
                    </FormGroup>
                </div>
            )}
        </div>
    );
}
