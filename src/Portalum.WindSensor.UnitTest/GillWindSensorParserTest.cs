namespace Portalum.WindSensor.UnitTest
{
    [TestClass]
    public class GillWindSensorParserTest
    {
        //Gill format– Polar, Continuous(Default format)
        [DataRow("02-51-2C-2C-30-30-30-2E-30-33-2C-4D-2C-30-30-2C-03-32-44-0D-0A")]
        [DataRow("02-51-2C-31-34-30-2C-30-30-30-2E-30-36-2C-4D-2C-30-30-2C-03-31-44-0D-0A")]
        [DataRow("02-51-2C-31-33-35-2C-30-30-30-2E-30-35-2C-4D-2C-30-30-2C-03-31-43-0D-0A")]
        [DataRow("02-51-2C-30-38-39-2C-30-30-32-2E-35-35-2C-4D-2C-30-30-2C-03-31-44-0D-0A")]
        [DataRow("02-51-2C-33-32-38-2C-30-30-30-2E-31-30-2C-4D-2C-30-30-2C-03-31-36-0D-0A")]
        [DataTestMethod]
        public void TestMethod1(string hexData)
        {
            var windSensorParser = new GillWindSensorParser();

            var data = ByteHelper.HexToByteArray(hexData);

            var parseResult = windSensorParser.Parse(data);
            Assert.IsNotNull(parseResult);
        }
    }
}