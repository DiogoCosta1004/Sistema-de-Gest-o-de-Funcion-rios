using BaseLibrary.Entities;
using Blazored.LocalStorage;
using Client;
using Client.ApplicationStates;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using ClientLibrary.Services.Implementations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7196");
}).AddHttpMessageHandler<CustomHttpHandler>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7196") });
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage(); 
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

builder.Services.AddScoped<IGenericServiceInterface<DepartamentoGeral>, GenericServiceImplementation<DepartamentoGeral>>();
builder.Services.AddScoped<IGenericServiceInterface<Departamento>, GenericServiceImplementation<Departamento>>();
builder.Services.AddScoped<IGenericServiceInterface<Filial>, GenericServiceImplementation<Filial>>();

builder.Services.AddScoped<IGenericServiceInterface<Pais>, GenericServiceImplementation<Pais>>();
builder.Services.AddScoped<IGenericServiceInterface<Cidade>, GenericServiceImplementation<Cidade>>();
builder.Services.AddScoped<IGenericServiceInterface<Town>, GenericServiceImplementation<Town>>();

builder.Services.AddScoped<IGenericServiceInterface<Funcionario>, GenericServiceImplementation<Funcionario>>();

builder.Services.AddScoped<AllState>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();

await builder.Build().RunAsync();
