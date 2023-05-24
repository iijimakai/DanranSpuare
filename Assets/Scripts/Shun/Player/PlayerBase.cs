using UnityEngine;
using UniRx;
using Shun_Constants;
using System.Collections.Generic;
using parameter = Shun_Player.PlayerParameter;
using Cysharp.Threading.Tasks;
using System;

namespace Shun_Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBase : MonoBehaviour
    {
        public float hp {  get; private set; }
        public float havingRod { get; private set; }

        private string rodAddress;

        private float chargeRatio = 0;
        private List<GameObject> rodStock = new List<GameObject>();

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

            GiveRod();
        }

        private async void GiveRod()
        {
            while (true)
            {
                if (havingRod < parameter.rodStock)
                {
                    havingRod++;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(parameter.rodRecastTime));
            }
        }

        public async void SetRod()
        {
            havingRod--;
            GameObject rod = await BulletPoolUtile.GetBullet(rodAddress);
            rod.transform.position = transform.position;
            rodStock.Add(rod);

            if (rodStock.Count > parameter.rodStock)
            {
                BulletPoolUtile.RemoveBullet(rodStock[0]);
                rodStock.RemoveAt(0);
            }
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

        private void Update()
        {
            Debug.Log(havingRod);
        }
    }
}
