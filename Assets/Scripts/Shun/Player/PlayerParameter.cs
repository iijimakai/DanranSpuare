using Shun_Constants;

namespace Shun_Player
{
    public enum Parameter
    {
        hp = 0,
        chargeDmg,
        speed,
        rodHp,
        rodDmg,
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
        public static float hp { get; private set; }
        public static float chargeDmg { get; private set; }
        public static float speed { get; private set; }
        public static float rodHp { get; private set; }
        public static float rodDmg { get; private set; }
        public static float rodRange { get; private set; }
        public static float rodInterval { get; private set; }
        public static float rodDuration { get; private set; }
        public static float rodAmount { get; private set; }


        public static void Init(PlayerData data)
        {
            PlayerParameter.data = data;
            SetData();
        }

        private static void SetData()
        {
            characterType = data.characterType;
            rodStock = data.rodStock;
            chargeMax = data.chargeMax;
            hp = data.hp;
            chargeDmg = data.chargeDmg;
            speed = data.speed;
            

        }

        public static void ChangeValue(Parameter parameter, int buffLevel)
        {
            switch (parameter)
            {
                case Parameter.hp:
                    hp = data.hp + data.hp * data.hpBuff * buffLevel;
                    break;
                case Parameter.chargeDmg:
                    chargeDmg = data.chargeDmg + data.chargeDmg * data.chargeDmgBuff * buffLevel;
                    break;
                case Parameter.speed:
                    speed = data.speed + data.speed * data.speedBuff * buffLevel;
                    break;
                case Parameter.rodHp:
                    rodHp = data.rodHp + data.rodHp * data.rodHpBuff * buffLevel;
                    break;
                case Parameter.rodDmg:
                    rodDmg = data.rodDmg + data.rodDmg * data.rodDmgBuff * buffLevel;
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

