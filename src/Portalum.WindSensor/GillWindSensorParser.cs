using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Portalum.WindSensor.Helpers;
using System.Globalization;
using System.Text;

namespace Portalum.WindSensor
{
    public class GillWindSensorParser
    {
        private readonly byte _dataStartByte = 0x02;
        private readonly byte _dataEndByte = 0x03;
        private readonly int _checksumByteCount = 2;
        private readonly byte[] _packageEndBytes = new byte[] { 0x0D, 0x0A };

        private readonly ILogger<GillWindSensorParser> _logger;

        public GillWindSensorParser(ILogger<GillWindSensorParser>? logger = default)
        {
            this._logger = logger ?? new NullLogger<GillWindSensorParser>();
        }

        public GillWindSensorParseResult? Parse(byte[] rawData)
        {
            if (rawData == null)
            {
                this._logger.LogError("rawData null");
                return null;
            }

            if (rawData.Length == 0 || rawData.Length < (this._checksumByteCount + this._packageEndBytes.Length + 1))
            {
                this._logger.LogError("rawData invalid length");
                return null;
            }

            var span = rawData.AsSpan(0, rawData.Length - this._packageEndBytes.Length);

            var startByte = span.Slice(0, 1);
            var endByteIndex = span.Length - 1 - this._checksumByteCount;
            var endByte = span.Slice(endByteIndex, 1);

            if (startByte[0] != this._dataStartByte || endByte[0] != this._dataEndByte)
            {
                this._logger.LogError("rawData corrupt package, start end byte corruption");
                return null;
            }

            var sensorData = span.Slice(1, endByteIndex - 1);

            var checksumData = span.Slice(endByteIndex + 1);
            var checksumHex = Encoding.ASCII.GetString(checksumData.ToArray());
            var calculatedChecksum = ChecksumHelper.CalculateXorChecksum(sensorData);

            if (checksumHex != calculatedChecksum.ToString("X2"))
            {
                this._logger.LogError("checksum invalid");
                return null;
            }

            var textData = Encoding.ASCII.GetString(sensorData);
            var textParts = textData.Split(',');

            if (textParts.Length != 6)
            {
                this._logger.LogError("wrong part count");
                return null;
            }

            var nodeAddress = textParts[0];
            var windDirection = textParts[1];
            var windSpeed = textParts[2];
            var units = textParts[3];
            var status = textParts[4];
            //var nothing = textParts[5];

            int.TryParse(windDirection, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out var tempWindDirection);
            double.TryParse(windSpeed, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out var tempWindSpeed);

            return new GillWindSensorParseResult
            {
                NodeAddress = nodeAddress,
                WindDirection = tempWindDirection,
                WindSpeed = tempWindSpeed,
                Units = units,
                Status = status
            };
        }


    }
}