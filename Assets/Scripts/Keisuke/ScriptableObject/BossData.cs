using UnityEngine;

[CreateAssetMenu(menuName = "Datas/BossData")]
public class BossData : ScriptableObject
{
    [Header("General Properties")]
    [SerializeField,Header("ヒットポイント")] private int hp; // ヒットポイント
    [SerializeField,Header("移動速度")] private float moveSpeed; // 移動速度

    [Header("Attack Properties")]
    [SerializeField,Header("攻撃力")] private float attackPower; // 攻撃力
    [SerializeField,Header("通常攻撃の間隔")] private float attackInterval; // 通常攻撃の間隔
    [SerializeField,Header("一発目の攻撃の間隔")] private float firstAttackInterval; // 一発目の攻撃の間隔
    [SerializeField,Header("攻撃態勢に入るまでの間隔")] private float prepareAttackInterval; // 攻撃態勢に入るまでの間隔
    [SerializeField,Header("追跡中にプレイヤーに近づける間隔")] private float trackingRange;

    // プロパティで外部から参照できるようにする
    public int HP => hp;
    public float MoveSpeed => moveSpeed;
    public float AttackPower => attackPower;
    public float AttackInterval => attackInterval;
    public float FirstAttackInterval => firstAttackInterval;
    public float PrepareAttackInterval => prepareAttackInterval;
    public float TrackingRange => trackingRange;
}
