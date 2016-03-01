using System.Runtime.Serialization;

namespace Akumina.WebParts.DocumentSummaryList
{
    /// <summary>
    ///     Used only to JSON serialize the html fragments needed to drive the banner. (Public only so that the serializer can
    ///     access it.)
    /// </summary>
    [DataContract]
    public class Templates
    {
        public Templates()
        {
            ControlTemplate = "";
            ItemTemplate = "";
        }

        [DataMember]
        public string ControlTemplate { get; set; }

        [DataMember]
        public string ItemTemplate { get; set; }
    }
}