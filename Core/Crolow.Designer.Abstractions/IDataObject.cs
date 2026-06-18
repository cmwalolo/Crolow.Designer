namespace Crolow.Designer.Core.Document
{
    public interface IDataObject
    {
        Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}