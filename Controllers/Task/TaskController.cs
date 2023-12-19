using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.FBTask
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskController : ControllerBase
    {
        private IMapper mapper;
        private readonly ILogger<TaskController> logger;
        private ITaskDBService TaskDBService { get; set; }

        public TaskController(ILogger<TaskController> logger, IMapper mapper, ITaskDBService TaskDBService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.TaskDBService = TaskDBService;
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<TaskDTO> returnedData = this.TaskDBService.GetTask(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }
    }
}