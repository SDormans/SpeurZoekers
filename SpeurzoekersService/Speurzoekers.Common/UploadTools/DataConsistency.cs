using System.Collections.Generic;
using System.Linq;

namespace Speurzoekers.Common.UploadTools
{
    public class DataConsistency
    {
        public enum ItemStatus
        {
            Action,
            Critical,
            Optional
        }

        public enum ValidationStatus
        {
            CriticalMissing,
            OptionalMissing,
            Complete
        }

        public ValidationStatus Consistency { get; set; }

        public IEnumerable<string> MissingData { get; set; }

        public override string ToString()
        {
            if (MissingData != null)
            {
                return string.Join("\n", MissingData
                        .Select(x => "* " + x));
            }
            // else
            return string.Empty;
        }
    }
}
