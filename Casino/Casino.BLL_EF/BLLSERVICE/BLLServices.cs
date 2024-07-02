using Casino.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL_EF.BLLSERVICE
{
    public static class BLLServices
    {
        public static IServiceCollection AddBLL(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddTransient(s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddScoped<IUser, UserEF>();
            services.AddScoped<IResults, ResultEF>();
            services.AddScoped<IGame, GameEF>();
            services.AddScoped<ITransactions, TransactionsEF>();

            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
