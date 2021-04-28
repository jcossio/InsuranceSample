using InsuranceSample.Domain.Models;
using InsuranceSample.Infrastructure;
using InsuranceSample.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceSample.web
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
            services.AddRazorPages();

            CreateInitialDatabase();

            services.AddTransient<InsuranceContext>();
            services.AddTransient<IRepository<Policy>, PolicyRepository>();
            services.AddTransient<IRepository<Client>, ClientRepository>();
        }

        public void CreateInitialDatabase()
        {
            using (var context = new InsuranceContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                var client1 = new Client { ClientId = 123456789, Name = "John Doe" };
                var client2 = new Client { ClientId = 987654321, Name = "Rowan Atkinson" };
                var client3 = new Client { ClientId = 112233445, Name = "Robert Wagner" };
                var client4 = new Client { ClientId = 556677889, Name = "Erik Estrada" };
                var client5 = new Client { ClientId = 222222222, Name = "Templeton Peck" };

                var clientRepository = new ClientRepository(context);

                clientRepository.Add(client1);
                clientRepository.Add(client2);
                clientRepository.Add(client3);
                clientRepository.Add(client4);
                clientRepository.Add(client5);

                clientRepository.SaveChanges();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
