using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.SMS_Services.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    [Authorize(Roles = "/smsservice")]
    public class SMSServiceController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private ISMSServiceDB smsServiceDB { get; set; }
        private readonly ILogger<SMSServiceController> logger;

        public SMSServiceController(ILogger<SMSServiceController> logger, IMapper mapper, ISMSServiceDB smsServiceDB)
        {
            this.logger  = logger;
            this.mapper = mapper;
            this.smsServiceDB = smsServiceDB;
        }

        [HttpGet]
        public string GetList()
        {
            var smsServices = this.smsServiceDB.GetSMSService().Select(x => x.Id + ":" + x.ServiceName + ":" + x.ServiceType.ToString()).ToList();
            return JsonConvert.SerializeObject(smsServices);
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<SMSServiceDTO> returnedData = this.smsServiceDB.GetSMSService(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] SMSServiceDTO[] accounts)
        {
            await this.smsServiceDB.RemoveSMSServiceAsync(mapper.Map<List<DBSMSActivation>>(accounts.ToList()));
            return JsonConvert.SerializeObject("true");
        }

        [HttpPost]
        public async Task<string> Post([FromBody] SMSServiceDTO[] accounts)
        {
            await this.smsServiceDB.AddSMSServiceAsync(mapper.Map<List<DBSMSActivation>>(accounts.ToList()));
            return JsonConvert.SerializeObject("true");
        }

        [HttpPut]
        public async Task<string> Put([FromBody] SMSServiceDTO[] accounts)
        {
            await this.smsServiceDB.UpdateSMSServiceAsync(mapper.Map<List<DBSMSActivation>>(accounts));
            return JsonConvert.SerializeObject("true");
        }
    }
}
