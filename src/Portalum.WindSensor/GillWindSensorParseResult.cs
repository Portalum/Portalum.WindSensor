namespace Portalum.WindSensor
{
    public class GillWindSensorParseResult
    {
        /// <summary>
        /// A unique Identifier of the sensor
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// WindDirection
        /// </summary>
        /// <remarks>
        /// Indicated in degrees, from 0 to 359°, with respect to the WindSonic North marker.
        /// </remarks>
        public int WindDirection { get; set; }

        /// <summary>
        /// WindSpeed
        /// </summary>
        public double WindSpeed { get; set; }

        /// <summary>
        /// WindSpeed unit
        /// </summary>
        /// <remarks>
        /// Meters per second (default) M, Knots N, Miles per hour P, Kilometres per hour K, Feet per minute F
        /// </remarks>
        public string Units { get; set; }

        public string Status { get; set; }
    }
}
