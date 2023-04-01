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
        /// 体力
        /// </summary>
        [field: SerializeField, Header("体力")] public int hp { get; private set; }
        /// <summary>
        /// 移動速度
        /// </summary>
        [field: SerializeField, Header("移動速度")] public float speed { get; private set; }

    }
}
