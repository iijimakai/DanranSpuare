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
        [SerializeField] GameObject rend;

        [SerializeField] private GameObject[] ice = new GameObject[8];
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
        private Camera mainCamera;

        public void Init(PlayerData _data, string _rodAddress, Camera mainCamera)
        {
            this.mainCamera = mainCamera;
            canvasShow = FindObjectOfType<CanvasShow>().GetComponent<CanvasShow>();
            ice[0].gameObject.SetActive(false);

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
            CameraMove(transform.position);
        }

        public IEnumerator Dash(Vector2 dashVec)
        {
            if (dashVec == Vector2.zero) yield break;

            var startTime = Time.timeSinceLevelLoad;
            Vector2 startPos = transform.position;
            Vector2 endPos = startPos + (dashVec * parameter.dashRange);
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = diff / parameter.dashTime;

            isStar = true;
            while (rate <= 1)
            {
                diff = Time.timeSinceLevelLoad - startTime;
                rate = diff / parameter.dashTime;

                transform.position = Vector2.Lerp(startPos, endPos, rate);
                //Debug.Log(Vector2.Lerp(startPos, endPos, rate));
                yield return new WaitForFixedUpdate();
            }
            isStar = false;
        }

        private void CameraMove(Vector2 pos)
        {
            mainCamera.transform.position = new Vector3(pos.x, pos.y, -10);
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
            Delay(starTime);
        }

        private async void Delay(float time)
        {
            isStar = true;
            await Task.Delay(TimeSpan.FromSeconds(time));
            isStar = false;
        }

        private void Shock(Vector2 vec)
        {
            switch ((int)chargeRatio/14)
            {
                case 0:
                    IceSizeFix(false, false, false, false, false, false, false);
                    break;
                case 1: 
                    IceSizeFix(true, false, false, false, false, false, false);
                    break;
                case 2: 
                    IceSizeFix(true, true, false, false, false, false, false);
                    break;
                case 3: 
                    IceSizeFix(true, true, true, false, false, false, false);
                    break;
                case 4:
                    IceSizeFix(true, true, true, true, false, false, false);
                    break;
                case 5:
                    IceSizeFix(true, true, true, true, true, false, false);
                    break;
                case 6:
                    IceSizeFix(true, true, true, true, true, true, false);
                    break;
                case 7:
                    IceSizeFix(true, true, true, true, true, true, true);
                    break;
                default:
                    break;
            }
            Vector2 pos = transform.position;
            ice[0].GetComponent<ShockWave>().Set(pos, vec);
        }

        private void IceSizeFix(bool xxxs, bool xxs, bool xs, bool s, bool m, bool l, bool xl)
        {
            for (int i = 1; i < ice.Length; i++)
            {
                ice[i].SetActive(false);
            }

            if (xxxs) ice[1].SetActive(true);
            if (xxs) ice[2].SetActive(true);
            if (xs) ice[3].SetActive(true);
            if (s) ice[4].SetActive(true);
            if (m) ice[5].SetActive(true);
            if (l) ice[6].SetActive(true);
            if (xl) ice[7].SetActive(true);
        }

        private async void Dead()
        {
            Debug.Log("GameOver");
            canvasShow.GameOverCanvasShow();
            rend.SetActive(false);
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
