using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace JellyFish
{
    [DataContract]
    public class Response
    {
        [DataMember(Name = "language")]
        public string Language { get; set; }
        [DataMember(Name = "textAngle")]
        public string TextAngle { get; set; }
        [DataMember(Name = "orientation")]
        public String Orientation { get; set; }
        [DataMember(Name = "regions")]
        public Region[] regions { get; set; }
    }


    [DataContract]
    public class Region
    {
        [DataMember(Name = "boundingBox")]
        public String boundingBox { get; set;}

        [DataMember(Name = "lines")]
        public Line[] lines { get; set; }
    }

    [DataContract]
    public class Point
    {
        /// <summary>
        /// Latitude,Longitude
        /// </summary>
        [DataMember(Name = "coordinates")]
        public double[] Coordinates { get; set; }
    }



    [DataContract]
    public class Line
    {
        [DataMember(Name = "boundingBox")]
        public String boundingBox { get; set; }
        [DataMember(Name = "words")]
        public Word[] words { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
    public class Location
    {
        [DataMember(Name = "boundingBox")]
        public String BoundingBox { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "point")]
        public Point Point { get; set; }
        [DataMember(Name = "entityType")]
        public string EntityType { get; set; }
       
    }

    [DataContract]
    public class Word
    {
        [DataMember(Name = "boundingBox")]
        public String boundingBox { get; set; }
        
        [DataMember(Name = "text")]
        public string text { get; set; }
    }
}
