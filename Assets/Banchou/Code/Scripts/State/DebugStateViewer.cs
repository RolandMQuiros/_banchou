using System;
using UniRx;
using Zenject;
using Sirenix.OdinInspector;

namespace Banchou {
    public class DebugStateViewer : SerializedMonoBehaviour {
        [ReadOnly] public GameState CurrentState;

        [Inject]
        public void Construct(IObservable<GameState> observeState) {
            observeState.DistinctUntilChanged()
                .Subscribe(state => CurrentState = state);
        }
    }
}