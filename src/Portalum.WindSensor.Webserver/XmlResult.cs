using System.Xml.Serialization;

namespace Portalum.WindSensor.Webserver
{
    public class XmlResult<T> : IResult
    {
        // Create the serializer that will actually perform the XML serialization
        private static readonly XmlSerializer Serializer = new(typeof(T));

        // The object to serialize
        private readonly T _result;

        public XmlResult(T result)
        {
            this._result = result;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            // NOTE: best practice would be to pull this, we'll look at this shortly
            using var memoryStream = new MemoryStream();

            // Serialize the object synchronously then rewind the stream
            Serializer.Serialize(memoryStream, this._result);
            memoryStream.Position = 0;

            httpContext.Response.ContentType = "application/xml";
            await memoryStream.CopyToAsync(httpContext.Response.Body);
        }
    }
}
