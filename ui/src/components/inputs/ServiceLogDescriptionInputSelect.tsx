import { requestService } from "../../services/requestService";
import { ServiceLogDescription } from "../../types/serviceLogDescription";
import InputSelect from "./Inputselect";

interface Props {
    onChange: (value: unknown) => void;
    value: ServiceLogDescription | undefined;
    disabled?: boolean;
}

const loade = (inputValue: string) =>
    requestService.post<ServiceLogDescription[]>(
        `/serviceLogDescription/autocomplete`,
        {
            searchValue: inputValue,
        }
    );

const checkSelceted = (
    option: ServiceLogDescription,
    value: ServiceLogDescription
) => option?.id === value?.id;

export default function ServiceLogDescriptionInputSelect({
    onChange,
    value,
    disabled = false,
}: Props) {
    return (
        <InputSelect
            checkSelceted={checkSelceted}
            loade={loade}
            onChange={onChange}
            value={value ?? ({} as ServiceLogDescription)}
            disabled={disabled}
        />
    );
}
