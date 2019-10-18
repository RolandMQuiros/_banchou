using System;

namespace Banchou {
    public interface IStoreConnector {
        void Inject(IObservable<State> observeState, Func<object, object> dispatch);
    }
}
