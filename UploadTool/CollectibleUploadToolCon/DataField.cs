using System.Collections.Generic;

namespace CollectibleUploadToolCon
{
    public class DataField
    {
        public delegate void CollectibleSetter(CollectibleWrapper collectible, string value, int row, List<OutputMessage> errors);

        public string ColumnName { get; set; }
        public CollectibleSetter Setter { get; set; }

        public DataField(string name, CollectibleSetter setter)
        {
            ColumnName = name;
            Setter = setter;
        }
    }
}
