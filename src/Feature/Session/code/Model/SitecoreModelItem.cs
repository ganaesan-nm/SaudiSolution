using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaudiA.Foundation.Session.Model
{
    public class SitecoreModelItem
    {
        public string ItemName { get; set; }

      //  public string FieldValue { get; set; }

        public List<FieldWithType> FieldList { get; set; }

        public List<FieldWithType> TreeListField { get; set; }
    }

    public class FieldWithType
    {
        public string FieldName { get; set; }

        public FieldType Type { get; set; }

        public bool IsShared { get; set; }

        public bool IsVersioned { get; set; }
    }


    public enum FieldType
    {
        SingleLine = 1,
        MultiLine = 2,
        Image = 3,
        Generallink = 4,
        Boolean = 5,
        MultiList = 6
    }

}
