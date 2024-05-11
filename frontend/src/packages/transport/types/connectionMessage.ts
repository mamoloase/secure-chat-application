import ConnectionParameter from "./connectionParameter";

export default interface ConnectionMessage {
    Id?: string,
    AnswerId?: string,
    Method: string,
    Handler: string,
    Parameters: Array<ConnectionParameter>
}