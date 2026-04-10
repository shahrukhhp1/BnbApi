using BnbApi;
using BnbApi.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BnbDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Bnb")));

builder.Services.AddScoped<BusinessLogic>();

builder.Services
    .AddControllersWithViews()
    .AddXmlSerializerFormatters()
    .AddMvcOptions(options =>
    {
        options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
        var xmlFormatter = options.OutputFormatters.OfType<XmlSerializerOutputFormatter>().FirstOrDefault();
        if (xmlFormatter != null)
        {
            options.OutputFormatters.Remove(xmlFormatter);
            options.OutputFormatters.Insert(0, xmlFormatter);
        }
    });

var app = builder.Build();

app.UseStaticFiles();

app.MapControllers();

app.Run();
