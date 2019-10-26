using System;
using Redux;

namespace Banchou {
    public interface IMiddleware<TState> {
        Middleware<TState> Middleware { get; }
    }
}