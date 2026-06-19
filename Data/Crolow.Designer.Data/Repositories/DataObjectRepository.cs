using Crolow.Designer.Common.Application;
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Data.Repositories
{
    public class DataObjectRepository : DataManager<DataObject>
    {
        public DataObjectRepository(DatabaseSettings context) : base(context, "Catalog", "Objects")
        {
        }
    }
}
