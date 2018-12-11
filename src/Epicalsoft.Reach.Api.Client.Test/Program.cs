using Epicalsoft.Reach.Api.Client.Net;
using Epicalsoft.Reach.Api.Client.Net.Models;
using System;
using System.Linq;

namespace Epicalsoft.Reach.Api.Client.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Init();
            Console.ReadLine();
        }

        private async static void Init()
        {
            var client = ReachClientAgent.GetInstance();
            var countries = await client.GlobalContext.GetCountries();
            var classifications = await client.GlobalContext.GetClassifications();
            var roadtypes = await client.GlobalContext.GetRoadTypes();
        }
    }

    public class ReachClientAgent
    {
        private static ReachClient _reachClient;

        public static ReachClient GetInstance()
        {
            var lang = AcceptLanguage.Spanish;
            if (null == _reachClient || _reachClient.Lang != lang)
                _reachClient = new ReachClient("[ClientId]", "[ClientSecret]", lang);
            return _reachClient;
        }
    }
}