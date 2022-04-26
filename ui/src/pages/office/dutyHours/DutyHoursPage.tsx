import { useState } from "react";
import { Theme, createStyles, makeStyles } from "@material-ui/core/styles";
import Page from "../../../components/layout/Page";
import { DutyHour } from "../../../types/dutyHour";
import { useTranslation } from "react-i18next";
import DutyHoursTable from "./DutyHoursTable";
import DutyHoursFormPage from "./DutyHoursFormPage";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        paper: { padding: 10 },
    })
);

export default function DutyHoursPage() {
    const { t } = useTranslation();
    const classes = useStyles();
    const [refreshCount, setRefreshCount] = useState<number>(0);
    const [slideInIsOpen, setSlideInIsOpen] = useState<boolean>(false);
    const [selectedDutyHour, setSelectedDutyHour] = useState<DutyHour | null>(
        null
    );

    const closeSlideIn = () => {
        refresh();
        setSelectedDutyHour(null);
        setSlideInIsOpen(false);
    };

    const onRowClick = (data: DutyHour) => {
        setSelectedDutyHour(data);
        setSlideInIsOpen(true);
    };

    const refresh = () => setRefreshCount(refreshCount + 1);

    return (
        <Page
            title={t("common.dutyHours")}
            slideInContend={
                <DutyHoursFormPage
                    slideInIsOpen={slideInIsOpen}
                    closeSlideIn={closeSlideIn}
                    selectedDutyHour={selectedDutyHour}
                    refresh={refresh}
                />
            }
        >
            <div className={classes.paper}>
                <DutyHoursTable
                    onRowClick={onRowClick}
                    onDeleteClick={(data: any) => {}}
                    refreshCount={refreshCount}
                />
            </div>
        </Page>
    );
}
