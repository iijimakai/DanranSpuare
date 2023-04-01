using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Shun_Player;

namespace Shun_System
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerBase playerBase;

        private Vector2 mouseVector = Vector2.zero;
        private Vector2 defaultPos = Vector2.zero;

        public void Init(PlayerBase _playerBase)
        {
            playerBase = _playerBase;

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(_ => { MouseMove(); })
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => { MouseOnDown(); })
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(_ => { MouseOnUp(); })
                .AddTo(this);
        }

        /// <summary>
        /// ボタン押した時の処理
        /// </summary>
        private void MouseOnDown()
        {
            defaultPos = Input.mousePosition;
        }
        /// <summary>
        /// ボタン離した時の処理
        /// </summary>
        private void MouseOnUp()
        {
            defaultPos = Vector2.zero;
            mouseVector = Vector2.zero;
        }
        /// <summary>
        /// 移動ベクトルを出す
        /// </summary>
        private void MouseMove()
        {
            Vector2 mousePos = Input.mousePosition;
            mouseVector = (mousePos - defaultPos);
            playerBase.Move(mouseVector.normalized);
        }
    }
}
