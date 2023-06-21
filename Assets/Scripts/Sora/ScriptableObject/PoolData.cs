using UnityEngine;

namespace Constants
{
    [CreateAssetMenu(menuName = "Datas/PoolData")]
    public class PoolData : ScriptableObject
    {
        [field: SerializeField, Header("各弾の最大数")] public int bulletPoolMaxvalue { get; private set; }
    }
}