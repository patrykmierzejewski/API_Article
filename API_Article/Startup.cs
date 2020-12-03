using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Article.Entities;
using API_Article.Models;
using API_Article.Validators;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
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
            //validate emial and password
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddControllers().AddFluentValidation();
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

            //hash of password
            
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ArticleSeeder articleSeeder)
        {
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
    }
}
