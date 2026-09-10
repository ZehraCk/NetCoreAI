using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project2_ApiConsumeUI.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace NetCoreAI.Project2_ApiConsumeUI.Controllers
{
    public class CustomerController1 : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController1(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CustomerList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7257/api/Customers");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();

                var values = JsonConvert.DeserializeObject<List<ResultCustomerDto>>(jsonData);

                return View(values ?? new List<ResultCustomerDto>());
            }

            return View(new List<ResultCustomerDto>());
        }
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createCustomerDto);
            StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");
            var responseMessage = await client.PostAsync("https://localhost:7257/api/Customers", stringContent);
            if (responseMessage.IsSuccessStatusCode) {

                return RedirectToAction("CustomerList");
            }
            return View();
        }
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMesage = await client.DeleteAsync("https://localhost:7257/api/Customers?id=" + id);
            if (responseMesage.IsSuccessStatusCode) {
                return RedirectToAction("CustomerList");

            }
            return View();

        }
        public async Task<IActionResult> UpDateCustomer(int id)
        {
        var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7257/api/Customers?id="+ id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData=await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetByIdCustomerDto>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpDateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateCustomerDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8,"application/json");
            var responseMessage = await client.PutAsync("https://localhost:7257/api/Customers", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }






    }
}
