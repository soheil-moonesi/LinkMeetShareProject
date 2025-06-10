global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
using BlazorAppFront;
using BlazorAppFront.Services;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using static BlazorAppFront.Pages.Home;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("ApiCalls");

builder.Services.AddScoped<UserServices>();
builder.Services.AddRadzenComponents();
builder.Services.AddTransient<IValidator<Model>, UserValidator>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
