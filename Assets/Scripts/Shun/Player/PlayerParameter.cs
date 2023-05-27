using Shun_Constants;

namespace Shun_Player
{
    public enum Parameter
    {
        maxHp = 0,
        chargeDmg,
        speed,
        rodMaxHp,
        rodDmg,
        shotSpeed,
        rodRange,
        rodInterval,
        rodDuration,
        rodAmount
    }

    public class PlayerParameter
    {
        private static PlayerData data;
        public static CharacterType characterType { get; private set; }
        public static float rodStock { get; private set; }
        public static float rodRecastTime { get; private set; }
        public static float rodSetCoolTime { get; private set; }
        public static float chargeMax { get; private set; }
        public static float maxHp { get; private set; }
        public static float chargeDmg { get; private set; }
        public static float speed { get; private set; }
        public static float maxRodHp { get; private set; }
        public static float rodDmg { get; private set; }
        public static float shotSpeed { get; private set; }
        public static float rodRange { get; private set; }
        public static float rodInterval { get; private set; }
        public static float rodDuration { get; private set; }
        public static float rodAmount { get; private set; }


        public static void Init(PlayerData _data)
        {
            data = _data;
            SetData(_data);
        }

        private static void SetData(PlayerData _data)
        {
            characterType = _data.characterType;
            rodStock = _data.rodStock;
            rodRecastTime = _data.rodRecastTime;
            rodSetCoolTime = _data.rodSetCoolTime;
            chargeMax = _data.chargeMax;
            maxHp = _data.hp;
            chargeDmg = _data.chargeDmg;
            speed = _data.speed;
            maxRodHp = _data.rodHp;
            rodDmg = _data.rodDmg;
            shotSpeed = _data.shotSpeed;
            rodRange = _data.rodRange;
            rodInterval = _data.rodInterval;
            rodDuration = _data.rodDuration;
            rodAmount = _data.rodAmount;
        }

        public static void ChangeValue(Parameter parameter, int buffLevel)
        {
            switch (parameter)
            {
                case Parameter.maxHp:
                    maxHp = data.hp + data.hp * data.hpBuff * buffLevel;
                    break;
                case Parameter.chargeDmg:
                    chargeDmg = data.chargeDmg + data.chargeDmg * data.chargeDmgBuff * buffLevel;
                    break;
                case Parameter.speed:
                    speed = data.speed + data.speed * data.speedBuff * buffLevel;
                    break;
                case Parameter.rodMaxHp:
                    maxRodHp = data.rodHp + data.rodHp * data.rodHpBuff * buffLevel;
                    break;
                case Parameter.rodDmg:
                    rodDmg = data.rodDmg + data.rodDmg * data.rodDmgBuff * buffLevel;
                    break;
                case Parameter.shotSpeed:
                    shotSpeed = data.shotSpeed + data.shotSpeed * data.shotSpeedBuff * buffLevel;
                    break;
                case Parameter.rodRange:
                    rodRange = data.rodRange + data.rodRange * data.rodRangeBuff * buffLevel;
                    break;
                case Parameter.rodInterval:
                    rodInterval = data.rodInterval + data.rodInterval * data.rodIntervalBuff * buffLevel;
                    break;
                case Parameter.rodDuration:
                    rodDuration = data.rodDuration + data.rodDuration * data.rodDurationBuff * buffLevel;
                    break;
                case Parameter.rodAmount:
                    rodAmount = data.rodAmount + data.rodAmount * data.rodAmountBuff * buffLevel;
                    break;
                default:
                    break;
            }
        }

    }
}

