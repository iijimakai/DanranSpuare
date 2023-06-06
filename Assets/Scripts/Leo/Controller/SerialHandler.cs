using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UniRx;

namespace Leo
{
    public class SerialHandler : MonoBehaviour
    {
        public delegate void SerialDataReceivedEventHandler(string message);
        public event SerialDataReceivedEventHandler OnDataReceived;
        public string portName = "COM6";
        public int baudRate = 115200;
        private SerialPort serialPort_;
        private Thread thread_;
        private bool isRunning_ = false;
        private string message_;
        private Subject<bool> isNewMessageReceived = new Subject<bool>();

        void Awake()
        {
            Open();
        }

        private void Start()
        {
            isNewMessageReceived.Where(flag => flag)
                .Subscribe(flag => OnDataReceived(message_))
                .AddTo(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }

        void OnDestroy()
        {
            Close();
        }

        private void OnApplicationQuit()
        {
            Close();
        }

        private void Open()
        {
            serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);

            //または
            //serialPort_ = new SerialPort(portName, baudRate);
            serialPort_.Open();

            isRunning_ = true;

            thread_ = new Thread(Read);
            thread_.Start();
        }

        private void Close()
        {
            isRunning_ = false;
            serialPort_.Close();

            // if (thread_ != null && thread_.IsAlive)
            // {
            //     thread_.Join();
            // }

            // if (serialPort_ != null && serialPort_.IsOpen)
            // {
            //     serialPort_.Close();
            //     serialPort_.Dispose();
            //     isNewMessageReceived.Dispose();
            // }
        }

        private void Read()
        {
            while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
            {
                try
                {
                    message_ = serialPort_.ReadLine();
                    isNewMessageReceived.OnNext(true);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }

        public void Write(string message)
        {
            try
            {
                serialPort_.Write(message);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
}