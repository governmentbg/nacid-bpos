import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { Configuration, configurationFactory } from 'src/infrastructure/base/configuration/configuration';
import { ConfirmDialogComponent } from 'src/infrastructure/components/confirm-modal/confirm-modal.component';
import { CurrentUserComponent } from 'src/infrastructure/components/current-user/current-user.component';
import { LanguageSelectComponent } from 'src/infrastructure/components/language-select/language-select.component';
import { LoadingIndicatorComponent } from 'src/infrastructure/components/loading-indicator/loading-indicator.component';
import { LoadingIndicatorService } from 'src/infrastructure/components/loading-indicator/loading-indicator.service';
import { AppMenuComponent } from 'src/infrastructure/components/menu/app-menu.component';
import { AuthGuard } from 'src/infrastructure/interceptors/auth.guard';
import { InterceptorsModule } from 'src/infrastructure/interceptors/interceptors.module';
import { MaterialComponentsModule } from 'src/infrastructure/material-components.module';
import { IsActivePipe } from 'src/infrastructure/pipes/is-active.pipe';
import { SafeHtmlPipe } from 'src/infrastructure/pipes/safe-html.pipe';
import { UserPermissionVerificatorService } from 'src/infrastructure/services/user-permission-verification.service';
import { SystemMessagesHandlerService } from 'src/infrastructure/system-messages/services/system-messages-handler.service';
import { SystemMessagesComponent } from 'src/infrastructure/system-messages/system-messages.component';
import { ClassificationModule } from 'src/modules/classification/classification.module';
import { InstitutionModule } from 'src/modules/institution/institution.module';
import { NomenclatureModule } from 'src/modules/nomenclature/nomenclature.module';
import { PublicationModule } from 'src/modules/publication/publication.module';
import { UserLoginService } from 'src/modules/user/services/user-login.service';
import { UserRoleService } from 'src/modules/user/services/user-role.service';
import { UserModule } from 'src/modules/user/user.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuItemService } from './menu-items.service';

@NgModule({
  declarations: [
    AppComponent,
    AppMenuComponent,
    LanguageSelectComponent,
    CurrentUserComponent,
    ConfirmDialogComponent,
    SafeHtmlPipe,
    IsActivePipe,
    LoadingIndicatorComponent,
    SystemMessagesComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    ClassificationModule,
    InstitutionModule,
    PublicationModule,
    UserModule,
    NomenclatureModule,
    MaterialComponentsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    AuthGuard,
    Configuration,
    LoadingIndicatorService,
    MenuItemService,
    {
      provide: APP_INITIALIZER,
      useFactory: configurationFactory,
      deps: [Configuration],
      multi: true
    },
    UserLoginService,
    UserRoleService,
    UserPermissionVerificatorService,
    InterceptorsModule,
    SystemMessagesHandlerService
  ],
  entryComponents: [
    ConfirmDialogComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
