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
        /// 杖の最大ストック数
        /// </summary>
        [field: SerializeField, Header("杖の最大ストック数")] public float rodStock { get; private set; }

        /// <summary>
        /// 杖が回復するまでの時間
        /// </summary>
        [field: SerializeField, Header("杖が回復するまでの時間")] public float rodRecastTime { get; private set; }

        /// <summary>
        /// 杖設置のクールタイム
        /// </summary>
        [field: SerializeField, Header("杖設置のクールタイム")] public float rodSetCoolTime { get; private set; }

        /// <summary>
        /// 最大チャージ距離
        /// </summary>
        [field: SerializeField, Header("最大チャージ距離")] public float chargeMax { get; private set; }

        /// <summary>
        /// 体力
        /// </summary>
        [field: SerializeField, Header("体力")] public float hp { get; private set; }
        /// <summary>
        /// 体力上昇量
        /// </summary>
        [field: SerializeField, Header("体力上昇量")] public float hpBuff { get; private set; }

        /// <summary>
        /// チャージ攻撃ダメージ
        /// </summary>
        [field: SerializeField, Header("チャージ攻撃ダメージ")] public float chargeDmg { get; private set; }
        /// <summary>
        /// チャージ攻撃ダメージ上昇量
        /// </summary>
        [field: SerializeField, Header("チャージ攻撃ダメージ上昇量")] public float chargeDmgBuff { get; private set; }

        /// <summary>
        /// 移動速度
        /// </summary>
        [field: SerializeField, Header("移動速度")] public float speed { get; private set; }
        /// <summary>
        /// 移動速度上昇量
        /// </summary>
        [field: SerializeField, Header("移動速度上昇量")] public float speedBuff { get; private set; }

        /// <summary>
        /// 杖HP
        /// </summary>
        [field: SerializeField, Header("杖HP")] public float rodHp { get; private set; }
        /// <summary>
        /// 杖HP上昇量
        /// </summary>
        [field: SerializeField, Header("杖HP上昇量")] public float rodHpBuff { get; private set; }

        /// <summary>
        /// 杖ダメージ
        /// </summary>
        [field: SerializeField, Header("杖ダメージ")] public float rodDmg { get; private set; }
        /// <summary>
        /// 杖ダメージ上昇量
        /// </summary>
        [field: SerializeField, Header("杖ダメージ上昇量")] public float rodDmgBuff { get; private set; }

        /// <summary>
        /// 杖範囲
        /// </summary>
        [field: SerializeField, Header("杖範囲")] public float rodRange { get; private set; }
        /// <summary>
        /// 杖範囲上昇量
        /// </summary>
        [field: SerializeField, Header("杖範囲上昇量")] public float rodRangeBuff { get; private set; }

        /// <summary>
        /// 杖攻撃間隔
        /// </summary>
        [field: SerializeField, Header("杖攻撃間隔")] public float rodInterval { get; private set; }
        /// <summary>
        /// 杖攻撃間隔上昇量
        /// </summary>
        [field: SerializeField, Header("杖攻撃間隔上昇量")] public float rodIntervalBuff { get; private set; }

        /// <summary>
        /// 杖攻撃持続時間
        /// </summary>
        [field: SerializeField, Header("杖攻撃持続時間")] public float rodDuration { get; private set; }
        /// <summary>
        /// 杖攻撃持続時間上昇量
        /// </summary>
        [field: SerializeField, Header("杖攻撃持続時間上昇量")] public float rodDurationBuff { get; private set; }

        /// <summary>
        /// 杖攻撃弾数
        /// </summary>
        [field: SerializeField, Header("杖攻撃弾数")] public float rodAmount { get; private set; }
        /// <summary>
        /// 杖攻撃弾数上昇量
        /// </summary>
        [field: SerializeField, Header("杖攻撃弾数上昇量")] public float rodAmountBuff { get; private set; }
    }
}
