import { Component } from '@angular/core';
import { SystemMessageType } from './enums/system-message-type.enum';
import { SystemMessage } from './models/system-message.model';
import { SystemMessagesHandlerService } from './services/system-messages-handler.service';

@Component({
  selector: 'system-messages',
  templateUrl: 'system-messages.component.html',
  styleUrls: ['./system-messages.styles.css']
})

export class SystemMessagesComponent {
  systemMessages: SystemMessage[] = [];
  systemMessageTypes = SystemMessageType;

  constructor(
    private systemMessagesHandler: SystemMessagesHandlerService
  ) {
    systemMessagesHandler.messages.subscribe((message: SystemMessage) => {
      this.addMessage(message);
    });

    systemMessagesHandler.clear.subscribe(() => {
      this.clearSystemMessages();
    });
  }

  removeSystemMessage(systemMessage: SystemMessage) {
    const index = this.systemMessages.indexOf(systemMessage);
    this.systemMessages.splice(index, 1);
  }

  private addMessage(systemMessage: SystemMessage) {
    this.systemMessages.push(systemMessage);

    const self = this;
    setTimeout(function () {
      self.removeSystemMessage(systemMessage);
    }, (this.systemMessagesHandler.timeoutSeconds) * 1000);
  }

  private clearSystemMessages() {
    this.systemMessages = [];
  }
}
