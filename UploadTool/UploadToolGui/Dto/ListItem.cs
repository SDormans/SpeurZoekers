namespace UploadToolGui.Dto
{
    public class ListItem
    {
        public ListItem(object id, string name)
        {
            Id = id;
            Name = name;
        }

        public object Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? "[ONBEKEND]";
        }
    }
}
