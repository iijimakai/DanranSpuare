using UnityEngine;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    public GameObject square;  // 移動させるオブジェクト
    public string portName = "COM6";  // Arduinoのポート名
    public int baudRate = 115200;  // Arduinoのボーレート
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
                string dataX = serialPort.ReadLine();
                string dataY = serialPort.ReadLine();

                int analogValueX = int.Parse(dataX);
                int analogValueY = int.Parse(dataY);

                angleX = Map(analogValueX, 0, 1023, 0f, 360f); // X軸方向の角度
                angleY = Map(analogValueY, 0, 1023, 0f, 360f); // Y軸方向の角度

                float radius = maxDistance / 2f; // 半径
                float newPositionX = radius * Mathf.Cos(angleX * Mathf.Deg2Rad); // X座標の計算
                float newPositionY = radius * Mathf.Sin(angleY * Mathf.Deg2Rad); // Y座標の計算

                square.transform.position = new Vector3(newPositionX, newPositionY, 0f);
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
