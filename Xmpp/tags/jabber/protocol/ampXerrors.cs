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
using System.Xml.Linq;
using XMPP.Registries;

namespace XMPP.Tags.Jabber.Protocol.AmpXerrors
{
    public class Namespace
    {
        public const string XmlNamespace = "http://jabber.org/protocol/amp#errors";

        public static readonly XName FailedRules = XName.Get("failed-rules", XmlNamespace);
        public static readonly XName Rule = XName.Get("rule", XmlNamespace);
    }

    [XmppTag(typeof(Namespace), typeof(FailedRules))]
    public class FailedRules : Tag
    {
        public FailedRules() : base(Namespace.FailedRules)
        {
        }

        public FailedRules(XElement other) : base(other)
        {
        }

        public IEnumerable<Rule> RuleElements
        {
            get { return Elements<Rule>(Namespace.Rule); }
        }
    }

    [XmppTag(typeof(Namespace), typeof(Rule))]
    public class Rule : Tag
    {
        public Rule() : base(Namespace.Rule)
        {
        }

        public Rule(XElement other) : base(other)
        {
        }

        public string ActionAttr
        {
            get { return (string)GetAttributeValue("action"); }
            set { InnerElement.SetAttributeValue("action", value); }
        }

        public string ConditionAttr
        {
            get { return (string)GetAttributeValue("condition"); }
            set { InnerElement.SetAttributeValue("condition", value); }
        }

        public string ValueAttr
        {
            get { return (string)GetAttributeValue("value"); }
            set { InnerElement.SetAttributeValue("value", value); }
        }
    }
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/amp#errors'
    xmlns='http://jabber.org/protocol/amp#errors'
    elementFormDefault='qualified'>
 
  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0079: http://www.xmpp.org/extensions/xep-0079.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='failed-rules'>
    <xs:complexType>
      <xs:sequence>
        <xs:element ref='rule' minOccurs='1' maxOccurs='unbounded'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:element name='rule'>
    <xs:complexType>
      <xs:attribute name='action' use='required' type='xs:NCName'/>
      <xs:attribute name='condition' use='required' type='xs:NCName'/>
      <xs:attribute name='value' use='required' type='xs:string'/>
    </xs:complexType>
  </xs:element>

</xs:schema>

*/