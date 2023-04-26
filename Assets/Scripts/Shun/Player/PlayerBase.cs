using UnityEngine;
using UniRx;
using Shun_Constants;
using System.Collections.Generic;
using parameter = Shun_Player.PlayerParameter;

namespace Shun_Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBase : MonoBehaviour
    {
        private string rodAddress;

        private float chargeRatio = 0;
        private List<GameObject> rodStock = new List<GameObject>();

        private ReactiveProperty<Direction> direction = new ReactiveProperty<Direction>();
        private CompositeDisposable disposables = new CompositeDisposable();

        public void Init(PlayerData _data, string _rodAddress)
        {
            parameter.Init(_data);
            ParameterBuff.Init();
 
            rodAddress = _rodAddress;

            direction.Value = Direction.Right;
            direction.Subscribe(_ => ChangeDirection()).AddTo(disposables);
        }

        public async void SetRod()
        {
            Debug.Log(parameter.hp);
            ParameterBuff.Up();
            Debug.Log(parameter.hp);
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
            Debug.Log(chargeRatio); //UIにチャージ割合を反映
        }

        /// <summary>
        /// Objectの向きを反転
        /// </summary>
        private void ChangeDirection() 
        {
            var scale = transform.localScale;
            scale.x = (int)direction.Value;
            transform.localScale = scale;
        }

        /// <summary>
        /// Objectを動かす
        /// </summary>
        /// <param name="moveVec">移動ベクトル</param>
        public void Move(Vector2 moveVec)
        {
            if (moveVec.x >= 0)
            {
                direction.Value = Direction.Right;
            }
            else
            {
                direction.Value = Direction.Left;
            }
            transform.Translate(moveVec * parameter.speed * Time.deltaTime);
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
