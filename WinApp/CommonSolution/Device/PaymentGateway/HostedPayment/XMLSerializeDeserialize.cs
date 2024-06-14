using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Used for XML Serialize & Deserialize
    /// </summary>
    class XMLSerializeDeserialize<T>
    {
        StringBuilder sbData;
        StringWriter swWriter;
        XmlDocument xDoc;
        XmlSerializer xmlSerializer;
        public XMLSerializeDeserialize()
        {
            sbData = new StringBuilder();
        }
        public string SerializeData(T data)
        {
            string xml = null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                swWriter = new StringWriter(sbData);
                xmlSerializer.Serialize(swWriter, data);

                xml = swWriter.ToString();
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

                //Add Soap header details
                xml = AddSoapHeader(xml);
            }
            catch (Exception ex)
            {
                throw;
            }
            return xml;
        }
        public T DeserializeData(string dataXML)
        {
            try
            {
                xDoc = new XmlDocument();
                xDoc.LoadXml(dataXML);

                //Remove SOAP Header 
                StringReader strReader = new StringReader(xDoc.DocumentElement.FirstChild.InnerXml);
                xmlSerializer = new XmlSerializer(typeof(T));

                var xmlData = xmlSerializer.Deserialize(strReader);
                T deserializedXML = (T)xmlData;

                return deserializedXML;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string AddSoapHeader(string xmlString)
        {
            string xmlSoapHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n" +
                "<soap:Body>" + xmlString + "\r\n</soap:Body>" +
                "\r\n</soap:Envelope>";
            return xmlSoapHeader;
        }
    }
}
