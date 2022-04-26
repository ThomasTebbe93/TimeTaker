import {
    Button,
    createStyles,
    makeStyles,
    Paper,
    Theme,
} from "@material-ui/core";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import Page from "../../../components/layout/Page";
import { Rights } from "../../../helpers/rights";
import { authenticationService } from "../../../services/authenticationService";
import ImportDialog from "../drkServerConnetion/ImportDialog";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            display: "flex",
            flexWrap: "wrap",
            overflowX: "hidden",
            justifyContent: "center",
            alignItems: "space-between",
        },
        header: {
            padding: 10,
            backgroundColor: theme.palette.background.default,
            borderBottomStyle: "solid",
            borderBottomColor: theme.palette.text.primary,
            borderBottomWidth: 2,
            justifyContent: "space-between",
            display: "flex",
        },
        paper: {
            width: 400,
            height: 275,
            backgroundColor: theme.palette.background.default,
            marginBottom: 10,
            position: "relative",
        },
        body: {
            padding: 12,
            fontWeight: 600,
            color: "#aaa",
        },
        edge: {
            padding: 12,
            fontWeight: 600,
            color: "#aaa",
            position: "absolute",
            bottom: 0,
            right: 0,
        },
    })
);

export default function Administration() {
    const { t } = useTranslation();
    const classes = useStyles();
    const [isDialogOpened, setIsDialogOpened] = useState<boolean>(false);
    const currentUser = authenticationService.currentUserValue;
    const canImport =
        currentUser?.rights?.some((x) => x === Rights.ADMINISTRATION_IMPORT) ??
        false;

    const openImport = (
        event: React.MouseEvent<HTMLButtonElement, MouseEvent>
    ) => setIsDialogOpened(true);
    const closeImport = () => setIsDialogOpened(false);

    return (
        <Page title={t("common.administration")}>
            <div className={classes.root}>
                {canImport && (
                    <Paper elevation={3} className={classes.paper}>
                        <div className={classes.header}>
                            <span style={{ fontSize: 22 }}>
                                {t("common.importDrkServer")}
                            </span>
                        </div>
                        <div className={classes.body}>
                            {t("common.importDrkServerExplain")}
                        </div>
                        <div className={classes.edge}>
                            <Button
                                variant="outlined"
                                color="primary"
                                onClick={openImport}
                            >
                                {t("action.doImport")}
                            </Button>
                        </div>
                    </Paper>
                )}
            </div>
            {canImport && (
                <ImportDialog isOpen={isDialogOpened} close={closeImport} />
            )}
        </Page>
    );
}
