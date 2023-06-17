using UnityEngine;
using UniRx;
using Shun_Constants;
using System.Collections.Generic;
using parameter = Shun_Player.PlayerParameter;
using Cysharp.Threading.Tasks;
using System;
using Shun_Rod;

namespace Shun_Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBase : MonoBehaviour
    {
        public float hp {  get; private set; }
        public float havingRod { get; private set; }

        private string rodAddress;
        private bool rodCharge = false;

        private float chargeRatio = 0;
        public static List<GameObject> rodStock = new List<GameObject>();

        private ReactiveProperty<Direction> direction = new ReactiveProperty<Direction>();
        private CompositeDisposable disposables = new CompositeDisposable();

        public void Init(PlayerData _data, string _rodAddress)
        {
            parameter.Init(_data);
            ParameterBuff.Init();

            ParameterBuff.hpLevel.Subscribe(_ => hp += _data.hp * _data.hpBuff).AddTo(disposables);

            hp = 60;
            havingRod = parameter.rodStock;
 
            rodAddress = _rodAddress;

            direction.Value = Direction.Right;
            direction.Subscribe(_ => ChangeDirection()).AddTo(disposables);
        }

        private async void GiveRod()
        {
            if (rodCharge) return;

            rodCharge = true;

            while (havingRod < parameter.rodStock)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(parameter.rodRecastTime));

                havingRod++;
            }

            rodCharge = false;
        }

        /// <summary>
        /// ��̐ݒu
        /// </summary>
        public async void SetRod()
        {
            havingRod--;
            GiveRod();
            GameObject rod = await BulletPoolUtile.GetBullet(rodAddress);
            rod.GetComponent<RodBase>().Init(this);
            rod.transform.position = transform.position;
            rodStock.Add(rod);

            if (rodStock.Count > parameter.rodStock)
            {
                BulletPoolUtile.RemoveBullet(rodStock[0]);
                Debug.Log(rodStock.Count);
                //rodStock.RemoveAt(0);
            }
        }
        /// <summary>
        /// ����Ŏ��̏���
        /// </summary>
        /// <param name="rod">�����Ώۂ̏�</param>
        public void RodClear(RodBase rod)
        {
            BulletPoolUtile.RemoveBullet(rod.gameObject);
        }

        public void SetChargeRatio(float charge)
        {
            charge = charge > parameter.chargeMax ? parameter.chargeMax : charge;
            chargeRatio = charge / parameter.chargeMax * 100;
            //UI�Ƀ`���[�W�����𔽉f
        }

        /// <summary>
        /// Object�̌����𔽓]
        /// </summary>
        private void ChangeDirection() 
        {
            var scale = transform.localScale;
            scale.x = (int)direction.Value;
            transform.localScale = scale;
        }

        /// <summary>
        /// Object�𓮂���
        /// </summary>
        /// <param name="moveVec">�ړ��x�N�g��</param>
        public void Move(Vector2 moveVec)
        {
            if (moveVec.x > 0)
            {
                direction.Value = Direction.Right;
            }
            else if (moveVec.x < 0)
            {
                direction.Value = Direction.Left;
            }
            transform.Translate(moveVec * parameter.speed * Time.deltaTime);
        }

        public void Damage(float damage)
        {
            
        }

        private void OnDestroy()
        {
            disposables.Dispose();
        }

        private void OnDisable()
        {
            disposables.Clear();
        }
    }
}
