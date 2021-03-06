﻿using Hogwarts.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hogwarts.Client.Services
{
    public class StaffDataService
    {
        private readonly HttpClient _httpClient;
        public StaffDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<StaffDto>>
                (await _httpClient.GetStreamAsync($"api/staff"), 
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<StaffDto> GetStaffByIdAsync(int staffId)
        {
            return await JsonSerializer.DeserializeAsync<StaffDto>
                (await _httpClient.GetStreamAsync($"api/staff/{staffId}"), 
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<StaffDto> AddStaff(StaffForCreationDto staff)
        {
            var employeeJson =
                new StringContent(JsonSerializer.Serialize(staff), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/staff", employeeJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<StaffDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

    }

}

