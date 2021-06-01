using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace Strategiya
{
    /*public class Client
   {
       static string userName;
       private const string host = "192.168.1.239";
       private const int port = 8888;
       static TcpClient client;
       static NetworkStream stream;
       public static string message = null;
       Thread receiveThread;
       public Client(string UserName)
       {
           userName = UserName;
           client = new TcpClient();
           try
           {
               client.Connect(host, port); //подключение клиента
               stream = client.GetStream(); // получаем поток

               string message = userName;
               byte[] data = Encoding.Unicode.GetBytes(message);
               stream.Write(data, 0, data.Length);

               //// запускаем новый поток для получения данных
               receiveThread = new Thread(new ThreadStart(ReceiveMessage));
               receiveThread.Start(); //старт потока
           }
           catch (Exception ex)
           {
               Form1.formPointer._Log("ошибка подключения");
               Disconnect();
           }
       }
       // отправка сообщений
       public void SendMessage(string msg)
       {
           byte[] data = Encoding.Unicode.GetBytes(msg);
           stream.Write(data, 0, data.Length);
       }
       void ReceiveMessage()
       {
           while (true)
           {
               try
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

                   string mes = builder.ToString();
                   message = mes;
               }
               catch
               {
                   Form1.formPointer._Log("ошибка подключения");
                   //Disconnect();
               }
           }
       }
      void Disconnect()
       {
           if (stream != null)
               stream.Close();//отключение потока
           if (client != null)
               client.Close();//отключение клиента
           Environment.Exit(0); //завершение процесса
       }
}*/
}
