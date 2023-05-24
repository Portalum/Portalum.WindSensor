using System.Xml.Serialization;

[XmlRoot(ElementName = "localtime")]
public class Localtime
{
    [XmlElement(ElementName = "time")]
    public string Time { get; set; }

    [XmlElement(ElementName = "date")]
    public string Date { get; set; }
}
