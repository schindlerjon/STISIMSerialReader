using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO.Ports;
using System.Threading;

namespace STISIMSerialReader_v0._1
{
    class stisimreader
    {
		// Initialize Serial Port
        private static SerialPort port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
		// We would like to run out program on COM1
		// the baud rate for this should be set to 9600 within STISIM
		// The Parity should be set to 0 within STISIM
		// The StopBits within STISIM should be set to 1
		
		// The following floats should be our recieved values or speed
        private static float currentSpeed = 0;
        private static float postedSpeed = 0;

        static void Main(string[] args)
        {
            // We want to contunuously gather data
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
                int q1 = port.ReadByte();   // 255
                int q2 = port.ReadByte();   // 255
                int q3 = port.ReadByte();   // 255
                int q4 = port.ReadByte();   // 255
                int vars = port.ReadByte(); // Number fo variables to read
				
                Console.WriteLine("{0} {1} {2} {3} {4}", q1, q2, q3, q4, vars);

                byte[] speed = new byte[4]; // Initialize byte object to convert later
				
				// Get the current speed
                port.Read(speed, 0, 4);
                currentSpeed = BitConverter.ToSingle(speed, 0);
                Console.WriteLine("     Current Speed: {0}", currentSpeed);
				
				// Get the posted speed
                port.Read(speed, 0, 4);
                postedSpeed = BitConverter.ToSingle(speed, 0);
                Console.WriteLine("     Posted Speed: {0}", postedSpeed);
				
				// NOTE: The order in which the "speeds" are sent from STISIM are important
				//       for current spee then posted speed, the first line in the STISIM file
				//       should look about as follows "0 SOUT 36 45"
            }
        }
		
		// Implemented for debugging purposes
        static void WriteX()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.Write("X");
                Thread.Sleep(10);
            }
        }
		
		// Implemented for debugging purposes and future projects dealing with ambient auditory displays
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
