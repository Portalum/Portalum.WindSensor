using System.Xml.Serialization;

[XmlRoot(ElementName = "measurements")]
public class Measurements
{
    [XmlElement(ElementName = "measurement")]
    public Measurement Measurement { get; set; }
}
