using Epicalsoft.Reach.Api.Client.Net;
using Epicalsoft.Reach.Api.Client.Net.Managers;
using System;

namespace Epicalsoft.Reach.Api.Client.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Test();
            Console.ReadKey();
        }

        private static async void Test()
        {
            ReachClient.InitWithUserKey("uk.dmbtdzdrcdbzbdadTdeckcYbFc2Gdkb8dpdRdLxd9b6cRcecOdWVAtbHbvyducjdqbHdAeaKdj8cLf2dDcrc6cccMDbJDcsdscScLcPchcodBdfEddcoc24dCdNAegcGd5bpdKud0dzcX");
            var countries = await GlobalScopeManager.Instance.GetCountriesAsync();
        }
    }
}