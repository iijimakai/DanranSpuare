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
    [field: SerializeField,Header("一度にアクティブにできるBulletの最大数")] public int maxActiveBullets { get; private set; } // 一度にアクティブにできるBulletの最大数
    [field: SerializeField,Header("ブレスの弾速")] public float bulletMoveSpeed { get; private set; } // ブレスの弾速
    [field: SerializeField, Header("警告アラートの距離")] public float alertDistanceFromPlayer { get; private set; } //
    [field: SerializeField, Header("警告アラートの表示時間")] public float alertDisplayDuration { get; private set; } //
    [field: SerializeField, Header("ブレスの持続時間")] public float breathDuration { get; private set; }
    [field: SerializeField, Header("警告アラートが表示されてから何秒後にブレス攻撃を開始するか")] public float warningToBreathDelay { get; private set; } // 3
    [field: SerializeField, Header("ブレスの密度")] public float bulletSpawnInterval { get; private set; } // 100
    [field: SerializeField, Header("ブレス後のインターバル")] public float postBreathInterval { get; private set; } // 1最適
}
