using UnityEngine;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    public GameObject square;  // �ړ�������I�u�W�F�N�g
    public string portName = "COM6";  // Arduino�̃|�[�g��
    public int baudRate = 115200;  // Arduino�̃{�[���[�g
    public float maxDistance = 5f; // �ő�ړ�����

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

                angleX = Map(analogValueX, 0, 1023, 0f, 360f); // X�������̊p�x
                angleY = Map(analogValueY, 0, 1023, 0f, 360f); // Y�������̊p�x

                float radius = maxDistance / 2f; // ���a
                float newPositionX = radius * Mathf.Cos(angleX * Mathf.Deg2Rad); // X���W�̌v�Z
                float newPositionY = radius * Mathf.Sin(angleY * Mathf.Deg2Rad); // Y���W�̌v�Z

                square.transform.position = new Vector3(newPositionX, newPositionY, 0f);
            }
            catch (System.Exception)
            {
                // �G���[�n���h�����O
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
