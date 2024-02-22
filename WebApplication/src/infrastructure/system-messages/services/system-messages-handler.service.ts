import { EventEmitter, Injectable } from '@angular/core';
import { SystemMessage } from '../models/system-message.model';

@Injectable()
export class SystemMessagesHandlerService {
  messages: EventEmitter<SystemMessage>;
  clear: EventEmitter<void>;
  timeoutSeconds = 30;

  constructor() {
    this.messages = new EventEmitter<SystemMessage>();
    this.clear = new EventEmitter<void>();
  }
}
