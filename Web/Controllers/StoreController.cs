using System;
using System.Web.Mvc;
using Marina.Store.Web.Commands;

namespace Marina.Store.Web.Controllers
{
    /// <summary>
    /// Базовый контроллер магазина
    /// </summary>
    public abstract class StoreController : Controller
    {
        /// <summary>
        /// var result = Using<MyCommand>().Execute( cmd=>cmd.MyAction(param1, param2) );
        /// if (ModelState.IsValid) { 
        ///     return View("Success", result); 
        /// }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public CommandWrapper<T> Using<T>() where T : Command
        {
            return new CommandWrapper<T>(DependencyResolver.Current.GetService<T>(), ModelState);
        }

        public class CommandWrapper<T> where T : Command
        {
            private readonly T _command;
            private readonly ModelStateDictionary _state;

            public CommandWrapper(T command, ModelStateDictionary modelState)
            {
                _command = command;
                _state = modelState;
            }

            public TResult Execute<TResult>(Func<T,TResult>  expr)
            {
                var r = expr(_command);
                HandleErrors(_command);
                return r;
            }

            public void Execute(Action<T> expr)
            {
                expr(_command);
                HandleErrors(_command);
            }

            private void HandleErrors(T command)
            {
                if (!command.HasErrors)
                {
                    return;
                }

                foreach (var error in command.Errors)
                {
                    _state.AddModelError(error.Key, error.Value);
                }
            }
        }
    }
}
