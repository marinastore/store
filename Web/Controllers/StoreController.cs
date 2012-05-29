using System.Web.Mvc;
using Marina.Store.Web.Commands;

namespace Marina.Store.Web.Controllers
{
    /// <summary>
    /// Базовый контроллер магазина
    /// </summary>
    public abstract class StoreController : Controller
    {
        public T Using<T>() where T : Command
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}
