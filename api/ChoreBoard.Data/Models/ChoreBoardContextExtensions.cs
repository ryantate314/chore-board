using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Models
{
    public partial class ChoreBoardContext
    {

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDefinition>(x =>
            {
                x.HasAlternateKey(y => y.Uuid);
                //x.Property(x => x.Uuid)
                //    .ValueGeneratedOnAdd()
                //    .Metadata
                //        .SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);

                //x.Property(x => x.CreatedAt)
                //    .ValueGeneratedOnAdd()
                //    .Metadata
                //        .SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            });

            modelBuilder.Entity<TaskSchedule>(x =>
            {
                //x.Property(x => x.CreatedAt)
                //    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TaskInstance>(x =>
            {
                x.HasAlternateKey(y => y.Uuid);
                //x.Property(x => x.Uuid)
                //    .ValueGeneratedOnAdd()
                //    .Metadata
                //        .SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);
            });

            modelBuilder.Entity<FamilyMember>(x =>
            {
                x.HasAlternateKey(y => y.Uuid);
                //x.Property(x => x.Uuid)
                //    .ValueGeneratedOnAdd()
                //    .Metadata
                //        .SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);

                //x.Property(x => x.CreatedAt)
                //    .ValueGeneratedOnAdd()
                //    .Metadata
                //        .SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            });
        }

    }
}
