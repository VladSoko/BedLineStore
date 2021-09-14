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
            services.AddTransient<IMainInfoService, MainInfoService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICartLineService, CartLineService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFreelanceSewingService, FreelanceSewingService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IConsultationDateService, ConsultationDateService>();
            services.AddTransient<IConsultationInfoService, ConsultationInfoService>();
            
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IExcelService, ExcelService>();
        }
    }
}