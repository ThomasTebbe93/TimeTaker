import { List } from "immutable";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { TableCellCustom } from "../../../components/tables/TableCellCustom";
import StickyHeadTable, {
    TableColumn,
    TableSearch,
} from "../../../components/tables/Table";
import { authenticationService } from "../../../services/authenticationService";
import { requestService } from "../../../services/requestService";
import { format } from "date-fns";
import { DutyHour } from "../../../types/dutyHour";
import { User } from "../../../types/user";
import { differenceInMilliseconds } from "date-fns/esm";
import { Rights } from "../../../helpers/rights";

enum ColumnId {
    date = "date",
    user = "user",
    startTime = "startTime",
    endTime = "endTime",
}

interface Row {
    date: Date;
    user: User;
    startTime: Date;
    endTime: Date;
    data: any;
}

function createData(dutyHour: DutyHour): Row {
    const date = dutyHour.signInBooking?.bookingTime;
    const user = dutyHour.signInBooking.user;
    const startTime = dutyHour.signInBooking?.bookingTime;
    const endTime = dutyHour.signOutBooking?.bookingTime;
    const data = dutyHour;
    return { date, user, startTime, endTime, data };
}

interface Props {
    onRowClick: (data: any) => void;
    onDeleteClick: (data: any) => void;
    refreshCount: number;
}

export interface DutyHourSearchOptions extends TableSearch {
    userName?: string;
    sortColumn?: string;
    isDescending?: boolean;
    skip: number;
    take: number;
}

export default function DutyHoursTable({
    onRowClick,
    onDeleteClick,
    refreshCount,
}: Props) {
    const { t } = useTranslation();
    const currentUser = authenticationService.currentUserValue;
    const canEdit =
        currentUser?.rights?.some(
            (x) => x === Rights.DUTY_HOURS_BOOKING_EDIT
        ) ?? false;
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [data, setData] = useState<List<DutyHour> | null>(null);
    const [totalRowCount, setTotalRowCount] = useState<number>(0);
    const [searchOptions, setSearchOptions] = useState<DutyHourSearchOptions>({
        sortColumn: ColumnId.date,
        isDescending: false,
        skip: 0,
        take: 25,
    } as DutyHourSearchOptions);

    const onChangeUserName = (
        event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    ) => {
        setSearchOptions({
            ...searchOptions,
            userName:
                !event.target.value || 0 === event.target.value.length
                    ? undefined
                    : event.target.value,
        });
    };

    useEffect(() => {
        setIsLoading(true);
        requestService
            .post<{ data: List<DutyHour>; totalRowCount: number }>(
                `/DutyHours/findBySearchValue`,
                searchOptions
            )
            .then((res) => {
                setData(res.data);
                setTotalRowCount(res.totalRowCount);
                setIsLoading(false);
            });
    }, [searchOptions, refreshCount]);

    const columns: TableColumn[] = [
        {
            lable: t("common.date"),
            id: ColumnId.date,
            minWidth: 85,
            element: (row, column) => {
                const value = row ? row[column.id] : null;
                return (
                    <TableCellCustom
                        column={column}
                        columnId={`${row?.data?.ident?.ident}_${ColumnId.date}`}
                        minWidth={370}
                    >
                        {value ? format(new Date(value), "dd.MM.yyyy") : null}
                    </TableCellCustom>
                );
            },
        },
        {
            lable: t("common.userName"),
            id: ColumnId.user,
            minWidth: 270,
            element: (row, column) => {
                const value = row ? (row[column.id] as User) : null;
                return (
                    <TableCellCustom
                        column={column}
                        columnId={`${row?.data?.ident?.ident}_${ColumnId.user}`}
                        minWidth={370}
                    >
                        {`${value?.firstName}, ${value?.lastName}`}
                    </TableCellCustom>
                );
            },
            onChangeSearchValue: onChangeUserName,
        },
        {
            lable: t("common.beginning"),
            id: ColumnId.startTime,
            notSortable: true,
            minWidth: 110,
            element: (row, column) => {
                const value = row ? row[column.id] : null;
                return (
                    <TableCellCustom
                        column={column}
                        columnId={`${row?.data?.ident?.ident}_${ColumnId.startTime}`}
                        minWidth={370}
                    >
                        {value
                            ? format(new Date(value), "dd.MM.yyyy HH:mm")
                            : null}
                    </TableCellCustom>
                );
            },
        },
        {
            lable: t("common.end"),
            id: ColumnId.endTime,
            notSortable: true,
            minWidth: 110,
            element: (row, column) => {
                const value = row ? row[column.id] : null;
                return (
                    <TableCellCustom
                        column={column}
                        columnId={`${row?.data?.ident?.ident}_${ColumnId.endTime}`}
                        minWidth={370}
                    >
                        {value
                            ? format(new Date(value), "dd.MM.yyyy HH:mm")
                            : null}
                    </TableCellCustom>
                );
            },
        },
        {
            lable: t("common.hours"),
            id: ColumnId.endTime,
            notSortable: true,
            minWidth: 30,
            element: (row, column) => {
                const value = row ? (row.data as DutyHour) : null;
                return (
                    <TableCellCustom
                        column={column}
                        columnId={`${row?.data?.ident?.ident}_${ColumnId.endTime}`}
                        minWidth={370}
                    >
                        {value
                            ? (
                                  differenceInMilliseconds(
                                      new Date(
                                          value?.signOutBooking.bookingTime
                                      ),
                                      new Date(value?.signInBooking.bookingTime)
                                  ) /
                                  1000 /
                                  60 /
                                  60
                              ).toFixed(2)
                            : undefined}
                    </TableCellCustom>
                );
            },
        },
    ];

    return (
        <StickyHeadTable
            canDelete={false}
            canEdit={canEdit}
            onDeleteClick={onDeleteClick}
            createData={createData}
            onRowClick={onRowClick}
            isLoading={isLoading}
            data={data}
            totalRowCount={totalRowCount}
            columns={columns}
            searchOptions={searchOptions}
            setSearchOptions={setSearchOptions}
        />
    );
}
