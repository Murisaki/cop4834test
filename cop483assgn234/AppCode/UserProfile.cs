using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cop483assgn234.AppCode;
using cop483assgn234;

namespace cop483assgn234.AppCode
{
    public class UserProfile : XmlPropertyObject
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string UserName { get { return Name; } set { Name = value; } }
        public string EMail { get { return Description; } set { Description = value; } }
    }
}