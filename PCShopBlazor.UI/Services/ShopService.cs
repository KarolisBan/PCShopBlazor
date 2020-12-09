using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
//using RoomRental.Application.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PCShopBlazor.Helpers;
using System.Net;
using PCShopBlazor.Extension.ViewModels;
using PCShopBlazor.Variables.Data;
using PCShopBlazor.Controllers;
using System;
//using RoomRental.Domain.Entities.RoomRental;

namespace PCShopBlazor.UI.Services
{
    public class ShopService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl;
        public LoginResult LoggedInUser { get; private set; }
        public bool IsLoggedIn => LoggedInUser != null;// || (await _localStorage.GetItemAsync<LoginResult>("user")) != null;

        public event Action ReloadUI;

        public int? UserId { get; private set; }

        public ShopService(IConfiguration config, HttpClient client)
        {
            _client = client;
            baseUrl = "https://localhost:44331";
        }

        //Login:
        public async Task<IEnumerable<User>> LoginBad(LoginRequest loginRequest)
        {
            System.Net.Http.StringContent json = RequestHelper.GetStringContentFromObject(loginRequest);
            var response = await _client.PostAsync($"{baseUrl}/api/Account/login", (json));
            return JsonConvert.DeserializeObject<IEnumerable<User>>(response.ToString());
        }

        public async Task<bool> Login(LoginRequest credentials)
        {
            var response = await _client.PostAsync($"{baseUrl}/api/Account/login", RequestHelper.GetStringContentFromObject(credentials));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                LoggedInUser = JsonConvert.DeserializeObject<LoginResult>(response.Content.ReadAsStringAsync().Result);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoggedInUser.AccessToken);
                var value = 1; //https://localhost:44331/api/Users/
                UserId = Convert.ToInt32(value);
                ReloadUI?.Invoke();
                return true;
            }

            return false;
        }

        public async Task<bool> CheckAdmin()
        {
            var isAdmin = await _client.GetStringAsync($"{baseUrl}/api/CheckAdmin");
            return bool.Parse(isAdmin);
        }

        public async Task<bool> CheckLogin()
        {
            var isLoggedin = await _client.GetStringAsync($"{baseUrl}/api/CheckLogin");
            return bool.Parse(isLoggedin);
        }

        public async Task<UserViewModel> GetLoggedUser()
        {
            string json = await _client.GetStringAsync($"{baseUrl}/api/GetLoggedUser");
            return JsonConvert.DeserializeObject<UserViewModel>(json);
        }

        //Users:

        public async Task<IEnumerable<UserViewModel>> GetUsersAsync()
        {
            string json = await _client.GetStringAsync($"{baseUrl}/api/Users");
            return JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(json);
        }

        public async Task<int> InsertUserAsync(User user)
        {
            var response = await _client.PostAsync($"{baseUrl}/api/Users", RequestHelper.GetStringContentFromObject(user));
            return JsonConvert.DeserializeObject<Part>(response.Content.ReadAsStringAsync().Result).Id;
        }

        //Parts:

        public async Task<IEnumerable<PartViewModel>> GetPartsAsync()
        {
            string json = await _client.GetStringAsync($"{baseUrl}/api/Parts");
            return JsonConvert.DeserializeObject<IEnumerable<PartViewModel>>(json);
        }

        public async Task<PartViewModel> GetPartsAsync(int partId)
        {
            var response = await _client.GetAsync($"{baseUrl}/api/Parts/{partId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<PartViewModel>(json);
            }
            return null;
        }

        public async Task<int> InsertPartAsync(PartViewModel part)
        {
            var response = await _client.PostAsync($"{baseUrl}/api/Parts", RequestHelper.GetStringContentFromObject(part));
            return JsonConvert.DeserializeObject<PartViewModel>(response.Content.ReadAsStringAsync().Result).Id;
        }

        public async Task<HttpResponseMessage> UpdatePartAsync(PartViewModel part)
        {
            return await _client.PutAsync($"{baseUrl}/api/Parts/{part.Id}", RequestHelper.GetStringContentFromObject(part));
        }

        public async Task<HttpResponseMessage> DeletePartAsync(int partId)
        {
            return await _client.DeleteAsync($"{baseUrl}/api/Parts/{partId}");
        }

        //Orders:

        public async Task<IEnumerable<OrderViewModel>> GetOrdersAsync()
        {
            string json = await _client.GetStringAsync($"{baseUrl}/api/Orders");
            return JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(json);
        }

        public async Task<OrderViewModel> GetOrdersAsync(int orderId)
        {
            var response = await _client.GetAsync($"{baseUrl}/api/Orders/{orderId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<OrderViewModel>(json);
            }
            return null;
        }

        public async Task<int> InsertOrderAsync(OrderViewModel order)
        {
            var response = await _client.PostAsync($"{baseUrl}/api/Orders", RequestHelper.GetStringContentFromObject(order));
            return JsonConvert.DeserializeObject<OrderViewModel>(response.Content.ReadAsStringAsync().Result).Id;
        }

        public async Task<HttpResponseMessage> UpdateOrderAsync(OrderViewModel order)
        {
            return await _client.PutAsync($"{baseUrl}/api/Orders/{order.Id}", RequestHelper.GetStringContentFromObject(order));
        }

        public async Task<HttpResponseMessage> DeleteOrderAsync(int orderId)
        {
            return await _client.DeleteAsync($"{baseUrl}/api/Orders/{orderId}");
        }

        //Computers:

        public async Task<IEnumerable<ComputerViewModel>> GetComputersAsync()
        {
            string json = await _client.GetStringAsync($"{baseUrl}/api/Computers");
            return JsonConvert.DeserializeObject<IEnumerable<ComputerViewModel>>(json);
        }

        public async Task<ComputerViewModel> GetComputersAsync(int computerId)
        {
            var response = await _client.GetAsync($"{baseUrl}/api/Computers/{computerId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ComputerViewModel>(json);
            }
            return null;
        }

        public async Task<int> InsertComputerAsync(ComputerViewModel computer)
        {
            var response = await _client.PostAsync($"{baseUrl}/api/Computers", RequestHelper.GetStringContentFromObject(computer));
            return JsonConvert.DeserializeObject<ComputerViewModel>(response.Content.ReadAsStringAsync().Result).Id;
        }

        public async Task<HttpResponseMessage> UpdateComputerAsync(ComputerViewModel computer)
        {
            return await _client.PutAsync($"{baseUrl}/api/Computers/{computer.Id}", RequestHelper.GetStringContentFromObject(computer));
        }

        public async Task<HttpResponseMessage> DeleteComputerAsync(int computerId)
        {
            return await _client.DeleteAsync($"{baseUrl}/api/Computers/{computerId}");
        }
    }
}
