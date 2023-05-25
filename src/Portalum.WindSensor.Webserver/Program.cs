using Nager.TcpClient;
using Portalum.WindSensor;
using Portalum.WindSensor.Webserver;
using Portalum.WindSensor.Webserver.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<TcpClient>();
builder.Services.AddSingleton<GillWindSensorParser>();
builder.Services.AddSingleton<IWindSensorService, WindSensorService>();

var app = builder.Build();

app.Services.GetService<IWindSensorService>();

// Configure the HTTP request pipeline.

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

app.Run("http://localhost:8023");
