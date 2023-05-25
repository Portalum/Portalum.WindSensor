using Nager.TcpClient;

namespace Portalum.WindSensor.Webserver.Services
{
    public class WindSensorService : IWindSensorService, IDisposable
    {
        private readonly GillWindSensorParser _gillWindSensorParser;
        private readonly TcpClient _tcpClient;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private GillWindSensorParseResult _lastResult;

        public WindSensorService(GillWindSensorParser gillWindSensorParser)
        {
            //TODO: Add logic read config from appsettings

            var rs485EthernetConverterIpAddress = "10.14.20.19";
            var rs485EthernetConverterPort = 8005;

            this._gillWindSensorParser = gillWindSensorParser;

            this._cancellationTokenSource = new CancellationTokenSource();
            this._tcpClient = new TcpClient();
            this._tcpClient.DataReceived += this.OnDataReceived;

            _ = Task.Run(async () => await this._tcpClient.ConnectAsync(
                rs485EthernetConverterIpAddress,
                rs485EthernetConverterPort,
                this._cancellationTokenSource.Token));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this._cancellationTokenSource?.Cancel();
            this._cancellationTokenSource?.Dispose();

            if (this._tcpClient != null)
            {
                this._tcpClient.Disconnect();
                this._tcpClient.DataReceived -= this.OnDataReceived;
            }
        }

        public GillWindSensorParseResult GetLastValue()
        {
            return this._lastResult;
        }

        private void OnDataReceived(byte[] receivedData)
        {
            if (receivedData == null)
            {
                return;
            }

            this._lastResult = this._gillWindSensorParser?.Parse(receivedData);
        }
    }
}
