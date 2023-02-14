using DTYelpAPI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed
{
    class Program
    {
        private const string BASE_ADDRESS = "https://api.yelp.com";
        private const string API_VERSION = "v3";
        static void Main(string[] args)
        {
            Junk();
        }

        static void Junk()
        {
            var client = new RestClient("https://api.yelp.com/v3/businesses/search?term=delis&latitude=37.786882&longitude=-122.399972");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "6cb09d04-c1a8-e103-9ab7-4874c47d31a1");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer OMJiy42P85UdkilYO-V8dC98cA9Y8HQlsr5QVijwltwqeSdNGPmgFfL7_921BDgzl_8z-sfe8i5zroWt_vogzfq2th4XlufZ2xqASsOkQ0sOBpoaRlemA6UM9EyjWnYx");
            IRestResponse response = client.Execute(request);
            var mycon = new DTYelpMuncherTools();
            var mystuff = mycon.BuildYelpReturn(response.Content);
            Console.ReadLine();


        }
    }
}
