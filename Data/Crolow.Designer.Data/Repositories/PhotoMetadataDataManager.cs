using Crolow.Designer.Common.Application;
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Data.Repositories
{
    public class PhotoMetadataDataManager<T> : DataManager<T> where T : IDataObject
    {
        public PhotoMetadataDataManager(DatabaseSettings context) : base(context, "Photos", "PhotoMetadata")
        {

        }
    }
}
