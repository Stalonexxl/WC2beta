using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace Strategiya
{
    class Client
    {
        static string userName;
        private const string host = "192.168.1.239";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

        public Client(string UserName)
        {
            userName = UserName;
            client = new TcpClient();

                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                //// запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
        }

        // отправка сообщений
        public void SendMessage(string msg)
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }
        static void ReceiveMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            string message = builder.ToString();
            Form1.formPointer.ClientMessage(message);
        }
    }
}
