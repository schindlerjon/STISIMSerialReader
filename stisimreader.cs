using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO.Ports;
using System.Threading;

namespace STISIMSerialReader_v0._1
{
    class Program
    {
        private static SerialPort port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
        private static float currentSpeed = 0;
        private static float postedSpeed = 0;

        static void Main(string[] args)
        {
           // Thread th_serial = new Thread(getSerialData);

            // continue to gather data
            Thread t = new Thread(getSerialData);
           // Thread y = new Thread(audioSplash);
            t.Start();
           // y.Start();

            while (t.IsAlive)
            {
                if (currentSpeed - postedSpeed >= 25)
                {
                    Console.Beep(800, 300);
                    //  Thread.Sleep(10);
                    Console.Write("MAIN BEEP!");
                }
                else if (currentSpeed - postedSpeed >= 15)
                {
                    Console.Beep(650, 300);
                    //  Thread.Sleep(10);
                    Console.Write("MAIN BEEP!");
                }
                else if (currentSpeed - postedSpeed >= 10)
                {
                    Console.Beep(600, 300);
                    //  Thread.Sleep(10);
                    Console.Write("MAIN BEEP!");
                }
                else if (currentSpeed - postedSpeed >= 5)
                {
                    Console.Beep(550, 300);
                    //  Thread.Sleep(10);
                    Console.Write("MAIN BEEP!");
                }
                currentSpeed = 0;
            }
            //t.Suspend();
            //t.Resume();

            t.Join();
          //  y.Join();

            Console.Write("\nPress any key to exit...");
            Console.ReadLine();
        }
        static void getSerialData()
        {
/*            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived); // Begin communications 
            port.Open(); // Enter an application loop to keep this thread alive 
        }

        private void port_DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {*/
            port.Open();

            while (port.IsOpen == true)
            {
                int q1 = port.ReadByte();
                int q2 = port.ReadByte();
                int q3 = port.ReadByte();
                int q4 = port.ReadByte();
                int vars = port.ReadByte();
                Console.WriteLine("{0} {1} {2} {3} {4}", q1, q2, q3, q4, vars);

                byte[] speed = new byte[4];
                //for (int i = 0; i < vars; i++)
                // {
                port.Read(speed, 0, 4);

                currentSpeed = BitConverter.ToSingle(speed, 0);
                Console.WriteLine("     Current Speed: {0}", currentSpeed);
                port.Read(speed, 0, 4);
                postedSpeed = BitConverter.ToSingle(speed, 0);
                Console.WriteLine("     Posted Speed: {0}", postedSpeed);
            }
        }
        static void WriteX()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.Write("X");
                Thread.Sleep(10);
            }
        }
        static void audioSplash()
        {
            if(currentSpeed >= postedSpeed)
            {
                Console.Beep(500, 100);
                Console.WriteLine("BEEP!");
                Thread.Sleep(10);
            }
        }
    }
}
