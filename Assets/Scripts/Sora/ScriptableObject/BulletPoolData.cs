using UnityEngine;

namespace Sora_Constants
{
    [CreateAssetMenu(menuName = "Datas/BulletPoolData")]
    public class BulletPoolData : ScriptableObject
    {
        [field: SerializeField, Header("プレイヤーテムの弾の最大数")] public int PlayerTemBulletMAxValue { get; private set; }
        [field: SerializeField, Header("プレイヤーLagの弾の最大数")] public int PlayerLagBulletMAxValue { get; private set; }
        [field: SerializeField, Header("プレイヤーシスの弾の最大数")] public int PlayerBulletMAxValue { get; private set; }
        [field: SerializeField, Header("エネミーE1の弾の最大数")] public int enemyE1BulletMaxValue { get; private set; }
        [field: SerializeField, Header("エネミーE2の弾の最大数")] public int enemyE2BulletMaxValue { get; private set; }
        [field: SerializeField, Header("エネミーE3の弾の最大数")] public int enemyE3BulletMaxValue { get; private set; }
        [field: SerializeField, Header("エネミーE4の弾の最大数")] public int enemyE4BulletMaxValue { get; private set; }
    }
}