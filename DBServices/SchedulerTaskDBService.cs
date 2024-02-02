using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.SchedulerTask.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Humanizer.DateTimeHumanizeStrategy;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BASAccountManager.DBServices
{
    public class SchedulerTaskDBService : ISchedulerTaskDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public SchedulerTaskDBService(
            AMContext dbcontext,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.dbcontext = dbcontext;
        }

        public async Task<bool> AddSchedulerTaskAsync(AddSchedulerTaskDTO data)
        {
            var newSchedulerTask = new DBSchedulerTask()
            {
                Name = data.SchedulerTaskName,
                SchedulerTaskStatus = SchedulerTaskStatus.Completed,
                StartupType = (StartupType)Enum.Parse(typeof(StartupType), data.StartupType),
                CurrentTaskPositionIndex = null,
                LastStart = DateTime.MinValue,
                TaskIds = JsonConvert.SerializeObject(data.TaskIds),
                TimeBetweenLaunchesMinutes = data.TimeBetweenLaunchesMinutes
            };

            await this.dbcontext.SchedulerTask.AddAsync(newSchedulerTask);
            await this.dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task DeleteSchedulerTaskAsync(int id)
        {
            var schedulerTask = await this.dbcontext.SchedulerTask.FindAsync(id);
            this.dbcontext.SchedulerTask.Remove(schedulerTask);
            await this.dbcontext.SaveChangesAsync();
        }

        public JqueryDataTable<SchedulerTaskDTO> GetSchedulerTask(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<SchedulerTaskDTO>();
            data.recordsTotal = this.dbcontext.SchedulerTask.Count();
            List<DBSchedulerTask> filteredData = new List<DBSchedulerTask>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (Enum.TryParse(searchdata, out SchedulerTaskStatus result))
                {
                    filteredData = this.dbcontext.SchedulerTask.AsQueryable().Where(m => m.SchedulerTaskStatus.Equals(result)
                                                                    || m.Name.Contains(searchdata)
                                                                    || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToList();
                }
                else
                {
                    filteredData = this.dbcontext.SchedulerTask.AsQueryable().Where(m => m.Name.Contains(searchdata)
                                                                    || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToList();
                }

                if (lenght != -1)
                {
                    filteredData = filteredData.Take(lenght).ToList();
                }

                data.data = mapper.Map<List<SchedulerTaskDTO>>(filteredData);
                data.recordsFiltered = filteredData.Count();
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                var listData = this.dbcontext.SchedulerTask
                    .AsQueryable()
                    .ToList();

                if (lenght == -1)

                {
                    listData = listData.Skip(start).Take(data.recordsTotal).ToList();
                }
                else
                {
                    listData = listData.Skip(start).Take(lenght).ToList();
                }
                data.data = mapper.Map<List<SchedulerTaskDTO>>(listData.ToArray());
                data.recordsFiltered = listData.Count();
            }
            return data;
        }

        public async Task<AddSchedulerTaskDTO> GetSchedulerTaskByIdAsync(int id)
        {
            var schedulerTask = await this.dbcontext.SchedulerTask.FindAsync(id);

            AddSchedulerTaskDTO returnedTask = new AddSchedulerTaskDTO()
            {
                SchedulerTaskName = schedulerTask.Name,
                StartupType = schedulerTask.StartupType.ToString(),
                TaskIds = JsonConvert.DeserializeObject<int[]>(schedulerTask.TaskIds),
                TimeBetweenLaunchesMinutes = schedulerTask.TimeBetweenLaunchesMinutes,
                Id = schedulerTask.Id
            };

            return returnedTask;
        }

        public List<DBSchedulerTask> GetSchesulerTask()
        {
            return this.dbcontext.SchedulerTask.ToList();
        }

        public async Task StartSchedulerTasksAsync(int[] data)
        {
            var startedTask = new List<DBSchedulerTask>();
            foreach (var item in data)
            {
                var schedulerTask = await this.dbcontext.SchedulerTask.FindAsync(item);
                schedulerTask.SchedulerTaskStatus = SchedulerTaskStatus.Started;
                startedTask.Add(schedulerTask);
            }
            this.dbcontext.SchedulerTask.UpdateRange(startedTask);
            await this.dbcontext.SaveChangesAsync();
        }

        public async Task StopSchedulerTasksAsync(int[] data)
        {
            var stoppedTask = new List<DBSchedulerTask>();
            foreach (var item in data)
            {
                var schedulerTask = await this.dbcontext.SchedulerTask.FindAsync(item);
                schedulerTask.SchedulerTaskStatus = SchedulerTaskStatus.Stopped;
                stoppedTask.Add(schedulerTask);
            }
            this.dbcontext.SchedulerTask.UpdateRange(stoppedTask);
            await this.dbcontext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateSchedulerTaskDTO data)
        {
            var schedulerTask = await this.dbcontext.SchedulerTask.FindAsync(data.Id);
            schedulerTask.TimeBetweenLaunchesMinutes = data.TimeBetweenLaunchesMinutes;
            schedulerTask.TaskIds = JsonConvert.SerializeObject(data.TaskIds);
            this.dbcontext.SchedulerTask.Update(schedulerTask);
            await this.dbcontext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DBSchedulerTask data)
        {
            this.dbcontext.SchedulerTask.Update(data);
            await this.dbcontext.SaveChangesAsync();
        }
    }
}
