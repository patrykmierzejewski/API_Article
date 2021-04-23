using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Article.Entities;
using API_Article.Filters;
using API_Article.Identity;
using API_Article.Models;
using API_Article.Validators;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace API_Article
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SetAuthenticationJwt(services); // tokeny autoryzacji
            SetAuthorization(services); // autentykacja uzytkownika

            services.AddScoped<TimeTrackFilter>();//czas wykonywania wywo³ywany przez Atrybut servicesFilter
            services.AddScoped<IJwtPrivider, JwtPrivider>();
            //validate emial and IPasswordHasher
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            //kontrolery z obs³ug¹ ³apania wyj¹tków oraz walidacja z fluentValidation
            services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter))).AddFluentValidation();

            services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserValidator>();
            //**********************
            services.AddDbContext<ArticleContext>();

            services.AddScoped<ArticleSeeder>();

            services.AddAutoMapper(this.GetType().Assembly);

            //swagger documentation
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Api Article", Version = "v1", Description = "private api" });
            });

            //dodanie mo¿liwoœci po³¹czeñ przez https
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendClient", builder => builder
                                                                    .AllowAnyHeader()
                                                                    .AllowAnyMethod()
                                                                    .WithOrigins("http://localhost:3000"));
            });
        }

       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ArticleSeeder articleSeeder)
        {
            app.UseCors("FrontendClient");
            // dodanie swagera UI 
            app.UseSwagger();
            app.UseSwaggerUI( x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Article API v1");
            });
            //******************

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            
            app.UseAuthentication();
            app.UseAuthorization();
           
            //domyœlne zwracanie 
            //app.Use(async (context, next) => { await context.Response.WriteAsync("Hello from 2nd delegate."); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            /*dodanie danych testowych*/
            articleSeeder.Seed();
        }

        private void SetAuthenticationJwt(IServiceCollection services)
        {
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //IdentityModelEventSource.ShowPII = true;
            JwtOptions jwtOptions = new JwtOptions();
            Configuration.GetSection("jwt").Bind(jwtOptions);
            services.AddSingleton(jwtOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(conf =>
                {
                    conf.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = jwtOptions.JwtIssuer,
                        ValidAudience = jwtOptions.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey)), // podpis wydawcy

                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = false,
                        RequireSignedTokens = true
                    };
                });
        }
        private void SetAuthorization(IServiceCollection services)
        {
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("HasCountry", builder => builder.RequireClaim("Country"));
                options.AddPolicy("HasActive", builder => builder.RequireClaim("isActive", "active"));
            });
        }

    }
}
