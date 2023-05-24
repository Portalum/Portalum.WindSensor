using System.Xml.Serialization;

[XmlRoot(ElementName = "measurement")]
public class Measurement
{
    [XmlElement(ElementName = "sourceid")]
    public int Sourceid { get; set; }

    [XmlElement(ElementName = "sequencenum")]
    public int Sequencenum { get; set; }

    [XmlElement(ElementName = "localtime")]
    public Localtime Localtime { get; set; }

    [XmlElement(ElementName = "csv")]
    public string Csv { get; set; }

    [XmlElement(ElementName = "isvalid")]
    public int Isvalid { get; set; }

    [XmlElement(ElementName = "error")]
    public string Error { get; set; }
}
