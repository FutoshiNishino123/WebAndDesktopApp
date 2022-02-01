using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Data.Generators
{
    public class CurrentTimeGenerator : ValueGenerator<DateTime>
    {
        public override bool GeneratesTemporaryValues => false;

        public override DateTime Next(EntityEntry entry) => DateTime.Now;

        protected override object NextValue(EntityEntry entry) => DateTime.Now;
    }
}
