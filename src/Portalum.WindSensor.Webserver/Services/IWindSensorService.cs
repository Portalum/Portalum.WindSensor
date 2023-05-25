namespace Portalum.WindSensor.Webserver.Services
{
    public interface IWindSensorService
    {
        GillWindSensorParseResult GetLastValue();
    }
}
