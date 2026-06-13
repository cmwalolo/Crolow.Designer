namespace Crolow.Designer.Abstractions
{
    public interface ICommandResult<T>
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T Result { get; set; }
    }
}
