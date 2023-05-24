using UniRx;

namespace Shun_Player
{
    public static class ParameterBuff
    {
        public static ReactiveProperty<int> hpLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> chargeDmgLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> speedLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> rodHpLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> rodDmgLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> shotSpeedLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> rodRangeLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> rodIntervalLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> rodDurationLevel { get; private set; } = new ReactiveProperty<int>();
        public static ReactiveProperty<int> rodAmountLevel { get; private set; } = new ReactiveProperty<int>();

        private static CompositeDisposable disposable = new CompositeDisposable();

        public static void Init()
        {
            hpLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.maxHp, hpLevel.Value); }).AddTo(disposable);
            chargeDmgLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.chargeDmg, chargeDmgLevel.Value); }).AddTo(disposable);
            speedLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.speed, speedLevel.Value); }).AddTo(disposable);
            rodHpLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.rodMaxHp, rodHpLevel.Value); }).AddTo(disposable);
            rodDmgLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.rodDmg, rodDmgLevel.Value); }).AddTo(disposable);
            shotSpeedLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.shotSpeed, shotSpeedLevel.Value); }).AddTo(disposable);
            rodRangeLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.rodRange, rodRangeLevel.Value); }).AddTo(disposable);
            rodIntervalLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.rodInterval, rodIntervalLevel.Value); }).AddTo(disposable);
            rodDurationLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.rodDuration, rodDurationLevel.Value); }).AddTo(disposable);
            rodAmountLevel.Subscribe(_ => { PlayerParameter.ChangeValue(Parameter.rodAmount, rodAmountLevel.Value); }).AddTo(disposable);
        }

        public static void Up(Parameter value)
        {
            
        }
    }
}

