// Copyright © 2006 - 2012 Dieter Lunn
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
using System.Linq;
using System.Xml.Linq;
using XMPP.Registries;

namespace XMPP.Tags.VCardTemp
{
    public class Namespace
    {
        public const string XmlNamespace = "vcard-temp";

        public static readonly XName VCard = XName.Get("vCard", XmlNamespace);
        public static readonly XName Fn = XName.Get("FN", XmlNamespace);
        public static readonly XName Org = XName.Get("ORG", XmlNamespace);
        public static readonly XName OrgName = XName.Get("ORGNAME", XmlNamespace);
        public static readonly XName OrgUnit = XName.Get("ORGUNIT", XmlNamespace);
    }

    [XmppTag(typeof(Namespace), typeof(VCard))]
    public class VCard : Tag
    {
        public VCard() : base(Namespace.VCard)
        {
        }

        public VCard(XElement other) : base(other)
        {
        }

        public Fn Fn
        {
            get { return Elements<Fn>(Namespace.Fn).FirstOrDefault(); }
        }

        public Org Org
        {
            get { return Elements<Org>(Namespace.Org).FirstOrDefault(); }
        }
    }

    [XmppTag(typeof(Namespace), typeof(Fn))]
    public class Fn : Tag
    {
        public Fn() : base(Namespace.Fn)
        {
        }

        public Fn(XElement other) : base(other)
        {
        }

        public string Value
        {
            get { return InnerElement.Value; }
            set { InnerElement.Value = value; }
        }
    }

    [XmppTag(typeof(Namespace), typeof(Org))]
    public class Org : Tag
    {
        public Org()
            : base(Namespace.Org)
        {
        }

        public Org(XElement other)
            : base(other)
        {
        }

        public IEnumerable<OrgName> OrgName
        {
            get { return Elements<OrgName>(Namespace.OrgName); }
        }
    }

    [XmppTag(typeof(Namespace), typeof(OrgName))]
    public class OrgName : Tag
    {
        public OrgName()
            : base(Namespace.OrgName)
        {
        }

        public OrgName(XElement other)
            : base(other)
        {
        }

        public IEnumerable<OrgUnit> OrgUnit
        {
            get { return Elements<OrgUnit>(Namespace.OrgUnit); }
        }

        public string Value
        {
            get { return InnerElement.Value; }
            set { InnerElement.Value = value; }
        }
    }

    [XmppTag(typeof(Namespace), typeof(OrgUnit))]
    public class OrgUnit : Tag
    {
        public OrgUnit()
            : base(Namespace.OrgUnit)
        {
        }

        public OrgUnit(XElement other)
            : base(other)
        {
        }

        public string Value
        {
            get { return InnerElement.Value; }
            set { InnerElement.Value = value; }
        }
    }
}

/*
<?xml version="1.0" encoding="utf-8"?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="X400">
    <xs:complexType />
  </xs:element>
  <xs:element name="WORK">
    <xs:complexType />
  </xs:element>
  <xs:element name="CLASS">
    <xs:complexType>
      <xs:choice>
        <xs:element ref="PUBLIC" />
        <xs:element ref="PRIVATE" />
        <xs:element ref="CONFIDENTIAL" />
      </xs:choice>
    </xs:complexType>
  </xs:element>
  <xs:element name="LOCALITY">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="INTL">
    <xs:complexType />
  </xs:element>
  <xs:element name="EXTADD">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="MODEM">
    <xs:complexType />
  </xs:element>
  <xs:element name="EXTVAL">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="NUMBER">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="CRED">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="LON">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="LINE">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="NICKNAME">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="PHOTO">
    <xs:complexType>
      <xs:choice>
        <xs:sequence>
          <xs:element ref="TYPE" />
          <xs:element ref="BINVAL" />
        </xs:sequence>
        <xs:element ref="EXTVAL" />
      </xs:choice>
    </xs:complexType>
  </xs:element>
  <xs:element name="ADR">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs="0" ref="HOME" />
        <xs:element minOccurs="0" ref="WORK" />
        <xs:element minOccurs="0" ref="POSTAL" />
        <xs:element minOccurs="0" ref="PARCEL" />
        <xs:choice minOccurs="0">
          <xs:element ref="DOM" />
          <xs:element ref="INTL" />
        </xs:choice>
        <xs:element minOccurs="0" ref="PREF" />
        <xs:element minOccurs="0" ref="POBOX" />
        <xs:element minOccurs="0" ref="EXTADD" />
        <xs:element minOccurs="0" ref="STREET" />
        <xs:element minOccurs="0" ref="LOCALITY" />
        <xs:element minOccurs="0" ref="REGION" />
        <xs:element minOccurs="0" ref="PCODE" />
        <xs:element minOccurs="0" ref="CTRY" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="PREF">
    <xs:complexType />
  </xs:element>
  <xs:element name="VIDEO">
    <xs:complexType />
  </xs:element>
  <xs:element name="PAGER">
    <xs:complexType />
  </xs:element>
  <xs:element name="PARCEL">
    <xs:complexType />
  </xs:element>
  <xs:element name="PREFIX">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="INTERNET">
    <xs:complexType />
  </xs:element>
  <xs:element name="PUBLIC">
    <xs:complexType />
  </xs:element>
  <xs:element name="BINVAL">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="MSG">
    <xs:complexType />
  </xs:element>
  <xs:element name="CTRY">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="LAT">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="GEO">
    <xs:complexType>
      <xs:sequence>
        <xs:element ref="LAT" />
        <xs:element ref="LON" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="USERID">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="ORGNAME">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="NOTE">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="REV">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="POBOX">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="BBS">
    <xs:complexType />
  </xs:element>
  <xs:element name="BDAY">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="URL">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="UID">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="SOUND">
    <xs:complexType>
      <xs:choice>
        <xs:element ref="PHONETIC" />
        <xs:element ref="BINVAL" />
        <xs:element ref="EXTVAL" />
      </xs:choice>
    </xs:complexType>
  </xs:element>
  <xs:element name="FAX">
    <xs:complexType />
  </xs:element>
  <xs:element name="vCard">
    <xs:complexType>
      <xs:sequence>
        <xs:sequence>
          <xs:element ref="VERSION" />
          <xs:element ref="FN" />
          <xs:element ref="N" />
        </xs:sequence>
        <xs:sequence minOccurs="0" maxOccurs="unbounded">
          <xs:element minOccurs="0" ref="NICKNAME" />
          <xs:element minOccurs="0" ref="PHOTO" />
          <xs:element minOccurs="0" ref="BDAY" />
          <xs:element minOccurs="0" ref="ADR" />
          <xs:element minOccurs="0" ref="LABEL" />
          <xs:element minOccurs="0" ref="TEL" />
          <xs:element minOccurs="0" ref="EMAIL" />
          <xs:element minOccurs="0" ref="JABBERID" />
          <xs:element minOccurs="0" ref="MAILER" />
          <xs:element minOccurs="0" ref="TZ" />
          <xs:element minOccurs="0" ref="GEO" />
          <xs:element minOccurs="0" ref="TITLE" />
          <xs:element minOccurs="0" ref="ROLE" />
          <xs:element minOccurs="0" ref="LOGO" />
          <xs:element minOccurs="0" ref="AGENT" />
          <xs:element minOccurs="0" ref="ORG" />
          <xs:element minOccurs="0" ref="CATEGORIES" />
          <xs:element minOccurs="0" ref="NOTE" />
          <xs:element minOccurs="0" ref="PRODID" />
          <xs:element minOccurs="0" ref="REV" />
          <xs:element minOccurs="0" ref="SORT-STRING" />
          <xs:element minOccurs="0" ref="SOUND" />
          <xs:element minOccurs="0" ref="UID" />
          <xs:element minOccurs="0" ref="URL" />
          <xs:element minOccurs="0" ref="CLASS" />
          <xs:element minOccurs="0" ref="KEY" />
          <xs:element minOccurs="0" ref="DESC" />
        </xs:sequence>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="ORG">
    <xs:complexType>
      <xs:sequence>
        <xs:element ref="ORGNAME" />
        <xs:element minOccurs="0" maxOccurs="unbounded" ref="ORGUNIT" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="HOME">
    <xs:complexType />
  </xs:element>
  <xs:element name="PRIVATE">
    <xs:complexType />
  </xs:element>
  <xs:element name="TZ">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="CONFIDENTIAL">
    <xs:complexType />
  </xs:element>
  <xs:element name="MAILER">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="VOICE">
    <xs:complexType />
  </xs:element>
  <xs:element name="PRODID">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="CELL">
    <xs:complexType />
  </xs:element>
  <xs:element name="CATEGORIES">
    <xs:complexType>
      <xs:choice>
        <xs:element maxOccurs="unbounded" ref="KEYWORD" />
      </xs:choice>
    </xs:complexType>
  </xs:element>
  <xs:element name="ISDN">
    <xs:complexType />
  </xs:element>
  <xs:element name="LABEL">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs="0" ref="HOME" />
        <xs:element minOccurs="0" ref="WORK" />
        <xs:element minOccurs="0" ref="POSTAL" />
        <xs:element minOccurs="0" ref="PARCEL" />
        <xs:choice minOccurs="0">
          <xs:element ref="DOM" />
          <xs:element ref="INTL" />
        </xs:choice>
        <xs:element minOccurs="0" ref="PREF" />
        <xs:element maxOccurs="unbounded" ref="LINE" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="FN">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="POSTAL">
    <xs:complexType />
  </xs:element>
  <xs:element name="LOGO">
    <xs:complexType>
      <xs:choice>
        <xs:sequence>
          <xs:element ref="TYPE" />
          <xs:element ref="BINVAL" />
        </xs:sequence>
        <xs:element ref="EXTVAL" />
      </xs:choice>
    </xs:complexType>
  </xs:element>
  <xs:element name="MIDDLE">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="PHONETIC">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="KEY">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs="0" ref="TYPE" />
        <xs:element ref="CRED" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="DOM">
    <xs:complexType />
  </xs:element>
  <xs:element name="ORGUNIT">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="N">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs="0" ref="FAMILY" />
        <xs:element minOccurs="0" ref="GIVEN" />
        <xs:element minOccurs="0" ref="MIDDLE" />
        <xs:element minOccurs="0" ref="PREFIX" />
        <xs:element minOccurs="0" ref="SUFFIX" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="REGION">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="EMAIL">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs="0" ref="HOME" />
        <xs:element minOccurs="0" ref="WORK" />
        <xs:element minOccurs="0" ref="INTERNET" />
        <xs:element minOccurs="0" ref="PREF" />
        <xs:element minOccurs="0" ref="X400" />
        <xs:element ref="USERID" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="JABBERID">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="PCS">
    <xs:complexType />
  </xs:element>
  <xs:element name="SORT-STRING">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="xCard">
    <xs:complexType>
      <xs:choice maxOccurs="unbounded">
        <xs:element ref="vCard" />
      </xs:choice>
    </xs:complexType>
  </xs:element>
  <xs:element name="VERSION">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="TYPE">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="SUFFIX">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="ROLE">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="TITLE">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="GIVEN">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="FAMILY">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="DESC">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="TEL">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs="0" ref="HOME" />
        <xs:element minOccurs="0" ref="WORK" />
        <xs:element minOccurs="0" ref="VOICE" />
        <xs:element minOccurs="0" ref="FAX" />
        <xs:element minOccurs="0" ref="PAGER" />
        <xs:element minOccurs="0" ref="MSG" />
        <xs:element minOccurs="0" ref="CELL" />
        <xs:element minOccurs="0" ref="VIDEO" />
        <xs:element minOccurs="0" ref="BBS" />
        <xs:element minOccurs="0" ref="MODEM" />
        <xs:element minOccurs="0" ref="ISDN" />
        <xs:element minOccurs="0" ref="PCS" />
        <xs:element minOccurs="0" ref="PREF" />
        <xs:element ref="NUMBER" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name="STREET">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="PCODE">
    <xs:complexType mixed="true" />
  </xs:element>
  <xs:element name="AGENT">
    <xs:complexType>
      <xs:choice>
        <xs:element ref="vCard" />
        <xs:element ref="EXTVAL" />
      </xs:choice>
    </xs:complexType>
  </xs:element>
  <xs:element name="KEYWORD">
    <xs:complexType mixed="true" />
  </xs:element>
</xs:schema>

*/