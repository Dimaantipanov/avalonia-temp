using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FenceFactory.Desktop.Services.Models.Responses;

namespace FenceFactory.Desktop.Services.Network;

public class StaffApiService
{
    private readonly HttpClient _httpClient;
    private readonly NetworkSession _session;

    public StaffApiService(HttpClient httpClient, NetworkSession session)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    private void ApplyAuthorization()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
        if (!string.IsNullOrEmpty(_session.AuthToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _session.AuthToken);
        }
    }

    public async Task<List<UserRowModel>> GetStaffAsync()
    {
        ApplyAuthorization();
        var result = await _httpClient.GetFromJsonAsync<List<UserRowModel>>("/api/admin/get-staff");
        return result ?? new List<UserRowModel>();
    }

    public async Task DismissStaffAsync(string email)
    {
        ApplyAuthorization();
        var payload = new List<string> { email };
        
        using var response = await _httpClient.PostAsJsonAsync("/api/admin/dismiss-staff", payload);

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(errorContent, null, response.StatusCode);
        }
    }
}