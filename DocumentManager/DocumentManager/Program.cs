using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp/public")),
    RequestPath = "/Docs"
});

app.Map("/pdfs/{*path}", async context =>
{
    var pdfPath = context.Request.Path.Value;
    var fileInfo = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Docs", pdfPath));

    if (fileInfo.Exists)
    {
        context.Response.ContentType = "application/pdf";
        await context.Response.SendFileAsync(fileInfo.FullName);
    }
    else
    {
        context.Response.StatusCode = 404;
    }
});

app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
