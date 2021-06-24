using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using passionP.Models;
using passionP.Models.ViewModels;
using System.Web.Script.Serialization;


namespace passionP.Controllers
{
    public class RetailerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RetailerController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };


            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44330/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;

        }

        // GET: Retailer/List
        public ActionResult List()
        {
            //objective: communicate with our Retailer data api to retrieve a list of Retailers
            //curl https://localhost:44330/api/Retailerdata/listretailers


            string url = "retailerdata/listretailers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<RetailerDto> Retailers = response.Content.ReadAsAsync<IEnumerable<RetailerDto>>().Result;
            //Debug.WriteLine("Number of Retailers received : ");
            //Debug.WriteLine(Retailers.Count());


            return View(Retailers);
        }

        // GET: Retailer/Details/5
        public ActionResult Details(int id)
        {
            DetailsRetailer ViewModel = new DetailsRetailer();

            //objective: communicate with our Retailer data api to retrieve one Retailer
            //curl https://localhost:44330/api/retailerdata/findretailer/{id}

            string url = "retailerdata/findretailer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            RetailerDto SelectedRetailer = response.Content.ReadAsAsync<RetailerDto>().Result;
            //Debug.WriteLine("Retailer received : ");
            //Debug.WriteLine(SelectedRetailer.RetailerName);

            ViewModel.SelectedRetailer = SelectedRetailer;

            //show all products sold under this retailer
            url = "productdata/listproductsforretailer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ProductDto> SoldProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;

            ViewModel.SoldProducts = SoldProducts;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Retailer/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Retailer/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Retailer Retailer)
        {
            GetApplicationCookie();
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Retailer.RetailerName);
            //objective: add a new Retailer into our system using the API
            //curl -H "Content-Type:application/json" -d @Retailer.json https://localhost:44330/api/Retailerdata/addRetailer 
            string url = "retailerdata/addretailer";


            string jsonpayload = jss.Serialize(Retailer);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Retailer/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "retailerdata/findretailer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RetailerDto selectedRetailer = response.Content.ReadAsAsync<RetailerDto>().Result;
            return View(selectedRetailer);
        }

        // POST: Retailer/Update/5
        [Authorize]
        [HttpPost]
        public ActionResult Update(int id, Retailer Retailer)
        {

            GetApplicationCookie();
            string url = "retailerdata/updateretailer/" + id;
            string jsonpayload = jss.Serialize(Retailer);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Retailer/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "retailerdata/findretailer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RetailerDto selectedRetailer = response.Content.ReadAsAsync<RetailerDto>().Result;
            return View(selectedRetailer);
        }

        // POST: Retailer/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "retailerdata/deleteretailer/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}