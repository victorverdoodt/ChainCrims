using ChainCrims.Background;
using ChainCrims.Data;
using ChainCrims.DataBase;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChainCrims
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

            services.AddMvc();
            services.AddBscScanner(opt =>
            {
                opt.ApiKey = "RBA89G9APKJM77247RNIVRD985DBBEVT17";
            });

            services.AddDbContext<ChainCrimsContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Default")).UseLazyLoadingProxies());
            services.AddInMemorySubscriptions();
            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddFiltering()
                .AddSorting();

            services.AddHostedService<ContractListener>();
            services.AddHostedService<WalletListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/api",
                Path = "/playground"
            });

            app.UseRouting();
            app.UseWebSockets();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL("/api");
            });
        }
    }
}
