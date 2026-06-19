using Crolow.Designer.Common.Application;
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Data.Repositories
{
    public class PhotoDataManager<T> : DataManager<T> where T : IDataObject
    {
        public PhotoDataManager(DatabaseSettings context) : base(context, "Photos", "Photos")
        {
        }
    }



}
