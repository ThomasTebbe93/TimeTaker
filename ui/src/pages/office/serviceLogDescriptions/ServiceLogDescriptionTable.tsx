import { List } from "immutable";
import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { TableCellCustom } from "../../../components/tables/TableCellCustom";
import StickyHeadTable, {
    TableColumn,
    TableSearch,
} from "../../../components/tables/Table";
import { requestService } from "../../../services/requestService";
import { User } from "../../../types/user";
import { ServiceLogDescription } from "../../../types/serviceLogDescription";

enum ColumnId {
    id = "id",
    shortcut = "shortcut",
    name = "name",
}

interface Row {
    id: string;
    shortcut: string;
    name: string;
    data: any;
}

function createData(serviceLogDescription: ServiceLogDescription): Row {
    const id = serviceLogDescription.id;
    const shortcut = serviceLogDescription.shortcut;
    const name = serviceLogDescription.name;
    const data = serviceLogDescription;
    return { id, shortcut, name, data };
}

interface Props {
    refreshCount: number;
}

interface ServiceLogDescriptionSearchOptions extends TableSearch {
    id?: string;
    shortcut?: string;
    name?: string;
    sortColumn?: string;
    isDescending?: boolean;
    skip: number;
    take: number;
}

export default function ServiceLogDescriptionTable({ refreshCount }: Props) {
    const { t } = useTranslation();
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [data, setData] = useState<List<User> | null>(null);
    const [totalRowCount, setTotalRowCount] = useState<number>(0);
    const [searchOptions, setSearchOptions] =
        useState<ServiceLogDescriptionSearchOptions>({
            sortColumn: ColumnId.id,
            isDescending: false,
            skip: 0,
            take: 25,
        } as ServiceLogDescriptionSearchOptions);

    const onChangeId = (
        event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    ) => {
        event.stopPropagation();
        event.preventDefault();
        const value = event.target.value;
        setSearchOptions({
            ...searchOptions,
            id: !value || 0 === value.length ? undefined : event.target.value,
        });
    };

    const onChangeShortcut = (
        event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    ) => {
        event.stopPropagation();
        event.preventDefault();
        const value = event.target.value;
        setSearchOptions({
            ...searchOptions,
            shortcut:
                !value || 0 === value.length ? undefined : event.target.value,
        });
    };

    const onChangeName = (
        event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    ) => {
        event.stopPropagation();
        event.preventDefault();
        const value = event.target.value;
        setSearchOptions({
            ...searchOptions,
            name: !value || 0 === value.length ? undefined : event.target.value,
        });
    };

    useEffect(() => {
        setIsLoading(true);
        requestService
            .post<{ data: List<User>; totalRowCount: number }>(
                `/serviceLogDescription/findBySearchValue`,
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
            lable: t("common.id"),
            id: ColumnId.id,
            element: (row, column) => {
                const value = row ? row[column.id] : null;
                return (
                    <TableCellCustom
                        column={column}
                        columnId={`${row?.data?.id}_${ColumnId.id}`}
                    >
                        {value}
                    </TableCellCustom>
                );
            },
            onChangeSearchValue: onChangeId,
        },
        {
            lable: t("common.name"),
            id: ColumnId.name,
            element: (row, column) => {
                const value = row ? row[column.id] : null;
                return (
                    <TableCellCustom
                        column={column}
                        columnId={`${row?.data?.id}_${ColumnId.name}`}
                    >
                        {value}
                    </TableCellCustom>
                );
            },
            onChangeSearchValue: onChangeName,
        },
        {
            lable: t("common.shortcut"),
            id: ColumnId.shortcut,
            element: (row, column) => {
                const value = row ? row[column.id] : null;
                return (
                    <TableCellCustom
                        column={column}
                        columnId={`${row?.data?.id}_${ColumnId.shortcut}`}
                    >
                        {value}
                    </TableCellCustom>
                );
            },
            onChangeSearchValue: onChangeShortcut,
        },
    ];

    return (
        <StickyHeadTable
            canDelete={false}
            canEdit={false}
            key="serviceLogDescriptionTable"
            createData={createData}
            onRowClick={() => {}}
            isLoading={isLoading}
            data={data}
            totalRowCount={totalRowCount}
            columns={columns}
            searchOptions={searchOptions}
            setSearchOptions={setSearchOptions}
        />
    );
}
