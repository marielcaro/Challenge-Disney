using Disney.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Disney.Repositories;
using Disney.Interfaces;
using Disney.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SendGrid.Extensions.DependencyInjection;
using Disney.Services;

namespace Disney
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Disney", Version = "v1" });

                //Incorporo el uso del token dentro del swagger
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme { 
                        Name="Authorization",
                        Type= SecuritySchemeType.ApiKey,
                        Scheme="Bearer",
                        BearerFormat="JWT",
                        In=ParameterLocation.Header,
                        Description="Ingrese Bearer [Token] para poder identificarse dentro de la aplicación"


                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                          {
                                 Reference= new OpenApiReference
                                 {
                                        Type=ReferenceType.SecurityScheme,
                                        Id="Bearer"
                                 }
                          },
                        new List<string>()
                    }

                });
  
            });

            //Inyecta las dependencias necesarias de identity
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UserContext>().AddDefaultTokenProviders();
            //Toma un usuario, los roles y coloca la configuración por default.

            //Añade un tipo de autentificación:
            /*Agrega autentificacion JWT ("Con "Bearers"")*/
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => {
                    options.SaveToken = true; // que guarde el toke na autorizado
                    options.RequireHttpsMetadata = false; //no necesito aun https
                    options.TokenValidationParameters = new TokenValidationParameters
                    { //parametros para la validacion del token
                        ValidateIssuer = true, //entidad que genera el token
                        ValidateAudience=true, //los que usan o ven el token
                        ValidAudience="https://localhost:5001",
                        ValidIssuer = "https://localhost:5001",
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySecretaSuperLargaDeAutorizacion")) //Encargado de proveernos info con la llave secreta para ingresar a la aplicación
                       // TOKEN = HEADER + PILOT + FIRMA
                    };
                });



            services.AddEntityFrameworkSqlServer();
            services.AddDbContextPool<DisneyContext>(optionsAction: (provider, builder) => {
                builder.UseInternalServiceProvider(provider);
                builder.UseSqlServer(connectionString: "Data Source=(localdb)\\MSSQLLocalDB;Database=DisneyDb;Integrated Security=True;");
            });

            services.AddDbContext<UserContext>(optionsAction:(services, options)=>
                {
                    options.UseInternalServiceProvider(services);
                    options.UseSqlServer(connectionString: "Data Source=(localdb)\\MSSQLLocalDB;Database=UsersDb;Integrated Security=True;");
            });

            // Inyección de dependencia de repositorios:
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();
            services.AddScoped<IMovieOrSerieRepository, MovieOrSerieRepository>();

            //Inyección de dependencia del mail:
            services.AddSendGrid(o =>
            {
                o.ApiKey = "poner aquí la api key"; //API KEY
            });

            services.AddScoped<IMailService, MailService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Disney v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
