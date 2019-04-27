
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Bpmtk.Engine.Cfg
{
    public class GuidValueGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => true;

        protected override object NextValue(EntityEntry entry)
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}
