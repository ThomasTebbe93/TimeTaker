import { Theme, createStyles, makeStyles } from "@material-ui/core/styles";
import Page from "../../../components/layout/Page";
import ServiceLogDescriptionTable from "./ServiceLogDescriptionTable";
import { useTranslation } from "react-i18next";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        paper: { padding: 10 },
    })
);

export default function ServiceLogDescriptionPage() {
    const { t } = useTranslation();
    const classes = useStyles();

    return (
        <Page title={t("common.serviceLogDescriptions")}>
            <div className={classes.paper}>
                <ServiceLogDescriptionTable
                    refreshCount={0}
                    key="serviceLogDescriptionTable"
                />
            </div>
        </Page>
    );
}
