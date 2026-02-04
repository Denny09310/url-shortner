using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client.Components;
using Client.Services;
using LumexUI.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient<ApiClient>(client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/"));

builder.Services.AddLumexServices();

builder.Services.AddScoped<Clipboard>();
builder.Services.AddScoped<BottomSheetService>();

builder.Services.AddScoped<ManagementState>();

await builder.Build().RunAsync();
