using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Email;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.Controllers.ProfileFilling.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.ProfileFilling
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProfileFillingController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private readonly ILogger<ProfileFillingController> logger;
        private IFillingDataDBService fillingDataDBService { get; set; }

        public ProfileFillingController(ILogger<ProfileFillingController> logger, IMapper mapper, IFillingDataDBService fillingDataDBService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.fillingDataDBService = fillingDataDBService;
        }

        [HttpGet]
        public async Task<string> GetList()
        {
            return JsonConvert.SerializeObject(this.fillingDataDBService.GetFillingDataNames());
        }

        [Route("{fillingDataId:int}")]
        [HttpGet]
        [Authorize(Roles = "/profilefilling/view,admin")]
        public async Task<string> Get(int fillingDataId)
        {
            return JsonConvert.SerializeObject(this.mapper.Map<GetProfileFillingDTO>(this.fillingDataDBService.GetById(fillingDataId)));
        }
        
        [HttpGet]
        [Authorize(Roles = "/profilefilling,admin")]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<ProfileFillingTableDTO> returnedData = this.fillingDataDBService.GetFillingData(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpPost]
        [Authorize(Roles = "/profilefilling/add,admin")]
        public async Task<string> Post([FromBody] AddFillingDataDTO data)
        {
            await this.fillingDataDBService.AddFillingDataAsync(data);
            return JsonConvert.SerializeObject(true);
        }

        [HttpDelete]
        [Authorize(Roles = "/profilefilling/add,admin")]
        public async Task<string> Delete(ProfileFillingTableDTO[] data)
        {
            await this.fillingDataDBService.DeleteAsync(mapper.Map<List<DBFillingData>>(data.ToList()));
            return JsonConvert.SerializeObject(true);
        }
    }
}