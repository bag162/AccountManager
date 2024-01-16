using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Email
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "/email,admin")]
    public class EmailController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private readonly ILogger<EmailController> logger;
        private IEmailDBService EmailDBService { get; set; }

        public EmailController(ILogger<EmailController> logger, IMapper mapper, IEmailDBService EmailDBService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.EmailDBService = EmailDBService;
        }

        [HttpGet]
        public string GetList()
        {
            var smsServices = this.EmailDBService.GetEmail().Select(x => x.Id + ":" + x.Name + ":" + x.APIToken).ToList();
            return JsonConvert.SerializeObject(smsServices);
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<EmailDTO> returnedData = this.EmailDBService.GetEmail(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] EmailDTO[] accounts)
        {
            await this.EmailDBService.RemoveEmailAsync(mapper.Map<List<DBEmail>>(accounts.ToList()));
            return JsonConvert.SerializeObject("true");
        }

        [HttpPost]
        public async Task<string> Post([FromBody] EmailDTO[] accounts)
        {
            await this.EmailDBService.AddEmailAsync(mapper.Map<List<DBEmail>>(accounts.ToList()));
            return JsonConvert.SerializeObject("true");
        }

        [HttpPut]
        public async Task<string> Put([FromBody] EmailDTO[] accounts)
        {
            await this.EmailDBService.UpdateEmailAsync(mapper.Map<List<DBEmail>>(accounts));
            return JsonConvert.SerializeObject("true");
        }
    }
}