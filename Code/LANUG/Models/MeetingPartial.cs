using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LANUG.Models
{
    public partial class Meeting : ITimestampable
    {
        void ITimestampable.SetCreatedTimestamp(DateTime dtm)
        {
            this.Created = dtm;
        }
        void ITimestampable.SetModifiedTimestamp(DateTime dtm)
        {
            this.Modified = dtm;
        }
    }
}