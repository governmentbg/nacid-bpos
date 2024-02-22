using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenScience.Common.DomainValidation;
using OpenScience.Common.Services;
using OpenScience.Data.Institutions.Models;
using OpenScience.Data.Publications.Models;
using OpenScience.Services.Auth;
using OpenScience.Services.Base;
using OpenScience.Services.Classifications;
using OpenScience.Services.Emails;
using OpenScience.Services.Institutions;
using OpenScience.Services.Logs;
using OpenScience.Services.Nomenclatures;
using OpenScience.Services.Publications;
using OpenScience.Services.Users;
using ServerApplication.Infrastructure.Auth;

namespace ServerApplication.Infrastructure.Extensions
{
	public static class ServicesDiConfigurationExtension
	{
		public static void ConfigureDI(this IServiceCollection services)
		{
			// Base
			services
				.AddScoped(typeof(IEntityService<>), typeof(BaseEntityService<>))
				.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
				;

			// Classification module
			services
				.AddScoped<ClassificationService>()
				.AddScoped<ClassificationClosureService>()
				;

			// Publication module
			services
				.AddScoped<PublicationService>()
				.AddScoped<IEntityService<Publication>, PublicationService>()
				.AddScoped<PublicationHandleService>()
				.AddScoped<PublicationIndexingService>()
				.AddScoped<PublicationPermissionVerificator>()
				;

			// Institutions module
			services
				.AddScoped<InstitutionService>()
				.AddScoped<IEntityService<Institution>, InstitutionService>()
				;

			// Common
			services
				.AddScoped<RetryExecutionService>()
				;

			// Users module
			services
				.AddScoped<UserContext>()
				.AddScoped<PasswordHasher>()
				.AddScoped<TokenService>()
				.AddScoped<UserService>()
				.AddScoped<RoleService>()
				;

			// Emails module
			services
				.AddScoped<EmailService>();
			
			// Nomenclatures module
			services
				.AddScoped(typeof(INomenclatureService<>), typeof(BaseNomenclatureService<>))
				.AddScoped<IAliasNomenclatureService, AliasNomenclatureService>();

			// Logger module
			services.AddScoped<DbLoggerService>();

			// Validation module
			services.AddScoped<DomainValidationService>();
		}
	}
}
