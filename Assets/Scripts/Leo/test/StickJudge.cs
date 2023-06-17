using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

namespace Leo
{
    public class StickJudge : MonoBehaviour
    {
        public string portName = "COM6";   // 接続するシリアルポートの名前
        public int baudRate = 115200;      // ボーレート


        public SerialHandler serialHandler;
        private SerialPort serialPort;
        public float delta = 0.1f;
        private float deltaPos;
        private Vector2 pos;

        void Start()
        {
            Open();
        }

        private void Open()
        {
            //serialPort_ = new SerialPort("COM" + portNumber, baudRate, Parity.None, 8, StopBits.One);
            serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);

            //または
            //serialPort_ = new SerialPort(portName, baudRate);
            serialPort.Open();
        }

        void Update()
        {
            if (serialPort.IsOpen)
            {
                pos = transform.localPosition;

                // シリアルポートから値を読み取る
                string getX_POS = serialPort.ReadLine();
                string getY_POS = serialPort.ReadLine();


                // 値を整数に変換する
                int value_x, value_y;
                if (int.TryParse(getX_POS, out value_x))
                {
                    // 値を表示する（例）
                    Debug.Log("X: " + value_x);

                    // オブジェクトを移動する
                    if (value_x < 512)
                    {
                        deltaPos = delta * 5;
                        pos.x -= deltaPos;
                    }
                    else
                    {
                        deltaPos = delta * 5;
                        pos.x += deltaPos;
                    }
                }

                if (int.TryParse(getY_POS, out value_y))
                {
                    // 値を表示する（例）
                    Debug.Log("X: " + value_y);

                    // オブジェクトを移動する
                    if (value_y < 512)
                    {
                        deltaPos = delta * 5;
                        pos.y -= deltaPos;
                    }
                    else
                    {
                        deltaPos = delta * 5;
                        pos.y += deltaPos;
                    }
                }
            }
        }

        void OnDestroy()
        {
            // シリアルポートを閉じる
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}