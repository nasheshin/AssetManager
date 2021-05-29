using System;
using System.Data.Entity;
using System.Net.Http;
using System.Web;
using AssetManagerServer.Models;

namespace AssetManagerServer.Utils
{
    public class Logger
    {
        public static void AddLog(string message, int userId, HttpRequestBase request, DataContext database)
        {
            database.Logs.Add(new Log
            {
                Datetime = DateTime.Now, UserId = userId, HostAddress = request.UserHostAddress, 
                UserAgent = request.UserAgent, Message = message
            });
            database.SaveChangesAsync();
        }
    }
}