using UniRx;

namespace Shun_Player
{
    public class ParameterBuff
    {
        public static ReactiveProperty<int> hpBuff { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> chargeDmgBuff { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> speedBuff { get; private set; } = new ReactiveProperty<int>();

        private static CompositeDisposable disposable = new CompositeDisposable();

        public static void Init()
        {
            hpBuff.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.hp, hpBuff.Value); }).AddTo(disposable);
            chargeDmgBuff.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.chargeDmg, chargeDmgBuff.Value); }).AddTo(disposable);
            speedBuff.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.speed, speedBuff.Value); }).AddTo(disposable);
        }

        public static void Up()
        {
            hpBuff.Value++;
        }
    }
}

