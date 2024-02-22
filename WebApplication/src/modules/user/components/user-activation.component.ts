import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PASSWORD_REGEX } from 'src/infrastructure/constants/password-regex.constant';
import { UserActivationDto } from '../dtos/user-activation.dto';
import { UserActivationResource } from '../resources/user-activation.resource';

@Component({
  selector: 'user-activation',
  templateUrl: 'user-activation.component.html',
  styleUrls: ['./credentials-container.styles.css']
})

export class UserActivationComponent implements OnInit {
  model = new UserActivationDto();

  passwordRegex = PASSWORD_REGEX;

  passwordAgain: string;
  isSuccess = false;

  errors = {
    isTokenExpired: false,
    isTokenUsed: false,
    hasMismatchingPasswords: false
  };

  constructor(
    private route: ActivatedRoute,
    private resource: UserActivationResource
  ) { }

  ngOnInit() {
    this.model.token = this.route.snapshot.queryParams.token;
  }

  activate(): void {
    if (this.model.password === this.passwordAgain) {
      this.resource.activateUser(this.model)
        .subscribe(() => this.isSuccess = true);
    } else {
      this.errors.hasMismatchingPasswords = true;
    }
  }
}
