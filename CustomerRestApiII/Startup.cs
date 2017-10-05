using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAppBLL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CustomerAppBLL.BusinessObjects;

namespace CustomerRestApiII
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(o => o.AddPolicy("MyPolicy", builder => {
                builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var facade = new BLLFacade();
                var address = facade.AddressService.Create(
                    new AddressBO()
                    {
                        City = "Ankara",
                        Street = "Bilkent",
                        Number = "22A"
                    });

                var address2 = facade.AddressService.Create(
                    new AddressBO()
                    {
                        City = "Istanbul",
                        Street = "Bahçeşehir",
                        Number = "22"
                    });

                var address3 = facade.AddressService.Create(
                    new AddressBO()
                    {
                        City = "Ankara",
                        Street = "Çankaya",
                        Number = "44d"
                    });

                var cust = facade.CustomerService.Create(
                    new CustomerBO()
                    {
                        FirstName = "Emir",
                        LastName = "Hazır",
                        AddressIds = new List<int>() { address.Id, address3.Id }
                    });
                facade.CustomerService.Create(
                    new CustomerBO()
                    {
                        FirstName = "Aziz",
                        LastName = "Hazır",
                        AddressIds = new List<int>() { address.Id, address2.Id }
                    });
                for (int i = 0; i < 5; i++)
                {
                    facade.OrderService.Create(
                        new OrderBO()
                        {
                            DeliveryDate = DateTime.Now.AddMonths(1),
                            OrderDate = DateTime.Now.AddMonths(-1),
                            CustomerId = cust.Id
                        });
                }
            }

            app.UseMvc();
        }
    }
}
