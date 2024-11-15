﻿using Inventory.Shared.Dtos.ConfigDto;

ServiceStackHelper.SetLicense();

var builder = WebApplication.CreateBuilder(args);
//var services = builder.Services;

// services.AddServiceStack(typeof(MainService).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

// app.UseStaticFiles();
//
// app.UseServiceStack(new AppHost(), options => {
//     options.MapEndpoints();
// });

app.Run();