import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './components/menu.component';
import { FooterComponent } from './components/footer.component';
import { PageNotFoundComponent } from './components/page-not-found.component';

import { Configuration, configurationFactory } from 'src/infrastructure/configuration/configuration';
import { NgBootstrapModule } from '../infrastructure/ng-bootstrap.module';

import { ComponentsModule } from '../components/components.module';
import { SearchModule } from '../search/search.module';
import { PublicationModule } from 'src/publication/publication.module';
import { ClassificationsModule } from 'src/classifications/classifications.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    FooterComponent,
    PageNotFoundComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    FontAwesomeModule,
    NgBootstrapModule,
    ComponentsModule,
    SearchModule,
    PublicationModule,
    ClassificationsModule,
    AppRoutingModule
  ],
  providers: [
    Configuration,
    {
      provide: APP_INITIALIZER,
      useFactory: configurationFactory,
      deps: [Configuration],
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
