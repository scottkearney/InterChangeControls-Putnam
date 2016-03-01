using System.Runtime.Serialization;

namespace Akumina.WebParts.Banner
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
            TileItemTemplate = "";
        }

        [DataMember]
        public string ControlTemplate { get; set; }

        [DataMember]
        public string ItemTemplate { get; set; }

        [DataMember]
        public string TileItemTemplate { get; set; }
    }
}