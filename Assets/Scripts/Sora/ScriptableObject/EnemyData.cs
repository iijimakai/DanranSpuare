using UnityEngine;

namespace Sora_Constants
{
    [CreateAssetMenu(menuName = "Datas/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        /// <summary>
        /// 体力
        /// </summary>
        [field: SerializeField, Header("体力")] public int hp { get; private set; }
        /// <summary>
        /// スピード
        /// </summary>
        [field: SerializeField, Header("スピード")] public int speed { get; private set; }
        /// <summary>
        /// 弾の攻撃力
        /// </summary>
        [field: SerializeField, Header("弾の攻撃力")] public int attackPoint { get; private set; }
        /// <summary>
        /// 弾の速さ
        /// </summary>
        [field: SerializeField, Header("弾の速さ")] public float bulletSpeed { get; private set; }
        /// <summary>
        /// 射程
        /// </summary>
        [field: SerializeField, Header("射程")] public float firingRange { get; private set; }
        /// <summary>
        /// 攻撃間隔
        /// </summary>
        [field: SerializeField, Header("攻撃間隔")] public float attackInterval { get; private set; }
        /// <summary>
        /// 瞬間移動できる距離
        /// </summary>
        [field: SerializeField, Header("瞬間移動距離")] public float teleportationDistance { get; private set; }

    }
}