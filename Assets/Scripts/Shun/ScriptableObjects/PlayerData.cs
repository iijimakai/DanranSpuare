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
        /// ��̍ő�X�g�b�N��
        /// </summary>
        [field: SerializeField, Header("��̍ő�X�g�b�N��")] public float rodStock { get; private set; }

        /// <summary>
        /// �񂪉񕜂���܂ł̎���
        /// </summary>
        [field: SerializeField, Header("�񂪉񕜂���܂ł̎���")] public float rodRecastTime { get; private set; }

        /// <summary>
        /// ��ݒu�̃N�[���^�C��
        /// </summary>
        [field: SerializeField, Header("��ݒu�̃N�[���^�C��")] public float rodSetCoolTime { get; private set; }

        /// <summary>
        /// �ő�`���[�W����
        /// </summary>
        [field: SerializeField, Header("�ő�`���[�W����")] public float chargeMax { get; private set; }

        /// <summary>
        /// �̗�
        /// </summary>
        [field: SerializeField, Header("�̗�")] public float hp { get; private set; }
        /// <summary>
        /// �̗͏㏸��
        /// </summary>
        [field: SerializeField, Header("�̗͏㏸��")] public float hpBuff { get; private set; }

        /// <summary>
        /// �`���[�W�U���_���[�W
        /// </summary>
        [field: SerializeField, Header("�`���[�W�U���_���[�W")] public float chargeDmg { get; private set; }
        /// <summary>
        /// �`���[�W�U���_���[�W�㏸��
        /// </summary>
        [field: SerializeField, Header("�`���[�W�U���_���[�W�㏸��")] public float chargeDmgBuff { get; private set; }

        /// <summary>
        /// �ړ����x
        /// </summary>
        [field: SerializeField, Header("�ړ����x")] public float speed { get; private set; }
        /// <summary>
        /// �ړ����x�㏸��
        /// </summary>
        [field: SerializeField, Header("�ړ����x�㏸��")] public float speedBuff { get; private set; }

        /// <summary>
        /// ��HP
        /// </summary>
        [field: SerializeField, Header("��HP")] public float rodHp { get; private set; }
        /// <summary>
        /// ��HP�㏸��
        /// </summary>
        [field: SerializeField, Header("��HP�㏸��")] public float rodHpBuff { get; private set; }

        /// <summary>
        /// ��_���[�W
        /// </summary>
        [field: SerializeField, Header("��_���[�W")] public float rodDmg { get; private set; }
        /// <summary>
        /// ��_���[�W�㏸��
        /// </summary>
        [field: SerializeField, Header("��_���[�W�㏸��")] public float rodDmgBuff { get; private set; }

        /// <summary>
        /// ��͈�
        /// </summary>
        [field: SerializeField, Header("��͈�")] public float rodRange { get; private set; }
        /// <summary>
        /// ��͈͏㏸��
        /// </summary>
        [field: SerializeField, Header("��͈͏㏸��")] public float rodRangeBuff { get; private set; }

        /// <summary>
        /// ��U���Ԋu
        /// </summary>
        [field: SerializeField, Header("��U���Ԋu")] public float rodInterval { get; private set; }
        /// <summary>
        /// ��U���Ԋu�㏸��
        /// </summary>
        [field: SerializeField, Header("��U���Ԋu�㏸��")] public float rodIntervalBuff { get; private set; }

        /// <summary>
        /// ��U����������
        /// </summary>
        [field: SerializeField, Header("��U����������")] public float rodDuration { get; private set; }
        /// <summary>
        /// ��U���������ԏ㏸��
        /// </summary>
        [field: SerializeField, Header("��U���������ԏ㏸��")] public float rodDurationBuff { get; private set; }

        /// <summary>
        /// ��U���e��
        /// </summary>
        [field: SerializeField, Header("��U���e��")] public float rodAmount { get; private set; }
        /// <summary>
        /// ��U���e���㏸��
        /// </summary>
        [field: SerializeField, Header("��U���e���㏸��")] public float rodAmountBuff { get; private set; }
    }
}
