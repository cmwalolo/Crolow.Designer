namespace Crolow.Designer.Common.Data
{
    public interface IDataTreeObject<T> : IDataObject where T : IDataObject
    {
        public List<T> Children { get; set; }
    }
}