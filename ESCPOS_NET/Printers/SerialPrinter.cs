using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public class SerialPrinter : BasePrinter
    {
        private readonly SerialPort _serialPort;

        private Queue<byte> recievedData = new Queue<byte>();
        // public SerialPort Port { get; set; }
        public event EventHandler<byte[]> PaperStatus;

        public SerialPrinter(string portName, int baudRate)
            : base()
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.ReadBufferSize = 1048576;
            _serialPort.ReadTimeout = 100;
            _serialPort.DtrEnable = true;
            _serialPort.RtsEnable = true;
            _serialPort.DataReceived += PortInform;
            _serialPort.Open();
            Port = _serialPort;
            SerialPrinter = this;
            Writer = new BinaryWriter(_serialPort.BaseStream);
            Reader = new BinaryReader(_serialPort.BaseStream);
        }
        

        private void PortInform(object s, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[_serialPort.BytesToRead];
            var si = _serialPort.Read(data, 0, data.Length);
            PaperStatus!.Invoke(this, data);
            // processData();
        }

        protected override void OverridableDispose()
        {
            _serialPort?.Close();
            _serialPort?.Dispose();
            Task.Delay(250).Wait(); // Based on MSDN Documentation, should sleep after calling Close or some functionality will not be determinant.
        }
    }
}