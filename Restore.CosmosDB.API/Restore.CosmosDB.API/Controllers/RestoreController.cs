using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Restore.CosmosDB.API.Service;

using System.Text.Json;

namespace Restore.CosmosDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestoreController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public RestoreController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertData([FromQuery] string filePath)
        {
            filePath = @"E:\\OutPut\\output[0].json";
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return BadRequest("Invalid file path.");
            }

            List<ObservationsModel> records;
            try
            {
                var jsonString = await System.IO.File.ReadAllTextAsync(filePath);
                records = JsonSerializer.Deserialize<List<ObservationsModel>>(jsonString);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Error reading or deserializing file: {ex.Message}");
            }

            try
            {
                await _mongoDbService.InsertRecordsAsync(records);
                return Ok("Data inserted successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error inserting data: {ex.Message}");
            }
        }
    }
}
