import { Theme, createStyles, makeStyles } from "@material-ui/core/styles";
import Page from "../../../components/layout/Page";
import ServiceLogTypeTable from "./ServiceLogTypeTable";
import { useTranslation } from "react-i18next";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        paper: { padding: 10 },
    })
);

export default function ServiceLogTypePage() {
    const { t } = useTranslation();
    const classes = useStyles();

    return (
        <Page title={t("common.serviceLogTypes")}>
            <div className={classes.paper}>
                <ServiceLogTypeTable
                    refreshCount={0}
                    key="serviceLogTypeTable"
                />
            </div>
        </Page>
    );
}
