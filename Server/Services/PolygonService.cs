﻿using System;
using System.Text.Json;
using APBD_PRO.Shared;
using Newtonsoft.Json;

namespace APBD_PRO.Server.Services
{
	public class PolygonService : IPolygonService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public PolygonService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("Polygon.io");
        }

        public async Task<Aggregates> GetAggregates(string stocksTicker)
        {
            
            //TODO - change query to be flexible
            var httpResponseMessage = await _httpClient.GetAsync($"v2/aggs/ticker/{stocksTicker}/range/1/day/2021-07-22/2021-07-22");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                Aggregates aggregates = await System.Text.Json.JsonSerializer.DeserializeAsync<Aggregates>(contentStream);

                Console.WriteLine(aggregates.ticker);

                return aggregates;
            }
            return null;
        }

        public async Task<IEnumerable<BasicTicker>> GetBasicTickers(string ticker)
        {
            var httpResponse = await _httpClient.GetAsync($"v3/reference/tickers?search={ticker}&active=true&sort=ticker&order=asc&limit=10");

            if (httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("HELLO FROM ME: " + ticker);
                using var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                var res = await System.Text.Json.JsonSerializer.DeserializeAsync<Tickers>(contentStream);
                return res.results;
                //return res.results;


                //    IEnumerable<BasicTicker> result = new List<BasicTicker>();

                //    var serializer = new Newtonsoft.Json.JsonSerializer();

                //    using (var contentStream = await httpResponse.Content.ReadAsStreamAsync())
                //    using (var reader = new StreamReader(contentStream))
                //    using (var jsonReader = new JsonTextReader(reader))
                //    {
                //        jsonReader.SupportMultipleContent = true;

                //        while (jsonReader.Read())
                //        {
                //            result.Append(serializer.Deserialize<BasicTicker>(jsonReader));
                //        }

                //        return result;
                //    }
            }
            return null;
        }
    }
}