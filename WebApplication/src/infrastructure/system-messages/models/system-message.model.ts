import { SystemMessageType } from '../enums/system-message-type.enum';

export class SystemMessage {
  text: string;
  type: SystemMessageType;
  timeoutSeconds?: number;

  constructor(
    text: string,
    type: SystemMessageType,
    timeoutSeconds?: number
  ) {
    this.text = text;
    this.type = type;
    this.timeoutSeconds = timeoutSeconds
  }
}
