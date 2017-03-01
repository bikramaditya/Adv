using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace HelliumClient
{
    public class TestStepReader
    {
        private TestCase tc;

        private XmlNode initNode;
        private XmlNode loopNode;

        public TestStepReader(TestCase tc)
        {
            this.tc = tc;

            createXMLDoc();

        }

        private void createXMLDoc()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.tc.XmlFileName);

            initNode = doc.SelectSingleNode("/testDefination/init");

            loopNode = doc.SelectSingleNode("/testDefination/loop");
        }

        public List<TestStep> readStepsFromXML(String xmlFile)
        {
            
            return null;
        }

        internal int getInitStepCount()
        {
            if (initNode == null)
            {
                return 0;
            }
            return initNode.ChildNodes.Count;
        }

        internal List<TestStep> getInitSteps()
        {
            return getStepsList(initNode);
        }
        internal List<TestStep> getLoopSteps()
        {
            return getStepsList(loopNode);
        }
        internal List<TestStep> getStepsList(XmlNode mainNode)
        {
            List<TestStep> steps_list = new List<TestStep>();

            if (mainNode != null && mainNode.ChildNodes.Count > 0)
            {
                foreach (XmlNode node in mainNode.ChildNodes)
                {

                    TestStep step = new TestStep();
                    String timeTowait;
                    String assert;
                    String asOccurance;
                    String clickOnText;
                    String occurance;
                    String xOffset;
                    String yOffset;
                    String clickOnAltText;
                    String altOccurance;
                    String xAltOffset;
                    String yAltOffset;
                    String textToType;
                    String isMarkerCollectionRequired;
                    String isClearCacheRequired;

                    try
                    {
                        step.scrollText = node["scroll"].InnerText;
                    }
                    catch (Exception)
                    {
                        step.scrollText = "";
                    }

                    try
                    {
                        String lines = node["scroll"].Attributes.GetNamedItem("lines").Value;
                        step.scrollLines = Int32.Parse(lines);
                    }
                    catch (Exception)
                    {
                        step.scrollLines = 0;
                    }

                    try
                    {
                        String occ = node["scroll"].Attributes.GetNamedItem("scrollOcc").Value;
                        step.scrollOcc = Int32.Parse(occ);
                    }
                    catch (Exception)
                    {
                        step.scrollOcc = 1;
                    }
                    


                    try
                    {
                        timeTowait = node["timeTowait"].InnerText;
                        step.timeTowait = Int32.Parse(timeTowait);
                    }
                    catch (Exception)
                    {
                        step.timeTowait = 1000;
                    }
                    
                    try
                    {
                        clickOnText = node["clickOnText"].InnerText;
                        step.text = clickOnText;
                    }
                    catch (Exception)
                    {
                        step.text = "";
                    }

                    try
                    {
                        assert = node["assert"].InnerText;
                        step.assert = assert;
                    }
                    catch (Exception)
                    {
                        step.assert = step.text;
                    }
                    
                    try
                    {
                        occurance = node["clickOnText"].Attributes.GetNamedItem("occurance").Value;
                        step.occurance = Int32.Parse(occurance);
                    }
                    catch (Exception)
                    {
                        step.occurance = 1;
                    }

                    try
                    {
                        asOccurance = node["assert"].Attributes.GetNamedItem("occurance").Value;
                        step.asOccurance = Int32.Parse(asOccurance);
                    }
                    catch (Exception)
                    {
                        step.asOccurance = step.occurance;
                    }

                    try
                    {
                        xOffset = node["clickOnText"].Attributes.GetNamedItem("xOffset").Value;
                        step.xOffset = Int32.Parse(xOffset);
                    }
                    catch (Exception)
                    {
                        step.xOffset = 0;
                    }

                    try
                    { 
                        yOffset = node["clickOnText"].Attributes.GetNamedItem("yOffset").Value;
                        step.yOffset = Int32.Parse(yOffset);
                    }
                    catch (Exception)
                    {
                        step.yOffset = 0;
                    }

                    try
                    { 
                        clickOnAltText = node["clickOnAltText"].InnerText;
                        step.altText = clickOnAltText;
                    }
                    catch (Exception)
                    {
                        step.altText = "";
                    }


                    try
                    {
                        altOccurance = node["clickOnAltText"].Attributes.GetNamedItem("occurance").Value;
                        step.altOccurance = Int32.Parse(altOccurance);
                    }
                    catch (Exception)
                    {
                        step.altOccurance = 1;
                    }

                    try
                    { 
                        xAltOffset = node["clickOnAltText"].Attributes.GetNamedItem("xOffset").Value;
                        step.xAltOffset = Int32.Parse(xAltOffset);
                    }
                    catch (Exception)
                    {
                        step.xAltOffset = 0;
                    }

                    try
                    {
                        yAltOffset = node["clickOnAltText"].Attributes.GetNamedItem("yOffset").Value;
                        step.yAltOffset = Int32.Parse(yAltOffset);
                    }
                    catch (Exception)
                    {
                        step.yAltOffset = 0;
                    }

                    try
                    { 
                        textToType = node["textToType"].InnerText;
                        step.textToType = textToType;
                    }
                    catch (Exception)
                    {
                        step.textToType = "";
                    }

                    try
                    { 
                        isMarkerCollectionRequired = node["isMarkerCollectionRequired"].InnerText;
                        step.isMarker = Boolean.Parse(isMarkerCollectionRequired);
                    }
                    catch (Exception)
                    {
                        step.isMarker = false;
                    }

                    try
                    {
                        String markerAssert = node["isMarkerCollectionRequired"].Attributes.GetNamedItem("assert").Value; ;
                        step.markerAssert = markerAssert;
                    }
                    catch (Exception)
                    {
                        step.markerAssert = "";
                    }

                    try
                    {
                        String markerAssertOcc = node["isMarkerCollectionRequired"].Attributes.GetNamedItem("occurance").Value; ;
                        step.markerAssertOcc = Int32.Parse(markerAssertOcc);
                    }
                    catch (Exception)
                    {
                        step.markerAssertOcc = 0;
                    }

                    try
                    { 
                        isClearCacheRequired = node["isClearCacheRequired"].InnerText;
                        step.isCold = Boolean.Parse(isClearCacheRequired);
                    }
                    catch (Exception)
                    {
                        step.isCold = false;
                    }


                    steps_list.Add(step);
                    
                }
            }

            return steps_list;
        }

        
    }
}
