export class PublicationStep {
  isValid: boolean;
  forms: {};

  constructor(isValid: boolean) {
    this.isValid = isValid;
    this.forms = {};
  }

  changeFormValidState(formName: string, isValid: boolean) {
    if (!this.forms[formName]) {
      this.forms[formName] = { isValid: false };
    }

    this.forms[formName].isValid = isValid;

    const keys = Object.keys(this.forms);
    let isValidStep = keys.length > 0;
    keys.forEach(form => {
      if (!this.forms[form].isValid) {
        isValidStep = false;
        return;
      }
    });

    this.isValid = isValidStep;
  }
}
