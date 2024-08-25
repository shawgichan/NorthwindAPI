using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NorthwindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatFactsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CatFactsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        private async Task<List<CatFact>> FetchAllFacts()
        {
            var response = await _httpClient.GetAsync("https://cat-fact.herokuapp.com/facts");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CatFact>>(content);
        }

        [HttpGet("verified")]
        public async Task<IActionResult> GetVerifiedFacts()
        {
            var facts = await FetchAllFacts();
            var verifiedFacts = facts.Where(f => f.Status.Verified).ToList();
            return Ok(verifiedFacts);
        }

        [HttpGet("created")]
        public async Task<IActionResult> GetFactsCreatedOn([FromQuery] DateTime date)
        {
            var facts = await FetchAllFacts();
            var factsCreatedOn = facts.Where(f => f.CreatedAt.Date == date.Date).ToList();
            return Ok(factsCreatedOn);
        }

        [HttpGet("updated")]
        public async Task<IActionResult> GetFactsUpdatedOn([FromQuery] DateTime date)
        {
            var facts = await FetchAllFacts();
            var factsUpdatedOn = facts.Where(f => f.UpdatedAt.Date == date.Date).ToList();
            return Ok(factsUpdatedOn);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFacts()
        {
            var facts = await FetchAllFacts();
            return Ok(facts);
        }
    }

    public class CatFact
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("upvotes")]
        public int Upvotes { get; set; }

        [JsonProperty("userUpvoted")]
        public object UserUpvoted { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }

    public class Status
    {
        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("sentCount")]
        public int SentCount { get; set; }
    }
}