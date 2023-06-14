using UnityEngine;
using UniRx;
using System;

namespace Leo
{
    public class SerialSent : MonoBehaviour
    {
        public SerialHandler serialHandler;
        private Subject<string> msg = new Subject<string>();

        void Start()
        {
            //信号を受信したときに、そのメッセージの処理を行う
            serialHandler.OnDataReceived += OnDataReceived;
        }

        //受信した信号(message)に対する処理
        void OnDataReceived(string message)
        {
            var data = message.Split(
                    new string[] { "\n" }, System.StringSplitOptions.None);
            try
            {
                Debug.Log(data[0]);    //Unityのコンソールに受信データを表示
                msg.OnNext(data[0]);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);  //エラーを表示
            }
        }

        public void WriteArduino(string msg)
        {
            serialHandler.Write(msg);
        }

        public IObservable<string> Getmessage()
        {
            return msg;
        }
    }
}