using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LabActiveDirectory.Entidades;

public partial class ApiClient
{

    private readonly HttpClient _httpClient;
    private Uri BaseEndpoint { get; set; }

    public ApiClient(Uri baseEndpoint)
    {
        if (baseEndpoint == null)
        {
            throw new ArgumentNullException("baseEndpoint");
        }
        BaseEndpoint = baseEndpoint;
        _httpClient = new HttpClient();
    }

    /// <summary>  
    /// Common method for making GET calls  
    /// </summary>  
    public ValidacionUsuario Get(Uri requestUrl)
    {
        addHeaders();
        var response = _httpClient.GetAsync(requestUrl).Result;

        if (response.IsSuccessStatusCode)
        {
            var responseContent = response.Content;
            string responseString = responseContent.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<ValidacionUsuario>(responseString);
        }
        else
        {
            throw new Exception("Error al realizar petición web - " + response.Content.ReadAsStringAsync().Result);
        }

    }

    /// <summary>  
    /// Common method for making POST calls  
    /// </summary>  
    public ValidacionUsuario Post(string requestUrl, HttpContent content)
    {
        addHeaders();
        var response = _httpClient.PostAsync(requestUrl, content).Result;

        if (response.IsSuccessStatusCode)
        {
            var responseContent = response.Content;
            string responseString = responseContent.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<ValidacionUsuario>(responseString);
        }
        else
        {
            throw new Exception("Error al realizar petición web - " + response.Content.ReadAsStringAsync().Result);
        }
    }
    private async Task<ValidacionUsuario> PostAsync<T1, T2>(Uri requestUrl, T2 content)
    {
        addHeaders();
        var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content));
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ValidacionUsuario>(data);
    }

    public Uri CreateRequestUri(string relativePath, string queryString = "")
    {
        var endpoint = new Uri(BaseEndpoint, relativePath);
        var uriBuilder = new UriBuilder(endpoint);
        uriBuilder.Query = queryString;
        return uriBuilder.Uri;
    }

    private HttpContent CreateHttpContent<T>(T content)
    {
        var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static JsonSerializerSettings MicrosoftDateFormatSettings
    {
        get
        {
            return new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
        }
    }

    private void addHeaders()
    {

    }
}