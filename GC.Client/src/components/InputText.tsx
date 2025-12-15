
interface Props {
    value: string,
    inputName : string
    handleInputChange : (params: any) => any 
}

export default function InputText({value, inputName, handleInputChange} : Props) {

    
    return (
        <input
            type="text"
            className="form-control"
            id={inputName}
            required
            value={value}
            onChange={handleInputChange}
            name={inputName}
        />
    )
}