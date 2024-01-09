using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Newtonsoft.Json;

namespace BASAccountManager.BackgroundTask
{
    public class AssignmentWriter
    {
        private ILogger<AssignmentWriter> logger;
        private ISMSServiceDB SMSServiceDB;
        private IProxyDBService proxyDBService;
        private ITaskDBService taskDbService { get; set; }
        private IWorkerTaskDBService workerTaskDbService { get; set; }
        private IEmailDBService emailDBService { get; set; }
        private IInstDBService instDBService { get; set; }
        private IPostDBService postDBService { get; set; }
        private IPostGroupDBService postGroupDBService { get; set; }
        private IInstPostDBService instPostDBService { get; set; }
        private IPostCommentGroupDBService postCommentGroupDBService { get; set; }
        private IPostCommentDBService postCommentDBService { get; set; }

        public AssignmentWriter(ITaskDBService taskDbService,
            IWorkerTaskDBService workerTaskDbService,
            IProxyDBService proxyDBService,
            ISMSServiceDB SMSServiceDB,
            ILogger<AssignmentWriter> logger,
            IEmailDBService emailDBService,
            IInstDBService instDBService,
            IPostDBService postDBService,
            IPostGroupDBService postGroupDBService,
            IInstPostDBService instPostDBService,
            IPostCommentGroupDBService postCommentGroupDBService,
            IPostCommentDBService postCommentDBService)
        {
            this.taskDbService = taskDbService;
            this.workerTaskDbService = workerTaskDbService;
            this.proxyDBService = proxyDBService;
            this.SMSServiceDB = SMSServiceDB;
            this.logger = logger;
            this.emailDBService = emailDBService;
            this.instDBService = instDBService;
            this.postDBService = postDBService;
            this.postGroupDBService = postGroupDBService;
            this.instPostDBService = instPostDBService;
            this.postCommentGroupDBService = postCommentGroupDBService;
            this.postCommentDBService = postCommentDBService;
        }

        // Парсит посты на наличие новых комментариев и добаляет их в базу задач
        public async Task CommentParserAsync()
        {
            // Получаем все активные посты
            var instPosts = await this.instPostDBService.GetAllInstPostAsync();
            // Оставляем только выложенные посты
            instPosts = instPosts.Where(x => x.InstPostStatus == InstPostStatus.Published).Where(x => x.Post.PostStatus == PostStatus.Active).ToList();
            
            foreach (var instPost in instPosts)
            {
                // Пропускаем пост если все необходимые комментарии уже есть в базе
                if (instPost.ListComment.Count() >= instPost.Post.RequiredCountComments)
                    continue;
                // Узнаем сколько комментариев нам нужно добавить
                var requiredComments = instPost.Post.RequiredCountComments - instPost.ListComment.Count();
                // Получаем образцы комментариев которые мы будем добавлять для поста
                var sampleComments = this.postCommentGroupDBService.GetCommentGroupById(instPost.Post.PostCommentGroupId).ListComment;

                foreach (var currentComment in instPost.ListComment)
                {
                    if (sampleComments.Where(x => x.Id == currentComment.CommentId).Count() != 0)
                    {
                        // Удаляем из листа комментариев, комментарии по образцам которых уже был добавлен комментарий
                        sampleComments.Remove(sampleComments.Where(x => x.Id == currentComment.CommentId).First());
                    }
                }
                // Создаем список комментариев, по образцам которых будем создавать комменты
                var sampleCommentsToAdd = sampleComments.Take(requiredComments).ToList();
                var commentsToAdd = new List<DBPostComment>();

                // Создаем комментарии на добавление
                foreach (var sampleComment in sampleCommentsToAdd)
                {
                    var newComment = new DBPostComment();
                    newComment.PostId = instPost.Id;
                    newComment.CommentId = sampleComment.Id;
                    newComment.CommentStatus = CommentStatus.NotPublished;
                    commentsToAdd.Add(newComment);
                }
                if (commentsToAdd.Count() != 0)
                await this.postCommentDBService.AddCommentAsync(commentsToAdd);
            }
        }

        // Парсит Post и добавляет InstPost
        public async Task PostParserAsync()
        {
            var allPostGroups = this.postGroupDBService.GetGroups();
            foreach (var postGroup in allPostGroups)
            {
                var parsedPosts = postGroup.ListPost.Where(x => x.PostStatus == PostStatus.Active).ToList();
                var accounts = await this.instDBService.GetInstAccountsByGroupAsync(postGroup.AccountGroup.Name);
                foreach (var account in accounts)
                {
                    foreach (var checkedPost in parsedPosts)
                    {
                        if (account.ListPost.Where(x => x.PostId == checkedPost.Id).Count() == 0)
                        {
                            var newInstPost = new DBInstPost() { AccountId = account.Id, PostId = checkedPost.Id, InstPostStatus = InstPostStatus.NotPublished };
                            await this.instPostDBService.AddInstPostAsync(newInstPost);
                        }
                    }
                }
            }
        }

        // Парсит Task и добавляет WorkerTask
        public async Task TaskParserAsync()
        {
            var addedTask = this.taskDbService.GetTask().Where(x => x.Status == StatusTask.Added || x.Status == StatusTask.AddingProcess);
            foreach (var newTask in addedTask)
            {
                await this.GenWorkerTaskAsync(newTask);
            }
        }

        private async Task GenWorkerTaskAsync(DBTask task)
        {
            switch (task.TaskType)
            {
                case TaskType.RegistrationAccounts:
                    await ParseRegistrationTask(task);
                    break;
                case TaskType.AuthorizationAccounts:
                    await ParseAuthorizationTask(task);
                    break;
                case TaskType.Posting:
                    await ParsePostingTask(task);
                    break;
                default:
                    break;
            }
        }

        private async Task<DBProxy> GetFreeProxyByGroupAsync(string group)
        {
            var proxy = this.proxyDBService.GetProxyByGroup(group);
            if (proxy.Where(x => x.ProxyStatus == ProxyStatus.Free).Count() == 0)
            {
                return null;
            }
            else
            {
                var returnedProxy = proxy.Where(x => x.ProxyStatus == ProxyStatus.Free).First();
                returnedProxy.ProxyStatus = ProxyStatus.BookedForWork;
                await this.proxyDBService.UpdateProxyAsync(returnedProxy);
                return returnedProxy;
            }
        }

        private async Task ParseRegistrationTask(DBTask task)
        {
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);

            var addedTask = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);
            List<DBWorkerTask> workerTaskList = new();
            RegistrationTaskWorkerUsefulDataDTO usefulData = JsonConvert.DeserializeObject<RegistrationTaskWorkerUsefulDataDTO>(task.UsefulData);
            int countAddTask;
            if (addedTask.Count == 0)
            {
                countAddTask = usefulData.CountAccount;
            }
            else
            {
                // Убираем задачи которые завершились с ошибкой, т.к. в них не было зарегестрировано аккаунтов
                countAddTask = usefulData.CountAccount - addedTask.Where(x => x.Status != DB.Models.TaskStatus.Error).Count();
            }
            for (int i = 0; i < countAddTask; i++)
            {
                var proxy = await GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }

                var workerTask = new DBWorkerTask()
                {
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.RegistrationAccounts,
                    TaskId = task.Id,
                    ProxyId = proxy.Id
                };

                switch (usefulData.RegistrationVerifyResoursesType)
                {
                    case RegistrationVerifyResoursesType.EmailService:
                        var emailService = this.emailDBService.GetEmailById((int)usefulData.EmailServiceId);
                        workerTask.UsefulData = JsonConvert.SerializeObject(new RegistrationClientTaskWorkerUsefulDataEmailServiceDTO() { EmailServiceData = emailService });
                        workerTask.RegistrationVerifyResoursesType = RegistrationVerifyResoursesType.EmailService;
                        break;
                    case RegistrationVerifyResoursesType.SMSService:
                        var smsService = this.SMSServiceDB.GetSMSServiceById((int)usefulData.SMSServiceId);
                        workerTask.UsefulData = JsonConvert.SerializeObject(new RegistrationClientTaskWorkerUsefulDataSMSServiceDTO() { SMSServiceData = smsService });
                        workerTask.RegistrationVerifyResoursesType = RegistrationVerifyResoursesType.SMSService;
                        break;
                }

                workerTaskList.Add(workerTask);

                if (i == countAddTask - 1)
                {
                    task.Status = StatusTask.Performed;
                    await this.taskDbService.UpdateTaskAsync(task);
                }
            }

            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
        }

        private async Task ParseAuthorizationTask(DBTask task)
        {
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);
            List<DBWorkerTask> workerTaskList = new();

            // Получаем список всех добавленных задач на авторизацию
            var addedTask = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

            // Получае список всех аккаунтов в группе на авторизацию
            var addedList = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);

            // Создаем список на удаление задач из списка на добавление. Если у задачи Status != Error, то удаляем эти аккаунты
            var deleteOutAddedList = addedTask.Where(x => x.Status != DB.Models.TaskStatus.Error).Select(x => x.Account).ToList();
            // Удаляем из списка на добавление аккаунтов все аккаунты, у задач которых статус != Error
            var listToAdd = new List<DBInstagramAccount>(); listToAdd.AddRange(addedList);
            if (deleteOutAddedList != null && deleteOutAddedList.Count != 0 && addedList != null && addedList.Count != 0)
            {
                foreach (var accToAdd in addedList)
                {
                    if (deleteOutAddedList.Where(x => x.Id == accToAdd.Id).Count() != 0)
                    {
                        listToAdd.Remove(accToAdd);
                    }
                }
            }
            // Оставляем только неавторизованные аккаунты
            listToAdd = listToAdd.Where(x => x.AccountStatus == AccountStatus.NotAuthorized).ToList();

            // Создаем список, который будет сигнализивать что мы обратали все аккаунты, что бы установить статус Performed
            var checkFullAddList = new List<DBInstagramAccount>();
            checkFullAddList.AddRange(listToAdd); // TODO Сделать как в PostingParser, убрать лишний код

            foreach (var newAccount in listToAdd)
            {
                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }
                var newTask = new DBWorkerTask()
                {
                    TaskId = task.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.AuthorizationAccounts,
                    AccountId = newAccount.Id,
                    ProxyId = proxy.Id
                };
                checkFullAddList.Remove(newAccount);
                workerTaskList.Add(newTask);
            }

            // Если все аккаунты добавлены, то обновляем статус головной задачи
            if (checkFullAddList.Count == 0)
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }
            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
            return;
        }

        private async Task ParsePostingTask(DBTask task)
        {
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);
            List<DBWorkerTask> workerTaskList = new();
            // Получаем список активных воркеров
            var addedWorkers = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);
            // Получаем список аккаунтов, для которых будем осуществлять постинг
            var accountList = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);
            // Используем только авторизованные аккаунты
            accountList = accountList.Where(x => x.AccountStatus == AccountStatus.Authorized).ToList();
            // Используем аккаунты, у которых есть посты для публикации
            accountList = accountList.Where(x => x.ListPost.Where(x => x.InstPostStatus == InstPostStatus.NotPublished).Count() != 0).ToList();
            // Удаляем из списка аккаунтов на добавление, аккаунты, которые уже ранее были добавлены в WorkerList
            foreach (var account in addedWorkers.Where(x => x.Status != DB.Models.TaskStatus.Error).Select(x => x.Account).ToList())
            {
                if (accountList.Contains(account))
                    accountList.Remove(account);
            }
            // Проходимся по списку аккаунтов и добавляем посты
            foreach (var account in accountList)
            {
                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }

                var newTask = new DBWorkerTask()
                {
                    TaskId = task.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.Posting,
                    AccountId = account.Id,
                    ProxyId = proxy.Id,
                    UsefulData = task.UsefulData
                };

                workerTaskList.Add(newTask);
            }

            // Если все аккаунты были добавлены в workerList, то устанавливаем статус Performed
            if (workerTaskList.Count() == accountList.Count())
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }
            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
            return;
        }
    }
}