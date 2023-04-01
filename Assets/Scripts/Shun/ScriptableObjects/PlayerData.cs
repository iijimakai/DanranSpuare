using UnityEngine;

namespace Shun_Constants
{
    [CreateAssetMenu(menuName = "Datas/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        /// <summary>
        /// �L�����N�^�[�̎��
        /// </summary>
        [field: SerializeField, Header("�L�����N�^�[�̎��")] public CharacterType characterType { get; private set; }
        /// <summary>
        /// �̗�
        /// </summary>
        [field: SerializeField, Header("�̗�")] public int hp { get; private set; }
        /// <summary>
        /// �ړ����x
        /// </summary>
        [field: SerializeField, Header("�ړ����x")] public float speed { get; private set; }

    }
}
