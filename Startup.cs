using CalazansUdemy.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalazansUdemy
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
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2)
                             .AddXmlSerializerFormatters();

            services.AddControllers();

            services.AddDbContext<ProdutoContexto>(opt => opt.UseInMemoryDatabase(databaseName: "produtoInMemory")
                                                             .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddTransient<IProdutoRepository, ProdutoRepository>();

            services.AddVersionedApiExplorer(opt=>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl= true;
            });

            services.AddApiVersioning();

            services.AddResponseCaching();

            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var item in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = $"Api Produto {item.ApiVersion}",
                        Version = item.ApiVersion.ToString()
                    });
                }

                //  c.SwaggerDoc("v1", new OpenApiInfo { Title = "CalazansUdemy", Version = "v1" });
            });

            services.AddResponseCompression(opt =>
            {
                // opt.Providers.Add<GzipCompressionProvider>();
                opt.Providers.Add<BrotliCompressionProvider>();
                opt.EnableForHttps = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

               // app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalazansUdemy v1"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                foreach(var item in provider.ApiVersionDescriptions) 
                {
                    c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName);
                }
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
