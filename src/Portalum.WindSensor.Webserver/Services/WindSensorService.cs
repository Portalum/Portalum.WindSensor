using Nager.TcpClient;

namespace Portalum.WindSensor.Webserver.Services
{
    public class WindSensorService : IWindSensorService, IDisposable
    {
        private readonly GillWindSensorParser _gillWindSensorParser;
        private readonly TcpClient _tcpClient;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private GillWindSensorParseResult _lastResult;

        public WindSensorService(
            IConfiguration configuration,
            TcpClient tcpClient,
            GillWindSensorParser gillWindSensorParser)
        {
            this._tcpClient = tcpClient;
            this._gillWindSensorParser = gillWindSensorParser;
            this._cancellationTokenSource = new CancellationTokenSource();

            this._tcpClient.DataReceived += this.OnDataReceived;

            var rs485EthernetConverterIpAddress = configuration.GetValue<string>("WindSensor:Rs485Module:IpAddress");
            var rs485EthernetConverterPort = configuration.GetValue<int>("WindSensor:Rs485Module:Port");

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

            var result = this._gillWindSensorParser?.Parse(receivedData);
            if (result != null)
            {
                this._lastResult = result;
            }
        }
    }
}
