using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class OutputSet : BaseElement
    {
        //private string[] dataOutputRefs;

        //private string[] optionalOutputRefs;

        //private string[] whileExecutingOutputRefs;

        //private string[] inputSetRefs;

        //private string name;
        public OutputSet()
        {
            this.DataOutputRefs = new List<string>();
            this.OptionalOutputRefs = new List<string>();
            this.WhileExecutingOutputRefs = new List<string>();
            this.InputSetRefs = new List<string>();
        }

        /// <remarks/>
        //[XmlElement("dataOutputRefs", DataType = "IDREF", Order = 0)]
        public ICollection<string> DataOutputRefs
        {
            get;
        }

        /// <remarks/>
        //[XmlElement("optionalOutputRefs", DataType = "IDREF", Order = 1)]
        public ICollection<string> OptionalOutputRefs
        {
            get;
        }

        /// <remarks/>
        //[XmlElement("whileExecutingOutputRefs", DataType = "IDREF", Order = 2)]
        public ICollection<string> WhileExecutingOutputRefs
        {
            get;
        }

        /// <remarks/>
        //[XmlElement("inputSetRefs", DataType = "IDREF", Order = 3)]
        public ICollection<string> InputSetRefs
        {
            get;
        }

        /// <remarks/>
        //[XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        //public override void ReadXml(XmlReader reader)
        //{
        //    reader.MoveToContent();

        //    this.Id = reader.GetAttribute("id");
        //    this.Name = reader.GetAttribute("name");

        //    if (reader.IsEmptyElement)
        //    {
        //        reader.Read();
        //        return;
        //    }

        //    reader.Read();

        //    while (!reader.EOF)
        //    {
        //        if (reader.IsStartElement())
        //        {
        //            switch (reader.LocalName)
        //            {
        //                case "dataOutputRefs":
        //                    reader.MoveToContent();
        //                    reader.Read();
        //                    var dataOutputRef = reader.ReadContentAsString();
        //                    this.DataOutputRefs.Add(dataOutputRef);
        //                    reader.ReadEndElement();
        //                    break;

        //                case "optionalOutputRefs":
        //                    reader.MoveToContent();
        //                    reader.Read();
        //                    var optionalOutputRef = reader.ReadContentAsString();
        //                    this.OptionalOutputRefs.Add(optionalOutputRef);
        //                    reader.ReadEndElement();
        //                    break;

        //                case "whileExecutingOutputRefs":
        //                    reader.MoveToContent();
        //                    reader.Read();
        //                    var whileExecutingOutputRef = reader.ReadContentAsString();
        //                    this.WhileExecutingOutputRefs.Add(whileExecutingOutputRef);
        //                    reader.ReadEndElement();
        //                    break;

        //                case "inputSetRefs":
        //                    reader.MoveToContent();
        //                    reader.Read();
        //                    var inputSetRef = reader.ReadContentAsString();
        //                    this.InputSetRefs.Add(inputSetRef);
        //                    reader.ReadEndElement();
        //                    break;

        //                default:
        //                    reader.Skip();
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            reader.Read();
        //            break;
        //        }
        //    }
        //}

        //public override void WriteXml(XmlWriter writer)
        //{
        //    writer.WriteStartElement("outputSet");

        //    writer.WriteAttributeString("id", this.Id);
        //    writer.WriteAttributeString("name", this.Name);

        //    foreach (var item in this.DataOutputRefs)
        //        writer.WriteElementString("dataOutputRefs", item);

        //    foreach (var item in this.OptionalOutputRefs)
        //        writer.WriteElementString("optionalOutputRefs", item);

        //    foreach (var item in this.WhileExecutingOutputRefs)
        //        writer.WriteElementString("whileExecutingOutputRefs", item);

        //    foreach (var item in this.InputSetRefs)
        //        writer.WriteElementString("inputSetRefs", item);

        //    writer.WriteEndElement();
        //}
    }
}
