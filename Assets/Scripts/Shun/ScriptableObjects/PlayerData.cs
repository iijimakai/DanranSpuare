using UnityEngine;

namespace Shun_Constants
{
    [CreateAssetMenu(menuName = "Datas/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        /// <summary>
        /// キャラクターの種類
        /// </summary>
        [field: SerializeField, Header("キャラクターの種類")] public CharacterType characterType { get; private set; }

        /// <summary>
        /// 被弾後の無敵時間
        /// </summary>
        [field: SerializeField, Header("被弾後の無敵時間(秒)")] public float starTime { get; private set; }

        /// <summary>
        /// ダッシュ距離
        /// </summary>
        [field: SerializeField, Header("ダッシュの距離")] public float dashRange { get; private set; }

        /// <summary>
        /// ダッシュにかかる時間
        /// </summary>
        [field: SerializeField, Header("ダッシュにかかる時間")] public float dashTime { get; private set; }

        /// <summary>
        /// ダッシュクールタイム
        /// </summary>
        [field: SerializeField, Header("ダッシュのクールタイム")] public float dashCoolTime { get; private set; }

        /// <summary>
        /// 杖の最大ストック数
        /// </summary>
        [field: SerializeField, Header("杖の最大ストック数(個)")] public float rodStock { get; private set; }

        /// <summary>
        /// 杖が回復するまでの時間
        /// </summary>
        [field: SerializeField, Header("杖が回復するまでの時間(秒)")] public float rodRecastTime { get; private set; }

        /// <summary>
        /// 杖設置のクールタイム
        /// </summary>
        [field: SerializeField, Header("杖設置のクールタイム(秒)")] public float rodSetCoolTime { get; private set; }

        /// <summary>
        /// 最大チャージ距離
        /// </summary>
        [field: SerializeField, Header("最大チャージ距離(m)")] public float chargeMax { get; private set; }

        /// <summary>
        /// 体力
        /// </summary>
        [field: SerializeField, Header("体力")] public float hp { get; private set; }
        /// <summary>
        /// 体力変化量
        /// </summary>
        [field: SerializeField, Header("体力変化量")] public float hpBuff { get; private set; }

        /// <summary>
        /// チャージ攻撃ダメージ
        /// </summary>
        [field: SerializeField, Header("チャージ攻撃ダメージ")] public float chargeDmg { get; private set; }
        /// <summary>
        /// チャージ攻撃ダメージ変化量
        /// </summary>
        [field: SerializeField, Header("チャージ攻撃ダメージ変化量")] public float chargeDmgBuff { get; private set; }
        /// <summary>
        /// チャージ攻撃継続時間
        /// </summary>
        [field: SerializeField, Header("チャージ攻撃継続時間(秒)")] public float waveTime { get; private set; }

        /// <summary>
        /// 移動速度
        /// </summary>
        [field: SerializeField, Header("移動速度(m/s)")] public float speed { get; private set; }
        /// <summary>
        /// 移動速度変化量
        /// </summary>
        [field: SerializeField, Header("移動速度変化量")] public float speedBuff { get; private set; }

        /// <summary>
        /// 杖HP
        /// </summary>
        [field: SerializeField, Header("杖HP")] public float rodHp { get; private set; }
        /// <summary>
        /// 杖HP変化量
        /// </summary>
        [field: SerializeField, Header("杖HP変化量")] public float rodHpBuff { get; private set; }

        /// <summary>
        /// 杖ダメージ
        /// </summary>
        [field: SerializeField, Header("杖ダメージ")] public float rodDmg { get; private set; }
        /// <summary>
        /// 杖ダメージ変化量
        /// </summary>
        [field: SerializeField, Header("杖ダメージ変化量")] public float rodDmgBuff { get; private set; }

        /// <summary>
        /// 杖攻撃貫通数
        /// </summary>
        [field: SerializeField, Header("杖攻撃貫通数")] public int penetrateCount { get; private set; }

        /// <summary>
        /// 杖攻撃弾速
        /// </summary>
        [field: SerializeField, Header("杖攻撃弾速(m/s)")] public float shotSpeed { get; private set; }
        /// <summary>
        /// 杖攻撃弾速変化量
        /// </summary>
        [field: SerializeField, Header("杖攻撃弾速変化量")] public float shotSpeedBuff { get; private set; }

        /// <summary>
        /// 杖範囲
        /// </summary>
        [field: SerializeField, Header("杖範囲(m)")] public float rodRange { get; private set; }
        /// <summary>
        /// 杖範囲変化量
        /// </summary>
        [field: SerializeField, Header("杖範囲変化量")] public float rodRangeBuff { get; private set; }

        /// <summary>
        /// 杖攻撃クールタイム
        /// </summary>
        [field: SerializeField, Header("杖攻撃クールタイム(秒)")] public float rodInterval { get; private set; }
        /// <summary>
        /// 杖攻撃クールタイム変化量
        /// </summary>
        [field: SerializeField, Header("杖攻撃クールタイム変化量")] public float rodIntervalBuff { get; private set; }

        /// <summary>
        /// 杖攻撃持続時間
        /// </summary>
        [field: SerializeField, Header("杖攻撃持続時間(秒)")] public float rodDuration { get; private set; }
        /// <summary>
        /// 杖攻撃持続時間変化量
        /// </summary>
        [field: SerializeField, Header("杖攻撃持続時間変化量")] public float rodDurationBuff { get; private set; }

        /// <summary>
        /// 杖攻撃弾数
        /// </summary>
        [field: SerializeField, Header("杖攻撃弾数(個)")] public float rodAmount { get; private set; }
        /// <summary>
        /// 杖攻撃弾数変化量
        /// </summary>
        [field: SerializeField, Header("杖攻撃弾数変化量")] public float rodAmountBuff { get; private set; }
    }
}
