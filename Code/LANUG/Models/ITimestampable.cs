using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LANUG.Models
{
    public interface ITimestampable
    {
        void SetCreatedTimestamp(DateTime dtm);
        void SetModifiedTimestamp(DateTime dtm);
    }
}