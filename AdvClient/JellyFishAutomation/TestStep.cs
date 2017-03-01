using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelliumClient
{
    public class TestStep
    {
        public int timeTowait;
        public String assert;
        public int asOccurance;
        public String text;
        public int occurance;
        public String altText;
        public int altOccurance;
        public int xOffset;
        public int yOffset;
        public int xAltOffset;
        public int yAltOffset;
        public String textToType;
        public bool isMarker;
        public String markerAssert;
        public int markerAssertOcc;
        public bool isCold;
        public String scrollText;
        public int scrollLines;
        public int scrollOcc;

        public override string ToString()
        {
            return 
                " timeTowait: " + timeTowait +
                " text: " + text +
                " altText: " + altText +
                " xOffset: " + xOffset +
                " yOffset: " + yOffset +
                " xAltOffset: " + xAltOffset +
                " yAltOffset: " + yAltOffset +
                " textToType: " + textToType +
                " isMarker: " + isMarker +
                " isCold: " + isCold;
        }

    }
}
