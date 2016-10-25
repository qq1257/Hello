// Copyright ?2006 - 2012 Dieter Lunn
// Modified 2012 Paul Freund ( freund.paul@lvl3.org )
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
// You should have received a copy of the GNU Lesser General Public License along
// with this library; if not, write to the Free Software Foundation, Inc., 59
// Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System.Xml.Linq;
using XMPP.Registries;

namespace XMPP.Tags.Xmpp.Avatar.Metadata
{
    public class Namespace
    {
        public const string XmlNamespace = "urn:xmpp:avatar:metadata";

        public static readonly XName Metadata = XName.Get("metadata", XmlNamespace);
        public static readonly XName Info = XName.Get("info", XmlNamespace);
        public static readonly XName Pointer = XName.Get("pointer", XmlNamespace);
        public static readonly XName Stop = XName.Get("stop", XmlNamespace);
    }

    [XmppTag(typeof(Namespace), typeof(Metadata))]
    public class Metadata : Tag
    {
        public Metadata() : base(Namespace.Metadata)
        {
        }

        public Metadata(XElement other)
            : base(other)
        {
        }

        public Info Info
        {
            get { return Element<Info>(Namespace.Info); }
        }

        public Pointer Pointer
        {
            get { return Element<Pointer>(Namespace.Pointer); }
        }

        public Stop Stop
        {
            get { return Element<Stop>(Namespace.Stop); }
        }
    }

    [XmppTag(typeof(Namespace), typeof(Info))]
    public class Info : Tag
    {
        public Info() : base(Namespace.Info)
        {
        }

        public Info(XElement other) : base(other)
        {
        }

        public string BytesAttr
        {
            get { return (string)GetAttributeValue("bytes"); }
            set { InnerElement.SetAttributeValue("bytes", value); }
        }

        public string HeightAttr
        {
            get { return (string)GetAttributeValue("height"); }
            set { InnerElement.SetAttributeValue("height", value); }
        }

        public string IdAttr
        {
            get { return (string)GetAttributeValue("id"); }
            set { InnerElement.SetAttributeValue("id", value); }
        }

        public string TypeAttr
        {
            get { return (string)GetAttributeValue("type"); }
            set { InnerElement.SetAttributeValue("type", value); }
        }

        public string UrlAttr
        {
            get { return (string)GetAttributeValue("url"); }
            set { InnerElement.SetAttributeValue("url", value); }
        }

        public string WidthAttr
        {
            get { return (string)GetAttributeValue("width"); }
            set { InnerElement.SetAttributeValue("width", value); }
        }
    }

    [XmppTag(typeof(Namespace), typeof(Pointer))]
    public class Pointer : Tag
    {
        public Pointer() : base(Namespace.Pointer)
        {
        }

        public Pointer(XElement other) : base(other)
        {
        }
    }

    [XmppTag(typeof(Namespace), typeof(Stop))]
    public class Stop : Tag
    {
        public Stop() : base(Namespace.Stop)
        {
        }

        public Stop(XElement other) : base(other)
        {
        }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='urn:xmpp:avatar:metadata'
    xmlns='urn:xmpp:avatar:metadata'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0084: http://www.xmpp.org/extensions/xep-0084.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='metadata'>
    <xs:complexType>
      <xs:choice>
        <xs:sequence minOccurs='0' maxOccurs='1'>
          <xs:element ref='info' minOccurs='1' maxOccurs='unbounded'/>
          <xs:element ref='pointer' minOccurs='0' maxOccurs='unbounded'/>
        </xs:sequence>
        <xs:element name='stop' type='empty'/>
      </xs:choice>
    </xs:complexType>
  </xs:element>

  <xs:element name='info'>
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base='empty'>
          <xs:attribute name='bytes' type='xs:unsignedShort' use='required'/>
          <xs:attribute name='height' type='xs:unsignedByte' use='optional'/>
          <xs:attribute name='id' type='xs:string' use='required'/>
          <xs:attribute name='type' type='xs:string' use='required'/>
          <xs:attribute name='url' type='xs:anyURI' use='optional'/>
          <xs:attribute name='width' type='xs:unsignedByte' use='optional'/>
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>

  <xs:element name='pointer'>
    <xs:complexType>
      <xs:sequence>
        <xs:any namespace='##other'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:simpleType name='empty'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value=''/>
    </xs:restriction>
  </xs:simpleType>

</xs:schema>

*/