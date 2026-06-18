using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Sessions.Selections;

namespace Crolow.Designer.UI
{
    public class RuntimeController
    {
        public static DesignerRuntime Runtime = null;
        public SelectionRegistry Selections { get; set; }

        public RuntimeController()
        {
            Runtime = new DesignerRuntime();
            Selections = new SelectionRegistry();
        }
    }
}
