namespace Portalum.WindSensor.Helpers
{
    public static class ChecksumHelper
    {
        public static byte CalculateXorChecksum(Span<byte> data)
        {
            byte checksum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                checksum ^= data[i];
            }

            return checksum;
        }
    }
}
