using UniRx;
using UnityEngine;

namespace Shun_Player
{
    public class PlayerLevel
    {
        private ReactiveProperty<int> level = new ReactiveProperty<int>();
        private CompositeDisposable disposables = new CompositeDisposable();

        public void Init()
        {
            level.Value = 1;
            level.Subscribe(_ => { LevelUp(); }).AddTo(disposables);
        }

        private void LevelUp()
        {
            Debug.Log("Level " + level.Value);
        }

        public void SetLevel()
        {
            level.Value++;
        }
    }
}

