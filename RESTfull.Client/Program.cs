using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RESTfull.Client;
using RESTfull.Client.Services;




var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Регистрация HttpClient для подключения к API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:7170") // Устанавливаем BaseAddress здесь
});

// Регистрация сервисов (УБЕРИТЕ ДУБЛИРОВАНИЕ)
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<EducationService>();

await builder.Build().RunAsync();