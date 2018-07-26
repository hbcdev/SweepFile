using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;


namespace SweepFile.Models
{
    public class CallFundcode
    {

        public string fundcode {get;set;}
        public string date_off {get;set;}
    }

    class httpAPI {
      /* static HttpClient client = new HttpClient();

      


        static async Task<CallFundcode> GetProductAsync()
        {
            CallFundcode product = null;
            HttpResponseMessage response = await client.GetAsync("http://api.hbc.in.th/api/fund/");
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<CallFundcode>();
            }
            return product;
        }
        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }
        static async Task RunAsync()
        {

            HttpClient client = new HttpClient();
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://api.hbc.in.th/api/fund/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                CallFundcode product = new CallFundcode
                {
                    fundcode = "Gizmo",
                    date_off = "Widgets"
                };
                // Get the product
                HttpResponseMessage response = await client.GetAsync("http://api.hbc.in.th/api/fund/");
                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadAsAsync<CallFundcode>();
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }*/
    }
}
