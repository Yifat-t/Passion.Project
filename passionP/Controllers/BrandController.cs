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
    public class BrandController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BrandController()
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
        // GET: Brand/List
        public ActionResult List()
        {
            //objective: communicate with our brand data api to retrive a list of brands 
            //curl https://localhost:44330/api/branddata/listbrands

            string url = "branddata/listbrands";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<BrandDto> brands = response.Content.ReadAsAsync<IEnumerable<BrandDto>>().Result;
            //Debug.WriteLine("Number of brands received: ");
            //Debug.WriteLine(brands.Count());

            return View(brands);
        }

        // GET: Brand/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our brand data api to retrive one brand.
            //curl https://localhost:44330/api/branddata/findbrand/{id}

            DetailsBrand ViewModel = new DetailsBrand();

            string url = "branddata/findbrand/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            BrandDto SelectedBrand = response.Content.ReadAsAsync<BrandDto>().Result;
            //Debug.WriteLine("brand received: ");
            //Debug.WriteLine(selectedbrand.BrandName);

            ViewModel.SelectedBrand = SelectedBrand;

            url = "productdata/listproductsforbrand/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ProductDto> RelatedProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;

            ViewModel.RelatedProducts = RelatedProducts;


            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Brand/New
        [Authorize]
        public ActionResult New()
        {
            //information about all brands in the system]
            //Get api/barnddata/listbrands
            return View();
        }

        // POST: Brand/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            GetApplicationCookie();
            Debug.WriteLine("the json payload is : ");
            //objective: add a new brand into our system using api
            //curl -H "Content-type:application/json -d @add.json https://localhost:44330/api/branddata/addbrand
            string url = "branddata/addbrand";

            string jsonpayload = jss.Serialize(brand);

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

        // GET: Brand/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            

            //the existing brand information
            string url = "branddata/findbrand/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BrandDto SelectedBrand = response.Content.ReadAsAsync<BrandDto>().Result;
            return View(SelectedBrand);


        }

        // POST: Brand/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Brand brand)
        {
            GetApplicationCookie();
            string url = "branddata/updatebrand/" + id;
            string jsonpayload = jss.Serialize(brand);
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

        // GET: Brand/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            string url = "branddata/findbrand/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BrandDto selectedbrand = response.Content.ReadAsAsync<BrandDto>().Result;
            return View(selectedbrand);
        }

        // POST: Brand/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "branddata/deletebrand/" + id;
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
