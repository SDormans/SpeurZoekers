using System.Threading.Tasks;

namespace UploadToolGui.Dto.Interfaces
{
    public interface IBase
    {
        string Name { get; set; }
        object Id { get; }
        bool IsNew { get; }

        Task SaveDb();
        Task RemoveItem();
    }

    public interface IBase<ID> : IBase
    {
        public new ID Id { get; set; }
    }
}
