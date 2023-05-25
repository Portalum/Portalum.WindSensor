using Portalum.WindSensor.Webserver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/cgi-bin/CGI_GetMeasurement.cgi", (HttpRequest request) => 
{
    var inputId = request.Query["input_id"];

    var random = new Random();
    var angle = random.Next(0, 250);
    var speed = random.Next(0, 50);

    var csv = $"A,{angle},0{speed:00}.95,N,00,00";

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
