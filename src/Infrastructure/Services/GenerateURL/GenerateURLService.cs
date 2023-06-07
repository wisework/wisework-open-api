using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Infrastructure.Services.Upload.Infrastructures;

namespace WW.Application.Common.Interfaces;
public class GenerateURLService : IGenerateURLService
{
    private IConfiguration configuration = ConfigurationFactory.GetConfigApp();

    public async Task<GenerateURLModel> GenerateShortenURL(GenerateURLModel url)
    {
        var generateURL = this.configuration.GetValue<String>("GenerateURLService");

        HttpClient _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = this.setBearerAuth();
        _client.DefaultRequestHeaders.Host = "api-ssl.bitly.com";

        var content = this.getBodyJsonURL(url);
        HttpResponseMessage response = await _client.PostAsync(generateURL, content);

        var contents = JsonConvert.DeserializeObject<GenerateURLModel>(await response.Content.ReadAsStringAsync());
        return contents;
    }

    private StringContent getBodyJsonURL(GenerateURLModel url)
    {
        var jsonContent = JsonConvert.SerializeObject(new { long_url = url.long_url });
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return content;
    }

    private System.Net.Http.Headers.AuthenticationHeaderValue setBearerAuth()
    {
        var bitlyPassword = this.configuration.GetValue<String>("BitlyPassword");

        return new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Bearer", bitlyPassword
        );
    }

}
