using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LANUG.Models
{
    public partial class LANUGEntities
    {
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<ITimestampable>())
            {
                ITimestampable iTime = (entry.Entity as ITimestampable);
                DateTime now = DateTime.Now;
                iTime.SetModifiedTimestamp(now);
                if (entry.State == EntityState.Added) iTime.SetCreatedTimestamp(now);
            }

            return base.SaveChanges();
        }
    }
}