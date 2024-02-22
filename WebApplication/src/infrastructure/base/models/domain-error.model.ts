import { DomainMessage } from './domain-message.model';

export class DomainError {
  status: string;
  messages: DomainMessage[];
}
