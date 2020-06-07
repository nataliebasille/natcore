using System.Threading.Tasks;

namespace Natcore.AspNet.Views
{
    public interface IViewRenderer
    {
        Task<string> RenderAsync<TModel>(string viewName, TModel model);
    }
}
