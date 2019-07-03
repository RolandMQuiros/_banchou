using System;
using System.Threading.Tasks;

using UniRx.Async;

namespace Redux
{
    public delegate void ActionsCreator<TState>(Dispatcher dispatcher, Func<TState> getState);
    public delegate Task AsyncActionsCreator<TState>(Dispatcher dispatcher, Func<TState> getState);
    public delegate UniTask UnityAsyncActionsCreator<TState>(Dispatcher dispatcher, Func<TState> getState);

    public static partial class Middlewares
    {
        public static Func<Dispatcher, Dispatcher> Thunk<TState>(IStore<TState> store) {
            return dispatch => action => {
                var actionsCreator = action as ActionsCreator<TState>;
                if (actionsCreator != null) {
                    actionsCreator(dispatch, store.GetState);
                }

                var asyncActionsCreator = action as AsyncActionsCreator<TState>;
                if (asyncActionsCreator != null) {
                    asyncActionsCreator(dispatch, store.GetState);
                }

                var uniTaskActionsCreator = action as UnityAsyncActionsCreator<TState>;
                if (uniTaskActionsCreator != null) {
                    uniTaskActionsCreator(dispatch, store.GetState);
                }

                return action;
            };
        }
    }
}