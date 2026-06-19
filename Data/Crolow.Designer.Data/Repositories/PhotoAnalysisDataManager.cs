using Crolow.Designer.Common.Application;
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Data.Repositories
{
    public class PhotoAnalysisDataManager<T> : DataManager<T> where T : IDataObject
    {
        public PhotoAnalysisDataManager(DatabaseSettings context) : base(context, "Photos", "PhotoAnalysis")
        {
        }
    }



}
