using DapperTemplate.Abstracts;
using DapperTemplate.Abstracts.Services;
using DapperTemplate.BuisinessLogic.Abstraction;
using DapperTemplate.BuisinessLogic.Implementation;
using DapperTemplate.DataAccess.Repository.Abstraction;
using DapperTemplate.DataAccess.Repository.Implementations;
using DapperTemplate.Helper;
using DapperTemplate.Repository;
using DapperTemplate.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DapperTemplate
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

            services.AddSingleton<DbHelper>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IClassService, ClassService>();

            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen();

            services.Configure<AppData>(Configuration.GetSection("AppData"));

            services.AddStackExchangeRedisCache(
                options => options.Configuration = Configuration.GetConnectionString("RedisConnection"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
