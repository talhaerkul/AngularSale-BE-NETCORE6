using AngularSaleAPI.Application.Abstractions.Services.AuthorizationServices;
using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using AngularSaleAPI.Application.Abstractions.Services.UserServices;
using AngularSaleAPI.Application.Abstractions.Services.UserServices.Authentication;
using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Application.Repositories.Category;
using AngularSaleAPI.Application.Repositories.CompletedOrder;
using AngularSaleAPI.Application.Repositories.File;
using AngularSaleAPI.Application.Repositories.InvoiceFile;
using AngularSaleAPI.Application.Repositories.ProductImageFile;
using AngularSaleAPI.Domain.Entities.Identity;
using AngularSaleAPI.Persistence.Contexts;
using AngularSaleAPI.Persistence.Repositories;
using AngularSaleAPI.Persistence.Repositories.Category;
using AngularSaleAPI.Persistence.Repositories.CompletedOrder;
using AngularSaleAPI.Persistence.Repositories.File;
using AngularSaleAPI.Persistence.Repositories.InvoiceFile;
using AngularSaleAPI.Persistence.Repositories.ProductImageFileReadRepository;
using AngularSaleAPI.Persistence.Repositories.ProductImageFileWriteRepository;
using AngularSaleAPI.Persistence.Services.AuthorizationServices;
using AngularSaleAPI.Persistence.Services.ProductServices;
using AngularSaleAPI.Persistence.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<AngularSaleAPIDbContext>(options => options.UseMySql(Configuration.ConnectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")));
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.MaxFailedAccessAttempts = 7;
                
            }).AddEntityFrameworkStores<AngularSaleAPIDbContext>()
            .AddDefaultTokenProviders(); // reset password tokenı için
            
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();
            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();
            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<ICompletedOrderReadRepository, CompletedOrderReadRepository>();
            services.AddScoped<ICompletedOrderWriteRepository, CompletedOrderWriteRepository>();
            services.AddScoped<IMenuWriteRepository, MenuWriteRepository>();
            services.AddScoped<IMenuReadRepository, MenuReadRepository>();
            services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();
            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();


            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthService, AuthService>();
            services.AddScoped<IInternalAuthService, AuthService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();













        }
    }
}
