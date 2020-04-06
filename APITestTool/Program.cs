using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace APITestTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var action = args[0];
            Stopwatch stopOverall = new Stopwatch();            
            var datetimeToString = DateTime.Now.ToString("MMddyyyyHHmmss");
            string filename = "C:\\data\\app\\apiTest\\log";
            Console.WriteLine("Start of Test");
            Log("Start of Test", filename, "1");

            List<long> arrElapsed = new List<long>();
            stopOverall.Start();
            for (int i = 0; i <= 1000; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = "";
                switch(action) 
                {
                    case "auth":
                        response = testAuth(args[1], args[2]);
                        break;
                    case "version":
                        response = testVer();
                        break;
                }                
                stopwatch.Stop();
                arrElapsed.Add(stopwatch.ElapsedMilliseconds);
                Console.WriteLine("Test " + i + " response: " + response + " elapsed:" + stopwatch.ElapsedMilliseconds + " ms" + " average: " + arrElapsed.Average() + " ms");
                Log("Test " + i + " response: " + response + " elapsed:" + stopwatch.ElapsedMilliseconds + " ms" + " average: " + arrElapsed.Average() + " ms", filename,"1");
            }
            stopOverall.Stop();
            Console.WriteLine("End of test, total time:" + stopOverall.Elapsed.TotalSeconds + " sec");
        }
        private static string testAuth(string screenName, string password)
        {
            string payload = "{\"action\":\"auth\",\"session_key\":\"new\",\"data\":{\"ScreenName\":\"" + screenName + "\",\"Password\":\"" + password + "\"}}";
            var response = makeAPICall("request", payload);
            return response;
        }

        private static string testVer()
        {
            var response = makeAPICall("get", "/version");
            return response;
        }

        private static string makeAPICall(string action, string payLoad)
        {            
            var response = "";
            IRestRequest request = new RestRequest();

            var client = new RestClient("http://xxxx/api");

            switch (action)
            {
                case "request":
                    var jsonBody = payLoad;                    
                    request = new RestSharp.RestRequest("/Request", RestSharp.Method.POST) { RequestFormat = RestSharp.DataFormat.Json }
                    .AddJsonBody(jsonBody);                    
                    break;
                case "get":
                    request = new RestSharp.RestRequest(payLoad, RestSharp.Method.GET) { RequestFormat = RestSharp.DataFormat.Json };
                    break; 
            }

            var res = client.Execute(request);
            response = res.Content;
            return response;
        }        
        private static void Log(string what, string filename, string which)
        {
            using (StreamWriter writer = File.AppendText(filename + "-" + which + ".txt"))
            {
                writer.WriteLine(what);
            }
        }
    }
}
