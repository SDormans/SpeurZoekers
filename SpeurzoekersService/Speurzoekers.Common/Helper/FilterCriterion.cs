using Speurzoekers.Common.Helper.Enums;
using System;

namespace Speurzoekers.Common.Helper
{
    public class FilterCriterion
    {
        public string PropertyName { get; set; }
        public Type Type { get; set; }
        public ComparisonTypes Comparison { get; set; }
        public object Value { get; set; }
    }
}
