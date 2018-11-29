//778568242:AAFAuMifVwnWFQOPB4fjymPNJ4Wy6ukBRrk

using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using com.LandonKey.SocksWebProxy;
using com.LandonKey.SocksWebProxy.Proxy;
using System.Net;
using System.Net.Sockets;
using Npgsql;
using telegramBot;

namespace telegramBot
{
    public class Program
    {
        static ITelegramBotClient botClient;
        public static int Count = 0;
        public static Dictionary<int, int> Users = new Dictionary<int, int>();
        public static Student student = new Student();
        public static StudentService servise = new StudentService();

        static void Main()
        {
            botClient = new TelegramBotClient(
                "778568242:AAFAuMifVwnWFQOPB4fjymPNJ4Wy6ukBRrk",
                new SocksWebProxy(
                        new ProxyConfig(
                            IPAddress.Parse("127.0.0.1"),
                            GetNextFreePort(),
                            IPAddress.Parse("185.20.184.217"),
                            3693,
                            ProxyConfig.SocksVersion.Five,
                            "userid66n9",
                            "pSnEA7M"),
                        false)
            );

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Привет, юзер! Я {me.Id} и меня зовут {me.FirstName}. Напиши мне что-нибудь:)"
            );
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Save"),
                            new KeyboardButton("Delete"),
                            new KeyboardButton("Update"),
                            new KeyboardButton("GetAll"),
                        }
                    }
                };
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Что вы хотите сделать?",
                    replyMarkup: rkm);
                return;
            }

            if (e.Message.Text == "Update")
            {

            }

            if (e.Message.Text == "Delete")
            {
                servise.Delete(e.Message.From.Id);
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Удалены");

                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Save"),
                            new KeyboardButton("Delete"),
                            new KeyboardButton("GetAll"),
                        }
                    }
                };
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Что вы хотите сделать?",
                    replyMarkup: rkm);
                return;
            }

            if (e.Message.Text == "Save")
            {
                student = new Student();
                Count = 0;
                student.SenderId = e.Message.From.Id;
                Count++;

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Как вас зовут?(Имя)",
                    replyMarkup: new ReplyKeyboardRemove() { });
                return;
            }

            if (Count == 1)
            {
                student.Name = e.Message.Text;
                Count++;

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Сколько тебе лет?",
                    replyMarkup: new ReplyKeyboardRemove() { });
                return;
            }

            if (Count == 2)
            {
                student.Age = e.Message.Text;
                Count++;

                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("701"),
                            new KeyboardButton("702"),
                            new KeyboardButton("703"),
                            new KeyboardButton("704"),
                        }
                    }
                };
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Из какой ты группы?",
                    replyMarkup: rkm);
                return;
            }

            if (Count == 3)
            {
                student.Groop = e.Message.Text;
                Count++;

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Придумайте Id для этого студента",
                    replyMarkup: new ReplyKeyboardRemove() { });
                return;
            }

            if (Count == 4)
            {
                student.Id = e.Message.Text;
                Count = 0;

                servise.Save(student);
                await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "Сохранён");
                var rkm = new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("Save"),
                            new KeyboardButton("Delete"),
                            new KeyboardButton("GetAll"),
                        }
                    }
                };
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Что вы хотите сделать?",
                    replyMarkup: rkm);
                return;
            }
        }

        private static void WriteInBd(MessageEventArgs e)
        {
            var messege = e.Message.Text;
            var infoOfStudent = messege.Split(' ');


            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=student";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO student (name, last_name, sender_id) VALUES (@name, @last_name, @sender_id)";
                    cmd.Parameters.AddWithValue("name", infoOfStudent[0]);
                    cmd.Parameters.AddWithValue("last_name", infoOfStudent[1]);
                    cmd.Parameters.AddWithValue("sender_id", e.Message.From.Id);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        private static int GetNextFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            return port;
        }
    }
}
