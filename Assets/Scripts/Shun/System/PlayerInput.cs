using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Shun_Player;
using Shun_UI;
using Cysharp.Threading.Tasks;
using System;

namespace Shun_System
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerBase playerBase;

        private Vector2 defaultPos = Vector2.zero;

        private bool isRodCharging = false;
        private bool isRodCoolTime = false;
        private bool isDashCoolTime = false;
        private bool isArrow = false;

        private CompositeDisposable moveDisposables = new CompositeDisposable();
        private CompositeDisposable rodDisposables = new CompositeDisposable();

        private Animator plAni;
        private SpriteRenderer plRend;

        ArrowController arrowController;

        public void Init(PlayerBase _playerBase)
        {
            playerBase = _playerBase;

            plAni = playerBase.GetRend.GetComponent<Animator>();
            plRend = playerBase.GetRend.GetComponent<SpriteRenderer>();
            arrowController = gameObject.GetComponent<ArrowController>();

            //ここから
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0) && !isRodCharging)
                .Subscribe(_ => { 
                    defaultPos = Input.mousePosition;
                })
                .AddTo(moveDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0) && !isRodCharging)
                .Subscribe(_ => { 
                    if (playerBase != null)
                    {
                        playerBase.Move(MouseMove().normalized);
                        // UIにスティックの座標を渡す
                    }
                })
                .AddTo(moveDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0) && !isRodCharging)
                .Subscribe(_ => {
                    plAni.SetBool("Walking", false);
                    plAni.SetBool("Idling", true);
                    defaultPos = Vector2.zero;
                })
                .AddTo(moveDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0) && Input.GetKeyDown(KeyCode.X) && !isRodCharging && !isDashCoolTime)
                .Subscribe(_ => {
                    StartCoroutine(playerBase.Dash(MouseMove().normalized));
                    DashCoolTime(PlayerParameter.dashCoolTime);
                })
                .AddTo(moveDisposables);
            //ここまで移動用

            this.UpdateAsObservable()
                .Where(_ =>  Input.GetKeyDown(KeyCode.Z) && !isRodCharging && !isRodCoolTime && !isArrow)
                .Subscribe(_ => {
                    if (playerBase.havingRod <= 0 ) 
                    {
                        Debug.Log("杖を所持していません。");
                    }
                    else
                    {
                        plAni.SetBool("Charging", true);
                        defaultPos = Input.mousePosition;
                        isRodCharging = true;
                    }
                })
                .AddTo(rodDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0) && isRodCharging)
                .Subscribe(_ => { defaultPos = Input.mousePosition; })
                .AddTo(rodDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0) && isRodCharging)
                .Subscribe(_ => { playerBase.SetChargeRatio(Charge()); })
                .AddTo(rodDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0) && isRodCharging)
                .Subscribe(_ => {
                    playerBase.SetRod(MouseMove().normalized);
                    RodCoolTime(PlayerParameter.rodSetCoolTime);
                    defaultPos = Vector2.zero;
                    plAni.SetBool("Walking", false);
                    plAni.SetBool("Idling", true);
                    plAni.SetBool("Charging", false);
                    isRodCharging = false;
                })
                .AddTo(rodDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape) && isRodCharging)
                .Subscribe(_ => {
                    defaultPos = Vector2.zero;
                    plAni.SetBool("Walking", false);
                    plAni.SetBool("Idling", true);
                    plAni.SetBool("Charging", false);
                    isRodCharging = false;
                })
                .AddTo(rodDisposables);
        }

        private async void DashCoolTime(float time)
        {
            try
            {
                var cancellationToken = this.GetCancellationTokenOnDestroy();
                isDashCoolTime = true;
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた時の処理
            }
            finally
            {
                isDashCoolTime = false;
            }
        }
        private async void RodCoolTime(float time)
        {
            try
            {
                var cancellationToken = this.GetCancellationTokenOnDestroy();
                isRodCoolTime = true;
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた時の処理
            }
            finally
            {
                isRodCoolTime = false;
            }
        }

        /// <summary>
        /// チャージ中の中心からの距離を算出
        /// </summary>
        /// <returns>距離</returns>
        private float Charge()
        {
            var vector = MouseMove();
            var charge = vector.x * vector.x + vector.y * vector.y;
            return Mathf.Sqrt(charge);
        }

        /// <summary>
        /// /// 移動ベクトルを出す
        /// </summary>
        /// // <returns>ベクトル</returns>
        private Vector2 MouseMove()
        {
            Vector2 mousePos = Input.mousePosition;
            var mouseVector = (mousePos - defaultPos);
            return mouseVector;
        }

        private void OnDestroy()
        {
            moveDisposables.Dispose();
            rodDisposables.Dispose();
        }

        private void OnDisable()
        {
            moveDisposables.Clear();
            rodDisposables.Clear();
        }
    }
}
