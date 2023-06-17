using UnityEngine;
using UniRx;
using System;

namespace Leo
{
    public class ButtonJudge : MonoBehaviour
    {
        [SerializeField] private SerialSent serialSent;
        [SerializeField] private int num = 1;
        private Subject<bool> buttonUp = new Subject<bool>();
        private Subject<bool> buttonDown = new Subject<bool>();
        private Subject<bool> buttonLeft = new Subject<bool>();
        private Subject<bool> buttonRight = new Subject<bool>();
        private Subject<bool> stickButton = new Subject<bool>();

        private void Start()
        {
            SetPlayerNum(num);
            serialSent.Getmessage().Subscribe(moji =>    // Arduino -> Unityに送る
            {
                switch (moji)
                {
                    case "ButtonUp":
                        buttonUp.OnNext(true);
                        break;
                    case "ButtonDown":
                        buttonDown.OnNext(true);
                        break;
                    case "ButtonLeft":
                        buttonLeft.OnNext(true);
                        break;
                    case "ButtonRight":
                        buttonRight.OnNext(true);
                        break;
                    default:
                        break;
                }
            }).AddTo(this);
        }

        public void SetPlayerNum(int n)    // Unity -> Arduinoに送る
        {
            switch (n)
            {
                case 1:
                    serialSent.WriteArduino("1");
                    break;
                case 2:
                    serialSent.WriteArduino("2");
                    break;
                case 3:
                    serialSent.WriteArduino("3");
                    break;
                case 4:
                    serialSent.WriteArduino("4");
                    break;
                default:
                    break;
            }
        }

        public IObservable<bool> GetButtonUp()
        {
            return buttonUp;
        }
        public IObservable<bool> GetButtonDown()
        {
            return buttonDown;
        }
        public IObservable<bool> GetButtonLeft()
        {
            return buttonLeft;
        }
        public IObservable<bool> GetButtonRight()
        {
            return buttonRight;
        }
        public IObservable<bool> GetStickButton()
        {
            return stickButton;
        }
    }
}
