using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LimitingPro
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
            services.AddQueuePolicy(option =>
            {
                //最大并发数量
                option.MaxConcurrentRequests = 1;
                //请求队列长度
                option.RequestQueueLimit = 1;
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(next =>
            {
                return async context =>
                {
                    Console.WriteLine(DateTime.Now.ToString());
                    //记录一下访问次数
                    GlobalContext.RequestQty++;
                    await next(context);
                };
            });

            //添加并发限制中间件
            app.UseConcurrencyLimiter();
            app.Run(async context =>
            {
                GlobalContext.RequestQtyP++;
                //Task.Delay(300).Wait();
                await context.Response.WriteAsync(string.Format("Hello World! RequestQty:{0} RequestQtyP:{1}", GlobalContext.RequestQty, GlobalContext.RequestQtyP));
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
