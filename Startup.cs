using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

using userService.Repository;
using userService.DBContext;

using userService.Service;

namespace userService
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
            services.AddMvc();
            services.AddEntityFrameworkNpgsql().AddDbContext<UserContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("DB_CONNECTION_STRING")));
            
            services.AddEntityFrameworkNpgsql().AddTransient<IUserRepository, UserRepository>();
            
            services.AddEntityFrameworkNpgsql().AddDbContext<SessionmContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("DB_CONNECTION_STRING")));
            
            services.AddEntityFrameworkNpgsql().AddTransient<ISessionmRepository, SessionmRepository>();

            services.AddEntityFrameworkNpgsql().AddDbContext<SessionwContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("DB_CONNECTION_STRING")));
            
            services.AddEntityFrameworkNpgsql().AddTransient<ISessionwRepository, SessionwRepository>();

            services.AddTokenAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
