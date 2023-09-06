using UnityEngine;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    public GameObject player;  // �ړ�������I�u�W�F�N�g
    [Header("USB�̃|�[�g�ԍ�")] public string portName = "COM6";  // Arduino�̃|�[�g��
    [Header("��̃{�[�h���[�g�Ȃ̂ŕύX���Ȃ���OK")] public int baudRate = 115200;  // Arduino�̃{�[�h���[�g
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
                string data = serialPort.ReadLine();
                string[] values = data.Split(',');

                //var data = message.Split(
                //    new string[] { "," }, System.StringSplitOptions.None); // �J���}�ŕ�������

                if (values.Length == 2)
                {
                    int analogValueX = int.Parse(values[0]);
                    int analogValueY = int.Parse(values[1]);

                    //float angleX = Map(analogValueX, 0, 1023, 0f, 360f); // X�������̊p�x
                    //float angleY = Map(analogValueY, 0, 1023, 0f, 360f); // Y�������̊p�x

                    //float radius = maxDistance / 2f; // ���a
                    //float newPositionX = radius * Mathf.Cos(angleX * Mathf.Deg2Rad); // X���W�̌v�Z
                    //float newPositionY = radius * Mathf.Sin(angleY * Mathf.Deg2Rad); // Y���W�̌v�Z

                    // X�����̐���
                    if (analogValueX == 514 || analogValueX == 515)
                    {
                        analogValueX = 0;
                    }
                    else if (analogValueX < 514 || analogValueX < 1023)
                    {
                        analogValueX = (analogValueX - 514) / 1000;
                    }

                    // Y�����̐���
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
