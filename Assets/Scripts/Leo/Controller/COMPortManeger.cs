using UnityEngine;
using System.IO.Ports;

namespace Leo
{
    public class COMPortManeger
    {
        public string GetPortNumber()
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports != null)
            {
                foreach (string port in ports)
                {
                    Debug.Log(port);
                }
                return ports[0];
            }
            return null;
        }
    }
}