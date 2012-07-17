using System;
using System.Web.Mvc;
using Marina.Store.Web.Infrastructure.Commands;

namespace Marina.Store.Web.Controllers
{
    /// <summary>
    /// Базовый контроллер магазина
    /// </summary>
    public abstract class StoreController : Controller
    {
        public TR Run<T, TR>(Func<T, TR> what) where T : Command where TR : Result
        {
            var cmd = DependencyResolver.Current.GetService<T>();
            var result = what(cmd);
            return result;
        }
    }
}
