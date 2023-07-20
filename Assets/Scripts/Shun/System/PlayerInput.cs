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

        private bool isCharging = false;
        private bool isCoolTime = false;

        private CompositeDisposable moveDisposables = new CompositeDisposable();
        private CompositeDisposable rodDisposables = new CompositeDisposable();

        private Animator plAni;

        public void Init(PlayerBase _playerBase)
        {
            playerBase = _playerBase;

            plAni = playerBase.GetComponent<Animator>();

            //��������
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
                    //UI�ɃX�e�B�b�N�̍��W��n��
                })
                .AddTo(moveDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0) && !isCharging)
                .Subscribe(_ => {
                    plAni.SetBool("Walking", false);
                    plAni.SetBool("Idling", true);
                    defaultPos = Vector2.zero;
                })
                .AddTo(moveDisposables);
            //�����܂ňړ��p

            this.UpdateAsObservable()
                .Where(_ =>  Input.GetKeyDown(KeyCode.Z) && !isCharging && !isCoolTime)
                .Subscribe(_ => {
                    if (playerBase.havingRod <= 0 ) 
                    {
                        Debug.Log("����������Ă��܂���B");
                    }
                    else
                    {
                        plAni.SetBool("Walking", false);
                        plAni.SetBool("Idling", true);
                        defaultPos = Input.mousePosition;
                        isCharging = true;
                    }
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
                    playerBase.SetRod();
                    CoolTime();
                    isCharging = false;
                })
                .AddTo(rodDisposables);

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape) && isCharging)
                .Subscribe(_ => {
                    defaultPos = Vector2.zero;
                    isCharging = false;
                })
                .AddTo(rodDisposables);
        }

        private async void CoolTime()
        {
            isCoolTime = true;

            await UniTask.Delay(TimeSpan.FromSeconds(PlayerParameter.rodSetCoolTime));

            isCoolTime = false;
        }

        /// <summary>
        /// �`���[�W���̒��S����̋������Z�o
        /// </summary>
        /// <returns>����</returns>
        private float Charge()
        {
            var vector = MouseMove();
            var charge = vector.x * vector.x + vector.y * vector.y;
            return Mathf.Sqrt(charge);
        }

        /// <summary>
        /// /// �ړ��x�N�g�����o��
        /// </summary>
        /// // <returns>�x�N�g��</returns>
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
