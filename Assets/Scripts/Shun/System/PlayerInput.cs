using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Shun_Player;
using Shun_UI;

namespace Shun_System
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerBase playerBase;

        private Vector2 defaultPos = Vector2.zero;

        private bool isCharging = false;

        private CompositeDisposable moveDisposables = new CompositeDisposable();
        private CompositeDisposable rodDisposables = new CompositeDisposable();

        public void Init(PlayerBase _playerBase)
        {
            playerBase = _playerBase;

            //ここから
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0) && !isCharging)
                .Subscribe(_ => { 
                    defaultPos = Input.mousePosition;
                })
                .AddTo(moveDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0) && !isCharging)
                .Subscribe(_ => { 
                    playerBase.Move(MouseMove().normalized);
                    //UIにスティックの座標を渡す
                })
                .AddTo(moveDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0) && !isCharging)
                .Subscribe(_ => { 
                    defaultPos = Vector2.zero;
                })
                .AddTo(moveDisposables);
            //ここまで移動用

            this.UpdateAsObservable()
                .Where(_ =>  Input.GetKeyDown(KeyCode.Z) && !isCharging)
                .Subscribe(_ => { 
                    playerBase.SetRod();
                    defaultPos = Input.mousePosition;
                    isCharging = true;
                })
                .AddTo(rodDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0) && isCharging)
                .Subscribe(_ => { defaultPos = Input.mousePosition; })
                .AddTo(rodDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0) && isCharging)
                .Subscribe(_ => { playerBase.SetChargeRatio(Charge()); })
                .AddTo(rodDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0) && isCharging)
                .Subscribe(_ => {
                    defaultPos = Vector2.zero;
                    isCharging = false;
                })
                .AddTo(rodDisposables);
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
