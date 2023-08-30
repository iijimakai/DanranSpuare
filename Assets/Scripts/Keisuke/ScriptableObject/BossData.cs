using UnityEngine;

[CreateAssetMenu(menuName = "Datas/BossData")]
public class BossData : ScriptableObject
{
    [field: SerializeField,Header("ヒットポイント")] public int hp { get; private set; } // ヒットポイント
    [field: SerializeField,Header("移動速度")] public float moveSpeed { get; private set; } // 移動速度
    [field: SerializeField,Header("攻撃力")] public float attackPower { get; private set; } // 攻撃力
    [field: SerializeField,Header("通常攻撃の間隔")] public float attackInterval { get; private set; } // 通常攻撃の間隔
    [field: SerializeField,Header("一発目の攻撃の間隔")] public float firstAttackInterval { get; private set; } // 一発目の攻撃の間隔
    [field: SerializeField,Header("攻撃態勢に入るまでの間隔")] public float prepareAttackInterval { get; private set; } // 攻撃態勢に入るまでの間隔
    [field: SerializeField,Header("追跡中にプレイヤーに近づける間隔")] public float trackingRange { get; private set; }
}
