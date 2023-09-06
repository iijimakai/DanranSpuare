using UnityEngine;
using UniRx;
using Shun_Constants;
using System.Collections;
using System.Collections.Generic;
using parameter = Shun_Player.PlayerParameter;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System;
using Shun_Rod;

namespace Shun_Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] ShockWave wave;
        [SerializeField] GameObject rend;
        private CanvasShow canvasShow;
        public float hp {  get; private set; }
        public float havingRod { get; private set; }

        private string rodAddress;
        private bool rodCharge = false;

        private bool isStar = false;
        private float starTime;

        private float chargeRatio = 0;
        public static List<GameObject> rodStock = new List<GameObject>();

        private ReactiveProperty<Direction> direction = new ReactiveProperty<Direction>();
        private CompositeDisposable disposables = new CompositeDisposable();

        private Animator animator;

        public void Init(PlayerData _data, string _rodAddress)
        {
            canvasShow = FindObjectOfType<CanvasShow>().GetComponent<CanvasShow>();
            Debug.Log(canvasShow);
            wave.gameObject.SetActive(false);

            animator = rend.GetComponent<Animator>();

            parameter.Init(_data);
            ParameterBuff.Init();

            ParameterBuff.hpLevel.Subscribe(_ => hp += _data.hp * _data.hpBuff).AddTo(disposables);

            hp = parameter.maxHp;
            havingRod = parameter.rodStock;
            starTime = parameter.starTime;
 
            rodAddress = _rodAddress;

            direction.Value = Direction.Right;
            direction.Subscribe(_ => ChangeDirection()).AddTo(disposables);
        }

        public GameObject GetRend => rend;

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
        /// 杖の設置
        /// </summary>
        public async void SetRod(Vector2 vec)
        {
            Shock(vec);

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
        /// 杖消滅時の処理
        /// </summary>
        /// <param name="rod">処理対象の杖</param>
        public void RodClear(RodBase rod)
        {
            BulletPoolUtile.RemoveBullet(rod.gameObject);
        }

        public void SetChargeRatio(float charge)
        {
            charge = charge > parameter.chargeMax ? parameter.chargeMax : charge;
            chargeRatio = charge / parameter.chargeMax * 100;
            //UIにチャージ割合を反映
        }

        /// <summary>
        /// Objectの向きを反転
        /// </summary>
        private void ChangeDirection() 
        {
            var scale = transform.localScale;
            scale.x = (int)direction.Value;
            rend.transform.localScale = scale;
        }

        /// <summary>
        /// Objectを動かす
        /// </summary>
        /// <param name="moveVec">移動ベクトル</param>
        public void Move(Vector2 moveVec)
        {
            animator.SetBool("Idling", false);
            animator.SetBool("Walking", true);
            if (moveVec == Vector2.zero)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Idling", true);
            }

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
            if (isStar) return;
            Debug.Log("P1 : "+hp +"->"+ (hp - damage));
            
            hp -= damage;
            if (hp <= 0)
            {
                Dead();
            }
            Delay();
        }

        private async void Delay()
        {
            isStar = true;
            await Task.Delay(TimeSpan.FromSeconds(starTime));
            isStar = false;
        }

        private void Shock(Vector2 vec)
        {
            Vector2 pos = transform.position;
            wave.Set(pos, vec, chargeRatio);
        }

        private void Dead()
        {
            Debug.Log("GameOver")
            canvasShow.GameOverCanvasShow();
            Destroy(gameObject);
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
