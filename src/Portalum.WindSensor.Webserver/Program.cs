using Nager.TcpClient;
using Portalum.WindSensor;
using Portalum.WindSensor.Webserver;
using Portalum.WindSensor.Webserver.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWindowsService(configure =>
{
    configure.ServiceName = "Portalum.WindSensor";
});

// Add services to the container.
builder.Services.AddSingleton<TcpClient>();
builder.Services.AddSingleton<GillWindSensorParser>();
builder.Services.AddSingleton<IWindSensorService, WindSensorService>();

var app = builder.Build();

app.Services.GetService<IWindSensorService>();

// Configure the HTTP request pipeline.

//app.MapGet("/", () => "<html><body>Hello World!</body></html>");
app.MapGet("/", async (HttpContext context) =>
{
    //sets the content type as html
    context.Response.Headers.ContentType = new Microsoft.Extensions.Primitives.StringValues("text/html; charset=UTF-8");
    await context.Response.SendFileAsync("wwwroot/index.html");
});

app.MapGet("/cgi-bin/CGI_GetMeasurement.cgi", (HttpRequest request, IWindSensorService windSensorService) => 
{
    var inputId = request.Query["input_id"];

    var lastValue = windSensorService.GetLastValue();
    if (lastValue == null)
    {
        return new XmlResult<Measurements>(new Measurements
        {
            Measurement = new Measurement
            {
                Sequencenum = 0,
                Sourceid = 0,
                Localtime = new Localtime
                {
                    Date = DateTime.Today.ToString("yyyy:MM:dd"),
                    Time = DateTime.Now.ToString("HH:mm:ss")
                },
                Isvalid = 0,
                Error = "NO_DATA"
            }
        });
    }

    var csv = $"A,{lastValue.WindDirection},{lastValue.WindSpeed:000.00},{lastValue.Units},00,00";

    return new XmlResult<Measurements>(new Measurements
    {
        Measurement = new Measurement
        {
            Sequencenum = 0,
            Sourceid = 0,
            Localtime= new Localtime
            {
                Date = DateTime.Today.ToString("yyyy:MM:dd"),
                Time = DateTime.Now.ToString("HH:mm:ss")
            },
            Csv = csv,
            Isvalid = 1,
            Error = "ERROR_NONE"
        }
    });

});

app.MapGet("/data", (IWindSensorService windSensorService) =>
{
    var lastValue = windSensorService.GetLastValue();
    if (lastValue == null)
    {
        return Results.Json(lastValue);
    }

    return Results.StatusCode(StatusCodes.Status204NoContent);
    
});

app.Run("http://localhost:8023");
