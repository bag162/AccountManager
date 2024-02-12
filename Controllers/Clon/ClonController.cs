using AutoMapper;
using BASAccountManager.Controllers.Clon.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Clon
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClonController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private IClonDBService clonDBService { get; set; }
        public ClonController(IMapper mapper,
            IClonDBService clonDBService)
        {
            this.mapper = mapper;
            this.clonDBService = clonDBService;
        }


        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<GetClonDTO> returnedData = this.clonDBService.GetClones(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [Route("{groupId:int}")]
        [HttpGet]
        public string Get(int start, int length, int draw, int groupId)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<GetClonDTO> returnedData = this.clonDBService.GetClonesByGroup(start, length, searchData.First(), groupId);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [Route("{clonId:int}")]
        [HttpGet]
        public string GetData(int clonId)
        {
            return JsonConvert.SerializeObject(this.clonDBService.GetClonData(clonId));
        }

        [HttpDelete]
        public async Task<string> Delete(int[] ids)
        {
            await this.clonDBService.DelteteClonesByIdAsync(ids);
            return JsonConvert.SerializeObject(true);
        }
    }
}
