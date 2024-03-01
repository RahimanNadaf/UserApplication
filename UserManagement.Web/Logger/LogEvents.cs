using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using UserManagement.Web.Models.Users;
using Newtonsoft.Json;

namespace UserManagement.Web.Logger;

public static class LogEvents
{
    private static readonly JsonSerializerOptions _options =
       new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    public static void LogToFile(Logs log, IWebHostEnvironment env)
    {
        bool exist = Directory.Exists(env.WebRootPath + "\\" + "LogFolder");
        if (!exist)
        {
            Directory.CreateDirectory(env.WebRootPath + "\\" + "LogFolder");
        }
        StreamWriter swLog;
        string LogPath = "";

        string FileName = DateTime.Now.ToString("ddMMYYYY")+"log.json";

        LogPath =   Path.Combine(env.WebRootPath+"\\"+ "LogFolder", FileName);

        if (!File.Exists(LogPath))
        {
            swLog = new StreamWriter(LogPath);
            swLog.WriteLine("[");
            var jsonString = JsonConvert.SerializeObject(log, Formatting.Indented);
            swLog.WriteLine(jsonString);
        }
        else
        {
            swLog = File.AppendText(LogPath);
            var jsonString = JsonConvert.SerializeObject(log, Formatting.Indented);
            swLog.WriteLine(",");
            swLog.WriteLine(jsonString);
        }              
        swLog.Close();
    }

    public static IEnumerable<Logs>? ReadLogs(IWebHostEnvironment env)
    {
        bool exist = Directory.Exists(env.WebRootPath + "\\" + "LogFolder");
        string FileName = DateTime.Now.ToString("ddMMYYYY") + "log.json";
        var LogPath =   Path.Combine(env.WebRootPath+"\\"+ "LogFolder", FileName);
        if (!exist)
        {
            Directory.CreateDirectory(env.WebRootPath + "\\" + "LogFolder");
        }
        if (!File.Exists(LogPath))
        {
            return null;
        }
        string jsonData = File.ReadAllText(LogPath);
        jsonData = jsonData + "]";
        var response = JsonConvert.DeserializeObject<IEnumerable<Logs>>(jsonData);
        Console.WriteLine(response);
        return response;

    }

}
