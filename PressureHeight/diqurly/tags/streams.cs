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

using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using XMPP.Registries;
using XMPP.Tags.Jabber.Client;
using XMPP.Tags.XmppSasl;
using XMPP.Tags.XmppTls;

// ReSharper disable once CheckNamespace
namespace XMPP.Tags.Streams
{
    public class Namespace
    {
        public const string XmlNamespace = "http://etherx.jabber.org/streams";

        public static readonly XName Features = XName.Get("features", XmlNamespace);
        public static readonly XName Stream = XName.Get("stream", XmlNamespace);
        public static readonly XName Error = XName.Get("error", XmlNamespace);
    }

    [XmppTag(typeof(Namespace), typeof(Stream))]
    public class Stream : Tag
    {
        public Stream() : base(Namespace.Stream)
        {
        }

        public Stream(XElement other) : base(other)
        {
        }

        public string FromAttr
        {
            get { return (string)GetAttributeValue("from"); }
            set { InnerElement.SetAttributeValue("from", value); }
        }

        public string ToAttr
        {
            get { return (string)GetAttributeValue("to"); }
            set { InnerElement.SetAttributeValue("to", value); }
        }

        public string IdAttr
        {
            get { return (string)GetAttributeValue("id"); }
            set { InnerElement.SetAttributeValue("id", value); }
        }

        public string LangAttr
        {
            get { return (string)GetAttributeValue(XName.Get("lang", Xml.Namespace.XmlNamespace)); }
            set { InnerElement.SetAttributeValue(XName.Get("lang", Xml.Namespace.XmlNamespace), value); }
        }

        public string VersionAttr
        {
            get { return (string)GetAttributeValue("version"); }
            set { InnerElement.SetAttributeValue("version", value); }
        }

        public string XmlnsAttr
        {
            get { return (string)GetAttributeValue("xmlns"); }
            set { InnerElement.SetAttributeValue("xmlns", value); }
        }

        public Features Features
        {
            get { return Element<Features>(Namespace.Features); }
        }

        public IEnumerable<Message> BodyElements
        {
            get { return Elements<Message>(Jabber.Client.Namespace.Message); }
        }

        public IEnumerable<Presence> PresenceElements
        {
            get { return Elements<Presence>(Jabber.Client.Namespace.Presence); }
        }

        public IEnumerable<Iq> IqElements
        {
            get { return Elements<Iq>(Jabber.Client.Namespace.Iq); }
        }

        public IEnumerable<Error> ErrorElements
        {
            get { return Elements<Error>(Namespace.Error); }
        }

        public string StartTag
        {
            get
            {
                var sb = new StringBuilder("<");
                sb.Append(InnerElement.Name.LocalName);
                sb.Append(":");
                sb.Append(InnerElement.Name.LocalName);
                sb.Append(" xmlns");
                sb.Append(":");
                sb.Append(InnerElement.Name.LocalName);
                sb.Append("=\'");
                sb.Append(InnerElement.Name.NamespaceName);
                sb.Append("\'");

                foreach (XAttribute attr in InnerElement.Attributes())
                {
                    sb.Append(" ");
                    sb.Append(attr.Name.LocalName);
                    sb.Append("=\'");
                    sb.Append(attr.Value);
                    sb.Append("\'");
                }

                sb.Append(">");
                return sb.ToString();
            }
        }
    }

    [XmppTag(typeof(Namespace), typeof(Features))]
    public class Features : Tag
    {
        public Features() : base(Namespace.Features)
        {
        }

        public Features(XElement other) : base(other)
        {
        }

        public Mechanisms Mechanisms
        {
            get { return Element<Mechanisms>(XmppSasl.Namespace.Mechanisms); }
        }

        public StartTls StartTls
        {
            get { return Element<StartTls>(XmppTls.Namespace.StartTls); }
        }

        public Jabber.Reatures.Compress.Compression Compression
        {
            get
            {
                return Element<Jabber.Reatures.Compress.Compression>(Jabber.Reatures.Compress.Namespace.Compression);
            }
        }
    }

    [XmppTag(typeof(Namespace), typeof(Error))]
    public class Error : Tag
    {
        public Error() : base(Namespace.Error)
        {
        }

        public Error(XElement other) : base(other)
        {
        }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://etherx.jabber.org/streams'
    xmlns='http://etherx.jabber.org/streams'
    elementFormDefault='unqualified'>

  <xs:import namespace='jabber:client'
             schemaLocation='http://xmpp.org/schemas/jabber-client.xsd'/>
  <xs:import namespace='jabber:server'
             schemaLocation='http://xmpp.org/schemas/jabber-server.xsd'/>
  <xs:import namespace='urn:ietf:params:xml:ns:xmpp-sasl'
             schemaLocation='http://xmpp.org/schemas/sasl.xsd'/>
  <xs:import namespace='urn:ietf:params:xml:ns:xmpp-streams'
             schemaLocation='http://xmpp.org/schemas/streamerror.xsd'/>
  <xs:import namespace='urn:ietf:params:xml:ns:xmpp-tls'
             schemaLocation='http://xmpp.org/schemas/tls.xsd'/>
  <xs:import namespace='http://www.w3.org/XML/1998/namespace'
             schemaLocation='http://www.w3.org/2001/03/xml.xsd'/>

  <xs:element name='stream'>
    <xs:complexType>
      <xs:sequence xmlns:client='jabber:client'
                   xmlns:server='jabber:server'>
        <xs:element ref='features' 
                    minOccurs='0' 
                    maxOccurs='1'/>
        <xs:any namespace='urn:ietf:params:xml:ns:xmpp-tls'
                minOccurs='0'
                maxOccurs='1'/>
        <xs:any namespace='urn:ietf:params:xml:ns:xmpp-sasl'
                minOccurs='0'
                maxOccurs='1'/>
        <xs:any namespace='##other'
                minOccurs='0'
                maxOccurs='unbounded'
                processContents='lax'/>
        <xs:choice minOccurs='0' maxOccurs='1'>
          <xs:choice minOccurs='0' maxOccurs='unbounded'>
            <xs:element ref='client:message'/>
            <xs:element ref='client:presence'/>
            <xs:element ref='client:iq'/>
          </xs:choice>
          <xs:choice minOccurs='0' maxOccurs='unbounded'>
            <xs:element ref='server:message'/>
            <xs:element ref='server:presence'/>
            <xs:element ref='server:iq'/>
          </xs:choice>
        </xs:choice>
        <xs:element ref='error' minOccurs='0' maxOccurs='1'/>
      </xs:sequence>
      <xs:attribute name='from' type='xs:string' use='optional'/>
      <xs:attribute name='id' type='xs:string' use='optional'/>
      <xs:attribute name='to' type='xs:string' use='optional'/>
      <xs:attribute name='version' type='xs:decimal' use='optional'/>
      <xs:attribute ref='xml:lang' use='optional'/>
      <xs:anyAttribute namespace='##other' processContents='lax'/> 
    </xs:complexType>
  </xs:element>

  <xs:element name='features'>
    <xs:complexType>
      <xs:sequence>
        <xs:any namespace='##other'
                minOccurs='0'
                maxOccurs='unbounded'
                processContents='lax'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='error'>
    <xs:complexType>
      <xs:sequence  xmlns:err='urn:ietf:params:xml:ns:xmpp-streams'>
        <xs:group   ref='err:streamErrorGroup'/>
        <xs:element ref='err:text'
                    minOccurs='0'
                    maxOccurs='1'/>
        <xs:any     namespace='##other'
                    minOccurs='0'
                    maxOccurs='1'
                    processContents='lax'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/