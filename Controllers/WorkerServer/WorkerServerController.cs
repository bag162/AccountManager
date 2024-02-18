using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.Controllers.WorkerServer.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.WorkerServer
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WorkerServerController : ControllerBase
    {
        private readonly IMapper mapper;
        private IWorkerServerDBService workerServerDBService;

        public WorkerServerController(IWorkerServerDBService workerServerDBService, IMapper mapper)
        {
            this.workerServerDBService = workerServerDBService;
            this.mapper = mapper;
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<GetWorkerServerDTO> returnedData = this.workerServerDBService.GetWorkerServer(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] int[] ids)
        {
            await this.workerServerDBService.DeleteServerAsync(ids);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        public async Task<string> ChangeStatus([FromBody] int[] ids)
        {
            await this.workerServerDBService.ChangeServerStatusAsync(ids);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] AddWorkerServerDTO newServer)
        {
            var addedServer = mapper.Map<DBWorkerServer>(newServer);
            await this.workerServerDBService.AddServerAsync(addedServer);
            return JsonConvert.SerializeObject(true);
        }
    }
}