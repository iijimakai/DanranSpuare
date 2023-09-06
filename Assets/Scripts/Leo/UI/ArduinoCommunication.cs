using UnityEngine;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    public GameObject player;  // 移動させるオブジェクト
    [Header("USBのポート番号")] public string portName = "COM6";  // Arduinoのポート名
    [Header("基板のボードレートなので変更しなくてOK")] public int baudRate = 115200;  // Arduinoのボードレート
    public float maxDistance = 5f; // 最大移動距離

    private SerialPort serialPort;
    private float angleX;
    private float angleY;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                string[] values = data.Split(',');

                //var data = message.Split(
                //    new string[] { "," }, System.StringSplitOptions.None); // カンマで分割する

                if (values.Length == 2)
                {
                    int analogValueX = int.Parse(values[0]);
                    int analogValueY = int.Parse(values[1]);

                    //float angleX = Map(analogValueX, 0, 1023, 0f, 360f); // X軸方向の角度
                    //float angleY = Map(analogValueY, 0, 1023, 0f, 360f); // Y軸方向の角度

                    //float radius = maxDistance / 2f; // 半径
                    //float newPositionX = radius * Mathf.Cos(angleX * Mathf.Deg2Rad); // X座標の計算
                    //float newPositionY = radius * Mathf.Sin(angleY * Mathf.Deg2Rad); // Y座標の計算

                    // X方向の制御
                    if (analogValueX == 514 || analogValueX == 515)
                    {
                        analogValueX = 0;
                    }
                    else if (analogValueX < 514 || analogValueX < 1023)
                    {
                        analogValueX = (analogValueX - 514) / 1000;
                    }

                    // Y方向の制御
                    if (analogValueY == 529)
                    {
                        analogValueY = 0;
                    }
                    else if (analogValueY < 529 || analogValueY < 1023)
                    {
                        analogValueY = (analogValueY - 529) / 1000;
                    }

                    //player.transform.position = new Vector3(newPositionX, newPositionY, 0f);
                    player.transform.position = new Vector3(analogValueX, analogValueY, 0f);
                }
            }
            catch (System.Exception)
            {
                // エラーハンドリング
            }
        }
    }

    float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
