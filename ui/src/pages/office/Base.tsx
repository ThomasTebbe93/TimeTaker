import { Context, useEffect, useState } from "react";
import PrivateRoute from "../../components/PrivateRoute";
import Dashboard from "./contentPages/Dashboard";
import MenuAppBar from "../../components/layout/MenuAppBar";
import SideBar from "../../components/layout/SideBar";
import UserPage from "./memberchip/UserPage";
import RolePage from "./roleRights/RolePage";
import {
    makeStyles,
    Theme,
    createStyles,
    useMediaQuery,
    useTheme,
} from "@material-ui/core";
import { Rights } from "../../helpers/rights";
import DutyHoursPage from "./dutyHours/DutyHoursPage";
import ServiceLogTypePage from "./serviceLogTypes/ServiceLogTypePage";
import ServiceLogDescriptionPage from "./serviceLogDescriptions/ServiceLogDescriptionPage";
import Administration from "./contentPages/Administration";

interface BaseProps {
    colorModeContext: Context<{
        toggleColorMode: () => void;
    }>;
}

export default function Base(props: BaseProps) {
    const theme = useTheme();
    const useMobile = useMediaQuery(theme.breakpoints.down("sm")) ?? true;
    const [isSidebarCollaped, setIsSidebarCollaped] =
        useState<boolean>(useMobile);
    const changeCollaption = () => setIsSidebarCollaped(!isSidebarCollaped);

    useEffect(() => {
        setIsSidebarCollaped(useMobile);
    }, [useMobile]);

    const useStyles = (useMobile: boolean) =>
        makeStyles((theme: Theme) =>
            createStyles({
                section: {
                    border: 0,
                    position: "absolute",
                    backgroundColor: theme.palette.background.default,
                    color: theme.palette.getContrastText(
                        theme.palette.background.default
                    ),
                    top: 0,
                    bottom: 0,
                    left: 0,
                    right: 0,
                    overflow: "hidden",
                    marginLeft: isSidebarCollaped || useMobile ? 32 : 250,
                    marginTop: useMobile ? 40 : 56,
                    transition: "margin-left 300ms",
                },
                "@global": {
                    "*::-webkit-scrollbar": {
                        width: "0em",
                    },
                    "*::-webkit-scrollbar-track": {
                        "-webkit-box-shadow": "inset 0 0 6px rgba(0,0,0,0.00)",
                    },
                    "*::-webkit-scrollbar-thumb": {
                        backgroundColor: "rgba(0,0,0,.1)",
                    },
                },
            })
        )();
    const classes = useStyles(useMobile);
    return (
        <div style={{ height: "100%" }}>
            <MenuAppBar
                useMobile={useMobile}
                changeSidebarCollaption={changeCollaption}
                isSidebarCollaped={isSidebarCollaped}
                colorModeContext={props.colorModeContext}
            />
            <SideBar
                isSidebarCollaped={isSidebarCollaped}
                changeSidebarCollaption={changeCollaption}
                useMobile={useMobile}
            />
            <div className={classes.section}>
                <PrivateRoute
                    key="/dashboard"
                    path="/dashboard"
                    component={Dashboard}
                />
                <PrivateRoute
                    key="/dutyhours"
                    path="/dutyhours"
                    right={Rights.DUTY_HOURS}
                    component={DutyHoursPage}
                ></PrivateRoute>
                <PrivateRoute
                    key="/administrationPage"
                    path="/administrationPage"
                    component={Administration}
                    right={Rights.ADMINISTRATION}
                />
                <PrivateRoute
                    key="/administration/users"
                    path="/administration/users"
                    component={UserPage}
                    right={Rights.ADMINISTRATION_USERS}
                />
                <PrivateRoute
                    key="/administration/serviceLogTypes"
                    path="/administration/serviceLogTypes"
                    component={ServiceLogTypePage}
                    right={Rights.ADMINISTRATION}
                />
                <PrivateRoute
                    key="/administration/serviceLogDescriptions"
                    path="/administration/serviceLogDescriptions"
                    component={ServiceLogDescriptionPage}
                    right={Rights.ADMINISTRATION}
                />
                <PrivateRoute
                    key="/administration/roles"
                    path="/administration/roles"
                    component={RolePage}
                    right={Rights.ADMINISTRATION_ROLES}
                />
            </div>
        </div>
    );
}
