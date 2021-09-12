using BedLinenStore.WEB.Services.Implementations;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BedLinenStore.WEB.DependencyInjection
{
    public static class Ioc
    {
        public static void Build(this IServiceCollection services)
        {
            //Services
            services.AddScoped<IMainInfoService, MainInfoService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartLineService, CartLineService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFreelanceSewingService, FreelanceSewingService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddTransient<IConsultationDateService, ConsultationDateService>();
            services.AddTransient<IConsultationInfoService, ConsultationInfoService>();
            
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IExcelService, ExcelService>();
        }
    }
}