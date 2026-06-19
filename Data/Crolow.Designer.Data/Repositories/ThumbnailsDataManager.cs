using Crolow.Designer.Common.Application;
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Data.Repositories
{
    public class ThumbnailsDataManager<T> : DataManager<T> where T : IDataObject
    {
        public ThumbnailsDataManager(DatabaseSettings context) : base(context, "Photos", "Thumbnails")
        {
        }
    }
}
