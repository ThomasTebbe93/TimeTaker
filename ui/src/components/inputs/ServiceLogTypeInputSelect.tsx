import { requestService } from "../../services/requestService";
import { ServiceLogType } from "../../types/serviceLogType";
import InputSelect from "./Inputselect";

interface Props {
    onChange: (value: unknown) => void;
    value: ServiceLogType | undefined;
    disabled?: boolean;
}

const loade = (inputValue: string) =>
    requestService.post<ServiceLogType[]>(`/serviceLogType/autocomplete`, {
        searchValue: inputValue,
    });

const checkSelceted = (option: ServiceLogType, value: ServiceLogType) =>
    option?.id === value?.id;

export default function ServiceLogTypeInputSelect({
    onChange,
    value,
    disabled = false,
}: Props) {
    return (
        <InputSelect
            checkSelceted={checkSelceted}
            loade={loade}
            onChange={onChange}
            value={value ?? ({} as ServiceLogType)}
            disabled={disabled}
        />
    );
}
