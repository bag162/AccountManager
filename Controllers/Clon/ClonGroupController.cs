using AutoMapper;
using BASAccountManager.Controllers.Clon.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Clon
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClonGroupController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private IClonGroupDBService clonGroupDBService { get; set; }

        public ClonGroupController(IClonGroupDBService clonGroupDBService, IMapper mapper)
        {
            this.clonGroupDBService = clonGroupDBService;
            this.mapper = mapper;
        }

        [HttpGet]
        public string GetList()
        {
            return JsonConvert.SerializeObject(this.clonGroupDBService.GetGroupNames());
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<GetClonGroupDTO> returnedData = this.clonGroupDBService.GetGroups(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] AddClonGroupDTO group)
        {
            await this.clonGroupDBService.AddClonGroupAsync(group);
            return JsonConvert.SerializeObject("true");
        }
    }
}