import MessageType from "./messageType";

export default interface MessageModel {
    Name: string,
    Message: string,
    Self : boolean ,
    Type: MessageType,
}