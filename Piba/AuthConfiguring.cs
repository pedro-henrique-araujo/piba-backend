
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Piba.Data;
using System.Text;

namespace Piba
{
    public class AuthConfiguring
    {
        private readonly IServiceCollection _services;
        private readonly ConfigurationManager _configuration;

        public AuthConfiguring(WebApplicationBuilder webApplicationBuilder)
        {
            _services = webApplicationBuilder.Services;
            _configuration = webApplicationBuilder.Configuration;
        }

        public void Configure()
        {
            AddIdentity();

            AddJwtBearerAuthentication();

            AddAuthorization();
        }

        private void AddJwtBearerAuthentication()
        {
            AddAuthentication()
                .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                };
            });
        }

        private AuthenticationBuilder AddAuthentication()
        {
            return _services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        }

        private void AddIdentity()
        {
            _services.AddIdentity<IdentityUser, IdentityRole>()
              .AddEntityFrameworkStores<PibaDbContext>()
              .AddDefaultTokenProviders();
        }

        private void AddAuthorization()
        {
            _services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
}
