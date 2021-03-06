using Application.Interfaces;
using Infrastructure.Email;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class UserServicesExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
            services.Configure<EmailSettings>(config.GetSection("Authentication:Email"));


            return services;
        }
    }
}