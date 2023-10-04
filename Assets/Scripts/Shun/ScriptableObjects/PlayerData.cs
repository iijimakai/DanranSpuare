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
        /// ��e��̖��G����
        /// </summary>
        [field: SerializeField, Header("��e��̖��G����(�b)")] public float starTime { get; private set; }

        /// <summary>
        /// �_�b�V������
        /// </summary>
        [field: SerializeField, Header("�_�b�V���̋���")] public float dashRange { get; private set; }

        /// <summary>
        /// �_�b�V���ɂ����鎞��
        /// </summary>
        [field: SerializeField, Header("�_�b�V���ɂ����鎞��")] public float dashTime { get; private set; }

        /// <summary>
        /// �_�b�V���N�[���^�C��
        /// </summary>
        [field: SerializeField, Header("�_�b�V���̃N�[���^�C��")] public float dashCoolTime { get; private set; }

        /// <summary>
        /// ��̍ő�X�g�b�N��
        /// </summary>
        [field: SerializeField, Header("��̍ő�X�g�b�N��(��)")] public float rodStock { get; private set; }

        /// <summary>
        /// �񂪉񕜂���܂ł̎���
        /// </summary>
        [field: SerializeField, Header("�񂪉񕜂���܂ł̎���(�b)")] public float rodRecastTime { get; private set; }

        /// <summary>
        /// ��ݒu�̃N�[���^�C��
        /// </summary>
        [field: SerializeField, Header("��ݒu�̃N�[���^�C��(�b)")] public float rodSetCoolTime { get; private set; }

        /// <summary>
        /// �ő�`���[�W����
        /// </summary>
        [field: SerializeField, Header("�ő�`���[�W����(m)")] public float chargeMax { get; private set; }

        /// <summary>
        /// �̗�
        /// </summary>
        [field: SerializeField, Header("�̗�")] public float hp { get; private set; }
        /// <summary>
        /// �̗͕ω���
        /// </summary>
        [field: SerializeField, Header("�̗͕ω���")] public float hpBuff { get; private set; }

        /// <summary>
        /// �`���[�W�U���_���[�W
        /// </summary>
        [field: SerializeField, Header("�`���[�W�U���_���[�W")] public float chargeDmg { get; private set; }
        /// <summary>
        /// �`���[�W�U���_���[�W�ω���
        /// </summary>
        [field: SerializeField, Header("�`���[�W�U���_���[�W�ω���")] public float chargeDmgBuff { get; private set; }
        /// <summary>
        /// �`���[�W�U���p������
        /// </summary>
        [field: SerializeField, Header("�`���[�W�U���p������(�b)")] public float waveTime { get; private set; }

        /// <summary>
        /// �ړ����x
        /// </summary>
        [field: SerializeField, Header("�ړ����x(m/s)")] public float speed { get; private set; }
        /// <summary>
        /// �ړ����x�ω���
        /// </summary>
        [field: SerializeField, Header("�ړ����x�ω���")] public float speedBuff { get; private set; }

        /// <summary>
        /// ��HP
        /// </summary>
        [field: SerializeField, Header("��HP")] public float rodHp { get; private set; }
        /// <summary>
        /// ��HP�ω���
        /// </summary>
        [field: SerializeField, Header("��HP�ω���")] public float rodHpBuff { get; private set; }

        /// <summary>
        /// ��_���[�W
        /// </summary>
        [field: SerializeField, Header("��_���[�W")] public float rodDmg { get; private set; }
        /// <summary>
        /// ��_���[�W�ω���
        /// </summary>
        [field: SerializeField, Header("��_���[�W�ω���")] public float rodDmgBuff { get; private set; }

        /// <summary>
        /// ��U���ђʐ�
        /// </summary>
        [field: SerializeField, Header("��U���ђʐ�")] public int penetrateCount { get; private set; }

        /// <summary>
        /// ��U���e��
        /// </summary>
        [field: SerializeField, Header("��U���e��(m/s)")] public float shotSpeed { get; private set; }
        /// <summary>
        /// ��U���e���ω���
        /// </summary>
        [field: SerializeField, Header("��U���e���ω���")] public float shotSpeedBuff { get; private set; }

        /// <summary>
        /// ��͈�
        /// </summary>
        [field: SerializeField, Header("��͈�(m)")] public float rodRange { get; private set; }
        /// <summary>
        /// ��͈͕ω���
        /// </summary>
        [field: SerializeField, Header("��͈͕ω���")] public float rodRangeBuff { get; private set; }

        /// <summary>
        /// ��U���N�[���^�C��
        /// </summary>
        [field: SerializeField, Header("��U���N�[���^�C��(�b)")] public float rodInterval { get; private set; }
        /// <summary>
        /// ��U���N�[���^�C���ω���
        /// </summary>
        [field: SerializeField, Header("��U���N�[���^�C���ω���")] public float rodIntervalBuff { get; private set; }

        /// <summary>
        /// ��U����������
        /// </summary>
        [field: SerializeField, Header("��U����������(�b)")] public float rodDuration { get; private set; }
        /// <summary>
        /// ��U���������ԕω���
        /// </summary>
        [field: SerializeField, Header("��U���������ԕω���")] public float rodDurationBuff { get; private set; }

        /// <summary>
        /// ��U���e��
        /// </summary>
        [field: SerializeField, Header("��U���e��(��)")] public float rodAmount { get; private set; }
        /// <summary>
        /// ��U���e���ω���
        /// </summary>
        [field: SerializeField, Header("��U���e���ω���")] public float rodAmountBuff { get; private set; }
    }
}
