// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POC;

var builder = new ConfigurationBuilder()
        //.SetBasePath("path here") //<--You would need to set the path
        .AddJsonFile("appsettings.json"); //or what ever file you have the settings

IConfiguration configuration = builder.Build();

var services = new ServiceCollection();

services.AddHttpClient();
services.AddSingleton<IConfiguration>(ctx => { return configuration; });
services.AddScoped<IPOCService, POCService>();
services.AddScoped<IAuthService, AuthService>();

var serviceProvider = services.BuildServiceProvider();

var myService = serviceProvider.GetService<IPOCService>();

await myService.TestCometD();


