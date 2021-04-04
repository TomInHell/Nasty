using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Program
    {
        static List<Weekday> week;
        static string[] breakfast = { "омлет", "овсянка", "кукурузные хлопья", "йогурт" };
        static string[] lunch = { "борщ", "плов", "картошка с печенью", "лапша" };
        static string[] dinner = { "рис с отварной котлетой", "рыба с овощами", "отварная говядина с картошкой", "лазанья" };

        static void Main(string[] args)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            TcpListener server = new TcpListener(localAddr, port);
            week = new List<Weekday>();
            init_menu();
            server.Start();
            Console.WriteLine("Сервер запущен!");

            while (true)
            {
                try
                {
                    // Подключение клиента
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    // Обмен данными
                    try
                    {
                        if (stream.CanRead)
                        {
                            byte[] myReadBuffer = new byte[1024];
                            StringBuilder myCompleteMessage = new StringBuilder();
                            int numberOfBytesRead = 0;
                            do
                            {
                                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
                            }
                            while (stream.DataAvailable);
                            Byte[] responseData = request_processing(myCompleteMessage.ToString());
                            stream.Write(responseData, 0, responseData.Length);
                        }
                    }
                    finally
                    {
                        stream.Close(); 
                        client.Close();
                    }
                }
                catch
                {
                    server.Stop();
                    break;
                }
            }
        }

        static byte[] request_processing(string message)
        {
            string[] comand = message.Split(new char[] { '@' });
            Random rnd = new Random();

            switch (comand[0])
            {
                case "view":
                    {
                        return (Encoding.UTF8.GetBytes(weekday(comand[1])));
                    }
                case "rebuild":
                    {
                        week.Clear();
                        init_menu();
                        return (Encoding.UTF8.GetBytes("меню на неделю пересобранно"));
                    }
                case "edit":
                    {
                        week.Find(x => x.day == comand[1]).rebild(new Menu(comand[1], breakfast[rnd.Next(0, 3)],
                                                                    lunch[rnd.Next(0, 3)], dinner[rnd.Next(0, 3)]));
                        return (Encoding.UTF8.GetBytes(weekday(comand[1])));
                    }
                default:
                    return (Encoding.UTF8.GetBytes(""));
            }
        }

        static void init_menu()
        {
            Random rnd = new Random();

            week.Add(new Weekday(new Menu("monday", breakfast[rnd.Next(0, 3)], lunch[rnd.Next(0, 3)], dinner[rnd.Next(0, 3)])));
            week.Add(new Weekday(new Menu("tuesday", breakfast[rnd.Next(0, 3)], lunch[rnd.Next(0, 3)], dinner[rnd.Next(0, 3)])));
            week.Add(new Weekday(new Menu("wednesday", breakfast[rnd.Next(0, 3)], lunch[rnd.Next(0, 3)], dinner[rnd.Next(0, 3)])));
            week.Add(new Weekday(new Menu("thursday", breakfast[rnd.Next(0, 3)], lunch[rnd.Next(0, 3)], dinner[rnd.Next(0, 3)])));
            week.Add(new Weekday(new Menu("friday", breakfast[rnd.Next(0, 3)], lunch[rnd.Next(0, 3)], dinner[rnd.Next(0, 3)])));
            week.Add(new Weekday(new Menu("saturday", breakfast[rnd.Next(0, 3)], lunch[rnd.Next(0, 3)], dinner[rnd.Next(0, 3)])));
            week.Add(new Weekday(new Menu("sunday", breakfast[rnd.Next(0, 3)], lunch[rnd.Next(0, 3)], dinner[rnd.Next(0, 3)])));
        }

        static string weekday(string day)
        {
            return (week.Find(x => x.day == day).show());
        }
    }
}
