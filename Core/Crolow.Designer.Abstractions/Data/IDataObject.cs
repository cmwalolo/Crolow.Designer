namespace Crolow.Designer.Common.Data
{

    public interface IDataObject
    {
        EditState EditState { get; set; }
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }

    }
}