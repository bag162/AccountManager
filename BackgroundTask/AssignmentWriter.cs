using AutoMapper;
using BASAccountManager.Abstraction;
using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Runtime.InteropServices;

namespace BASAccountManager.BackgroundTask
{
    public class AssignmentWriter
    {
        private IMapper mapper;
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
        private IPostLikeDBService postLikeDBService { get; set; }
        private IFollowDBService followDBService { get; set; }
        private IFillingDataDBService fillingDataDBService { get; set; }
        private IAdvertPostDBService advertPostDBService { get; set; }
        private IAdvertAccountDBService advertAccountDBService { get; set; }

        public AssignmentWriter(ITaskDBService taskDbService,
            IWorkerTaskDBService workerTaskDbService,
            IProxyDBService proxyDBService,
            ISMSServiceDB SMSServiceDB,
            IEmailDBService emailDBService,
            IInstDBService instDBService,
            IPostDBService postDBService,
            IPostGroupDBService postGroupDBService,
            IInstPostDBService instPostDBService,
            IPostCommentGroupDBService postCommentGroupDBService,
            IPostCommentDBService postCommentDBService,
            IMapper mapper,
            IPostLikeDBService postLikeDBService,
            IFollowDBService followDBService,
            IFillingDataDBService fillingDataDBService,
            IAdvertPostDBService advertPostDBService,
            IAdvertAccountDBService advertAccountDBService)
        {
            this.taskDbService = taskDbService;
            this.workerTaskDbService = workerTaskDbService;
            this.proxyDBService = proxyDBService;
            this.SMSServiceDB = SMSServiceDB;
            this.emailDBService = emailDBService;
            this.instDBService = instDBService;
            this.postDBService = postDBService;
            this.postGroupDBService = postGroupDBService;
            this.instPostDBService = instPostDBService;
            this.postCommentGroupDBService = postCommentGroupDBService;
            this.postCommentDBService = postCommentDBService;
            this.mapper = mapper;
            this.postLikeDBService = postLikeDBService;
            this.followDBService = followDBService;
            this.fillingDataDBService = fillingDataDBService;
            this.advertAccountDBService = advertAccountDBService;
            this.advertPostDBService = advertPostDBService;
        }

        // Парсит посты на наличие новых комментариев и добаляет их в базу задач
        public async Task CommentParserAsync()
        {
            // Получаем все активные посты
            var instPosts = await this.instPostDBService.GetAllInstPostAsyncAsNoTracking();
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
                var sampleComments = instPost.Post.PostCommentGroup.ListComment;

                foreach (var currentComment in instPost.ListComment)
                {
                    if (sampleComments.Where(x => x.Id == currentComment.CommentId).Count() != 0)
                    {
                        // Удаляем из листа комментариев, комментарии по образцам которых уже был добавлен комментарий
                        sampleComments.Remove(sampleComments.Where(x => x.Id == currentComment.CommentId).First());
                    }
                }
                // Если нет комментариев которые можем добавить, то пропускаем пост
                if (sampleComments.Count() == 0)
                {
                    continue;
                }
                // Перемешивание строк
                sampleComments = sampleComments.OrderBy(x => Guid.NewGuid().ToString()).ToList();
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

        public async Task LikesParserAsync()
        {
            // Получаем все активные посты
            var instPosts = await this.instPostDBService.GetAllInstPostAsyncAsNoTracking();
            // Оставляем только выложенные посты
            instPosts = instPosts.Where(x => x.InstPostStatus == InstPostStatus.Published).Where(x => x.Post.PostStatus == PostStatus.Active).ToList();

            foreach (var instPost in instPosts)
            {
                if (instPost.ListLikes.Count() >= instPost.Post.RequiredCountLikes)
                    continue;

                var requiredLikes = instPost.Post.RequiredCountLikes - instPost.ListLikes.Count();
                var likesToAdd = new List<DBPostLikes>();
                for (int i = 0; i < requiredLikes; i++)
                {
                    var templateLike = new DBPostLikes()
                    {
                        PostId = instPost.Id,
                        LikeStatus = LikeStatus.NotPublished
                    };
                    likesToAdd.Add(templateLike);
                }
                await this.postLikeDBService.AddLikesAsync(likesToAdd);
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
            var addedTask = this.taskDbService.GetTask().Where(x => x.Status == StatusTask.Added || x.Status == StatusTask.AddingProcess).ToList();
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
                case TaskType.Commenting:
                    await ParseCommentTask(task);
                    break;
                case TaskType.Liking:
                    await ParseLikingTask(task);
                    break;
                case TaskType.Following:
                    await ParseFollowingTask(task);
                    break;
                case TaskType.FillingProfile:
                    await ParseFillingProfileTask(task);
                    break;
                case TaskType.AdvertCommenting:
                    await ParseAdvertCommentingTask(task);
                    break;
                case TaskType.AdvertFollowing:
                    await ParseAdvertFollowingTask(task);
                    break;
                case TaskType.AdvertLiking:
                    await ParseAdvertLikingTask(task);
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
            var listToAdd = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);
            listToAdd = listToAdd.Where(x => x.AccountStatus == AccountStatus.NotAuthorized).ToList();

            // Удаляем из списка аккаунтов на добавление, аккаунты, которые уже ранее были добавлены в WorkerList
            foreach (var account in addedTask.Select(x => x.Account).ToList())
            {
                // Не авторизуем аккаунты если он содержится в workerList, а если и содержится, то эта задача завершилась неудачно
                if (listToAdd.Where(x => x.Id == account.Id).Count() != 0)
                    listToAdd.Remove(listToAdd.Where(x => x.Id == account.Id).First());
            }

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
                workerTaskList.Add(newTask);
            }

            // Если все аккаунты добавлены, то обновляем статус головной задачи
            if (workerTaskList.Count() == listToAdd.Count())
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
            accountList = accountList.Where(x => x.AccountStatus == AccountStatus.Authorized)
                // Используем аккаунты, у которых есть посты для публикации
                .Where(x => x.ListPost
                .Where(x => x.InstPostStatus == InstPostStatus.NotPublished).Count() != 0 || x.ListPost.Where(x => x.InstPostStatus == InstPostStatus.PostingError).Count() != 0)

                .OrderBy(x => Guid.NewGuid().ToString())
                .ToList();
            
            // Удаляем из списка аккаунтов на добавление, аккаунты, которые уже ранее были добавлены в WorkerList
            foreach (var account in addedWorkers.Select(x => x.Account).ToList())
            {
                if (accountList.Where(x => x.Id == account.Id).Count() != 0)
                    accountList.Remove(accountList.Where(x => x.Id == account.Id).First());
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

        private async Task ParseCommentTask(DBTask task)
        {
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);
            List<DBWorkerTask> workerTaskList = new();

            // Получае список всех добавленных задач на комментинг
            var addedTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

            // Получаем все аккаунты по группе и оставляем только авторизованные
            var accounts = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);
            accounts = accounts.Where(x => x.AccountStatus == AccountStatus.Authorized).OrderBy(x => Guid.NewGuid().ToString()).ToList();

            // Удаляем из списка аккаунты, для которых уже были добавлены комментарии
            foreach (var addedTask in addedTasks)
            {
                if (accounts.Where(x => x.Id == addedTask.AccountId).Count() != 0)
                {
                    accounts.Remove(accounts.Where(x => x.Id == addedTask.AccountId).First());
                }
            }
            var usefulData = JsonConvert.DeserializeObject<CommentingTaskWorkerUsefulDataDTO>(task.UsefulData);

            var posts = new List<DBInstPost>();
            // Получаем все посты по группам
            if (usefulData.PostGroup == "All groups")
            {
                posts = await this.instPostDBService.GetAllInstPostAsync();
            }
            else
            {
                posts = await this.instPostDBService.GetAllPostByGroupAsync(usefulData.PostGroup);
            }
            // Получаем все комментарии по выбраннной группе
            var allComments = new List<DBPostComment>();
            foreach (var post in posts)
            {
                allComments.AddRange(post.ListComment);
            }

            // Оставляем только неопубликованные комментарии к которым не привязан аккаунт
            var allFilteredComments = allComments
                .Where(x => x.CommentStatus == CommentStatus.NotPublished)
                .Where(x => x.SenderAccountId == null)
                .OrderBy(x => Guid.NewGuid().ToString())
                .ToList();

            bool noProxy = false;
            foreach (var accountToTask in accounts)
            {
                var commentsToTask = new List<DBPostComment>();

                // Если аккаунт который будет публиковать комментарий не является владельцем поста, то добавляем его в задачу
                foreach (var newComment in allFilteredComments)
                {
                    if (commentsToTask.Count() == usefulData.CommentsPerAccount)
                    {
                        break;
                    } 

                    // Проверка, что аккаунт будет публиковать комментарий не на свой пст
                    if (newComment.Post.AccountId != accountToTask.Id)
                    {
                        // Проверка, что аккаунт не публиковало ранее на этот пост комментарий
                        if (!allComments.Where(x => x.SenderAccountId == accountToTask.Id).Select(x => x.PostId).Contains(newComment.PostId))
                        {
                            // Пверока, что в листе на добавление комментариев нет комментов на этот пост, что бы 1 аккаунт не публиковал 2 комментария на 1 пост
                            if (commentsToTask.Where(x => x.PostId == newComment.PostId).Count() == 0)
                            {
                                commentsToTask.Add(newComment);
                            }
                            
                        }
                    }
                }
                if (commentsToTask.Count == 0)
                {
                    continue;
                }
                // удаляем комментарии из листа на добавление
                foreach (var newComment in commentsToTask)
                {
                    allFilteredComments.Remove(newComment);
                }
                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    noProxy = true;
                    break;
                }
                // Прикрепляем к комментарию аккаунт
                foreach (var updatedComment in commentsToTask)
                {
                    updatedComment.SenderAccountId = accountToTask.Id;
                }
                await this.postCommentDBService.UpdatePostCommentAsync(commentsToTask);

                var commentsList = this.mapper.Map<List<CommentUsefulDataDTO>>(commentsToTask);
                var newTask = new DBWorkerTask()
                {
                    AccountId = accountToTask.Id,
                    ProxyId = proxy.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.Commenting,
                    TaskId = task.Id,
                    UsefulData = JsonConvert.SerializeObject(commentsList)
                };
                
                workerTaskList.Add(newTask);
            }
            if (noProxy == false)
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }
            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
        }

        private async Task ParseLikingTask(DBTask task)
        {
            // Обновляем статус задачи
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);
            List<DBWorkerTask> workerTaskList = new();

            // Получаем уже добавленные задачи
            var addedTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

            // Получаем авторизованные аккаунты по группе
            var accounts = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);
            accounts = accounts
                .Where(x => x.AccountStatus == AccountStatus.Authorized)
                .OrderBy(x => Guid.NewGuid().ToString())
                .ToList();
            
            var usefulData = JsonConvert.DeserializeObject<LikingTaskWorkerUsefulDatadTO>(task.UsefulData);

            // Получаем посты которые будем лайкать
            var postsForLiking = new List<DBInstPost>();
            if (usefulData.PostGroup == "All groups")
            {
                postsForLiking = await this.instPostDBService.GetAllInstPostAsync();
            }
            else
            {
                postsForLiking = await this.instPostDBService.GetAllPostByGroupAsync(usefulData.PostGroup);
            }

            // Вытягиваем из постов все сущности Like
            var allLikes = new List<DBPostLikes>();
            foreach (var instPost in postsForLiking)
            {
                allLikes.AddRange(instPost.ListLikes.Where(x => x.LikeStatus == LikeStatus.NotPublished).Where(x => x.SenderAccountId == null).ToList());
            }
            allLikes = allLikes.OrderBy(x => Guid.NewGuid().ToString()).ToList();
            // Удаляем аккаунты для которых уже созданы задачи
            foreach (var addedTask in addedTasks)
            {
                if (accounts.Where(x => x.Id == addedTask.AccountId).Count() != 0)
                {
                    accounts.Remove(accounts.Where(x => x.Id == addedTask.AccountId).First());
                }
            }

            foreach (var accToAdd in accounts)
            {
                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }
                // Все добавленные лайки
                var allCurrentLikes = this.postLikeDBService.GetAllLikesAsNoTracking();

                // Получаем лайки для добавления в задачу
                var likesWorkerList = new List<DBPostLikes>();
                foreach (var like in allLikes)
                {
                    // Проверяем, что поста уже нет в листе задач
                    if (likesWorkerList.Where(x => x.PostId == like.PostId).Count() == 0)
                    {
                        // Проверяем, что поста нет в листе задач с этим аккаунтом на глобальном уровен
                        if (allCurrentLikes.Where(x => x.PostId == like.PostId).Where(x => x.SenderAccountId == accToAdd.Id).Count() == 0)
                        {
                            likesWorkerList.Add(like);
                            if (likesWorkerList.Count() >= usefulData.LikesPerAccount)
                            {
                                break;
                            }
                        }
                    }
                }
                if (likesWorkerList.Count() == 0)
                {
                    continue;
                }
                foreach (var likeToRem in likesWorkerList)
                {
                    allLikes.Remove(likeToRem);
                }

                // Закрепляем за лайком аккаунт
                foreach (var likeToUpdate in likesWorkerList)
                {
                    likeToUpdate.SenderAccountId = accToAdd.Id;
                }
                await this.postLikeDBService.UpdateLikesAsync(likesWorkerList);

                var likesList = this.mapper.Map<List<LikeUsefulDataDTO>>(likesWorkerList);
                var newTask = new DBWorkerTask()
                {
                    AccountId = accToAdd.Id,
                    ProxyId = proxy.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.Liking,
                    TaskId = task.Id,
                    UsefulData = JsonConvert.SerializeObject(likesList)
                };

                workerTaskList.Add(newTask);
            }

            if (workerTaskList.Count() == accounts.Count())
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }
            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
        }

        private async Task ParseFollowingTask(DBTask task)
        {
            List<DBWorkerTask> workerTaskList = new();

            // Парсим UsefulData
            var usefulData = JsonConvert.DeserializeObject<FollowingTaskWorkerUsefulDataDTO>(task.UsefulData);

            // Обновляем статус задачи
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);

            // Получаем уже добавленные задачи
            var addedTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

            // Получаем авторизованные аккаунты по группе
            var accounts = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);
            accounts = accounts
                .Where(x => x.AccountStatus == AccountStatus.Authorized)
                .OrderBy(x => Guid.NewGuid().ToString())
                .ToList();
            // Удаляем из списка аккаунтов на добавление, аккаунты, которые уже ранее были добавлены в WorkerList
            foreach (var account in addedTasks.Select(x => x.Account).ToList())
            {
                // Не подписываемся на  аккаунты если он содержится в workerList, а если и содержится, то эта задача завершилась неудачно
                if (accounts.Where(x => x.Id == account.Id).Count() !=0)
                    accounts.Remove(accounts.Where(x => x.Id == account.Id).First());
            }

            // Получаем аккаунты на которые будем подписываться
            var accountsToSubscription = new List<DBInstagramAccount>();
            if (usefulData.AccountGroupForSubscription == "All groups")
            {
                accountsToSubscription = this.instDBService.GetInstAccounts().Where(x => x.AccountStatus == AccountStatus.Authorized).ToList();
            }
            else
            {
                accountsToSubscription = await this.instDBService.GetInstAccountsByGroupAsync(usefulData.AccountGroupForSubscription);
                accountsToSubscription = accountsToSubscription.Where(x => x.AccountStatus == AccountStatus.Authorized).ToList();
            }
            accountsToSubscription = accountsToSubscription.OrderBy(x => Guid.NewGuid().ToString()).ToList();
            var listFollows = new List<DBFollow>();
            // Создаем подписки на аккаунты
            foreach (var acc in accountsToSubscription)
            {
                // Получаем существующие подписки на аккаунт
                var accFollows = this.followDBService.GetFollowsByRecipientAccountId(acc.Id)
                    .Where(x => x.FollowStatus != FollowStatus.ErrorFollow)
                    .OrderBy(x => Guid.NewGuid().ToString())
                    .ToList();

                // Если достаточное кол-во подписок уже есть, то пропускаем
                if (accFollows.Count() >= usefulData.RequiredFollowersPerAccount)
                    continue;

                // Создаем цикл который будет создавать необходимо кол-во подписок
                for (int i = 0; i < usefulData.RequiredFollowersPerAccount - accFollows.Count(); i++)
                {
                    foreach (var account in accounts)
                    {
                        if (accFollows
                            // Проверяем делал ли аккаунт подписку ранее
                            .Where(x => x.SenderAccountId == account.Id)
                            .Where(x => x.FollowStatus != FollowStatus.ErrorFollow)
                            .Count() == 0 
                            // Проверяем что это не 2-а одинаковых аккаунта
                            && acc.Id != account.Id
                            // Проверяем что эта подписка не создана
                            && listFollows
                            .Where(x => x.SenderAccountId == account.Id)
                            .Where(x => x.RecipientAccountId == acc.Id)
                            .Count() == 0)
                        {
                            listFollows.Add(new DBFollow()
                            {
                                FollowStatus = FollowStatus.NotPublished,
                                SenderAccountId = account.Id,
                                RecipientAccountId = acc.Id
                            });
                            break;
                        }
                    }
                }
            }

            // Получае Id аккаунтов для которых будем создавать воркеров
            var accountIds = listFollows.Select(x => x.SenderAccountId).ToList();

            // Создаем воркеров
            foreach (var accountId in accountIds)
            {
                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }

                // Получаем follows для аккаунта
                var followList = listFollows.Where(x => x.SenderAccountId == accountId).Take(usefulData.FollowsPerAccount).ToList();
                // Добавляем follows в базу
                await this.followDBService.AddFollowsAsync(followList);

                // Получаем их Id для передачи в TaskWorker
                var listToWorker = this.followDBService.GetFollowsBySenderAccountId(accountId)
                    .Where(x => x.FollowStatus == FollowStatus.NotPublished)
                    .Select(x => x.Id)
                    .ToList();

                var newTask = new DBWorkerTask()
                {
                    AccountId = accountId,
                    ProxyId = proxy.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.Following,
                    TaskId = task.Id,
                    UsefulData = JsonConvert.SerializeObject(listToWorker)
                };

                workerTaskList.Add(newTask);
            }

            if (workerTaskList.Count() == accountIds.Count())
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }
            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);

        }

        private async Task ParseFillingProfileTask(DBTask task)
        {
            List<DBWorkerTask> workerTaskList = new();

            // Парсим UsefulData
            var usefulData = JsonConvert.DeserializeObject<FillingProfileTaskWorkerUsefulDataDTO>(task.UsefulData);

            // Обновляем статус задачи
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);

            // Получаем уже добавленные задачи
            var addedTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

            // Получаем данные для заполнения профиля
            var profileFilling = this.fillingDataDBService.GetFillingDataByName(usefulData.FillingProfileName);

            // Получаем аккаунты, которые будем заполнять
            var accounts = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);
            accounts = accounts.Where(x => x.FillingDataId != profileFilling.Id).ToList();

            // Удаляем из списка аккаунтов на добавление, аккаунты, которые уже ранее были добавлены в WorkerList
            foreach (var taskAccount in addedTasks.Select(x => x.Account).ToList())
            {
                // Не заполняем аккаунты если он содержится в workerList
                if (accounts.Where(x => x.Id == taskAccount.Id).Count() != 0)
                    accounts.Remove(accounts.Where(x => x.Id == taskAccount.Id).First());
                // Не заполняем аккаунты если у них статус не равнен Authorized
                if (taskAccount.AccountStatus != AccountStatus.Authorized)
                    accounts.Remove(accounts.Where(x => x.Id == taskAccount.Id).First());
            }

            foreach (var accountToAdd in accounts)
            {
                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }
                ProfileFillingUsefulDataDTO serializedUsefulData = mapper.Map<ProfileFillingUsefulDataDTO>(profileFilling);

                var newTask = new DBWorkerTask()
                {
                    AccountId = accountToAdd.Id,
                    ProxyId = proxy.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.FillingProfile,
                    TaskId = task.Id,
                    UsefulData = JsonConvert.SerializeObject(serializedUsefulData)
                };

                workerTaskList.Add(newTask);
            }

            if (workerTaskList.Count() == accounts.Count())
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }
            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
        }

        private async Task ParseAdvertLikingTask(DBTask task)
        {
            List<DBWorkerTask> workerTaskList = new();

            // Парсим UsefulData
            var usefulData = JsonConvert.DeserializeObject<AdvertLikingTaskWorkerUsefulDatadTO>(task.UsefulData);

            // Обновляем статус задачи
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);

            // Получаем уже добавленные задачи
            var addedTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

            // Получаем аккаунты
            var accounts = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);

            // Удаляем из списка аккаунтов на добавление, аккаунты, которые уже ранее были добавлены в WorkerList
            foreach (var taskAccount in addedTasks.Select(x => x.Account).ToList())
            {
                // Не используем аккаунты если он содержится в workerList
                if (accounts.Where(x => x.Id == taskAccount.Id).Count() != 0)
                    accounts.Remove(accounts.Where(x => x.Id == taskAccount.Id).First());
                // Не используем аккаунты если у них статус не равнен Authorized
                if (taskAccount.AccountStatus != AccountStatus.Authorized)
                    accounts.Remove(accounts.Where(x => x.Id == taskAccount.Id).First());
            }

            // Получаем посты, на которые ранее не ставились лайки
            var postsToWork = await this.advertPostDBService.GetAdvertPostByGroupAsync(usefulData.AdvertPostGroup);
            postsToWork = postsToWork.Where(x => x.AdvertPostLikeStatus == DB.Models.AdvertResourses.AdvertPostActionStatus.NotProcessed).ToList();

            // Если во входных данных есть условие что нельзя лайкать посты, которые были прокомментированы, то фильтруем данные
            if (usefulData.LikeIfPostCommentedPreviously == false)
            {
                postsToWork = postsToWork.Where(x => x.AdvertPostCommentStatus == DB.Models.AdvertResourses.AdvertPostActionStatus.NotProcessed).ToList();
            }

            foreach (var account in accounts)
            {
                var postToLikes = postsToWork.Take(usefulData.LikesPerAccount).ToList();
                if (postToLikes.Count() == 0)
                {
                    break;
                }
                foreach (var item in postToLikes)
                {
                    postsToWork.Remove(item);
                    item.AdvertPostLikeStatus = DB.Models.AdvertResourses.AdvertPostActionStatus.ProcessTreatment;

                }

                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }

                await this.advertPostDBService.UpdateAdvertPostAsync(postToLikes);

                var newTask = new DBWorkerTask()
                {
                    AccountId = account.Id,
                    ProxyId = proxy.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.AdvertLiking,
                    TaskId = task.Id,
                    UsefulData = JsonConvert.SerializeObject(postToLikes)
                };
                workerTaskList.Add(newTask);
            }

            if (workerTaskList.Count() == accounts.Count() || postsToWork.Count() == 0)
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }

            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
        }

        private async Task ParseAdvertCommentingTask(DBTask task)
        {
            List<DBWorkerTask> workerTaskList = new();

            // Парсим UsefulData
            var usefulData = JsonConvert.DeserializeObject<AdvertCommentingTaskWorkerUsefulDatadTO>(task.UsefulData);

            // Обновляем статус задачи
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);

            // Получаем уже добавленные задачи
            var addedTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

            // Получаем аккаунты
            var accounts = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);

            // Удаляем из списка аккаунтов на добавление, аккаунты, которые уже ранее были добавлены в WorkerList
            foreach (var taskAccount in addedTasks.Select(x => x.Account).ToList())
            {
                // Не используем аккаунты если он содержится в workerList
                if (accounts.Where(x => x.Id == taskAccount.Id).Count() != 0)
                    accounts.Remove(accounts.Where(x => x.Id == taskAccount.Id).First());
                // Не используем аккаунты если у них статус не равнен Authorized
                if (taskAccount.AccountStatus != AccountStatus.Authorized)
                    accounts.Remove(accounts.Where(x => x.Id == taskAccount.Id).First());
            }

            // Получаем посты, которые ранее не комментировались
            var postsToWork = await this.advertPostDBService.GetAdvertPostByGroupAsync(usefulData.AdvertPostGroup);
            postsToWork = postsToWork.Where(x => x.AdvertPostCommentStatus == DB.Models.AdvertResourses.AdvertPostActionStatus.NotProcessed).ToList();

            // Если во входных данных есть условие что нельзя комментировать посты, которые были пролайканы, то фильтруем данные
            if (usefulData.CommentIfPostLikedPreviously == false)
            {
                postsToWork = postsToWork.Where(x => x.AdvertPostLikeStatus == DB.Models.AdvertResourses.AdvertPostActionStatus.NotProcessed).ToList();
            }

            foreach (var account in accounts)
            {
                var postToCommenting = postsToWork.Take(usefulData.CommentsPerAccount).ToList();
                if (postToCommenting.Count() == 0)
                {
                    break;
                }
                foreach (var item in postToCommenting)
                {
                    postsToWork.Remove(item);
                    item.AdvertPostCommentStatus = DB.Models.AdvertResourses.AdvertPostActionStatus.ProcessTreatment;

                }

                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }

                await this.advertPostDBService.UpdateAdvertPostAsync(postToCommenting);

                var newTask = new DBWorkerTask()
                {
                    AccountId = account.Id,
                    ProxyId = proxy.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.AdvertCommenting,
                    TaskId = task.Id,
                    UsefulData = JsonConvert.SerializeObject(postToCommenting)
                };
                workerTaskList.Add(newTask);
            }

            if (workerTaskList.Count() == accounts.Count() || postsToWork.Count() == 0)
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }

            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
        }

        private async Task ParseAdvertFollowingTask(DBTask task)
        {
            List<DBWorkerTask> workerTaskList = new();

            // Парсим UsefulData
            var usefulData = JsonConvert.DeserializeObject<AdvertFollowingTaskWorkerUsefulDatadTO>(task.UsefulData);

            // Обновляем статус задачи
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);

            // Получаем уже добавленные задачи
            var addedTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

            // Получаем аккаунты
            var accounts = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);

            // Удаляем из списка аккаунтов на добавление, аккаунты, которые уже ранее были добавлены в WorkerList
            foreach (var taskAccount in addedTasks.Select(x => x.Account).ToList())
            {
                // Не используем аккаунты если он содержится в workerList
                if (accounts.Where(x => x.Id == taskAccount.Id).Count() != 0)
                    accounts.Remove(accounts.Where(x => x.Id == taskAccount.Id).First());
                // Не используем аккаунты если у них статус не равнен Authorized
                if (taskAccount.AccountStatus != AccountStatus.Authorized)
                    accounts.Remove(accounts.Where(x => x.Id == taskAccount.Id).First());
            }

            // Получаем аккаунты, на которые ранее не подписывались
            var accountsToWork = await this.advertAccountDBService.GetAdvertAccountsByGroupAsync(usefulData.AdvertAccountGroup);
            accountsToWork = accountsToWork.Where(x => x.AdvertAccountStatus == DB.Models.AdvertResourses.AdvertAccountStatus.NotProcessed).ToList();

            foreach (var account in accounts)
            {
                var accountsToFollow = accountsToWork.Take(usefulData.FollowsPerAccount).ToList();
                if (accountsToFollow.Count() == 0)
                {
                    break;
                }
                foreach (var item in accountsToFollow)
                {
                    accountsToWork.Remove(item);
                    item.AdvertAccountStatus = DB.Models.AdvertResourses.AdvertAccountStatus.ProcessTreatment;

                }

                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }

                await this.advertAccountDBService.UpdateAvertAccountsAsync(accountsToFollow);

                var newTask = new DBWorkerTask()
                {
                    AccountId = account.Id,
                    ProxyId = proxy.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.AdvertFollowing,
                    TaskId = task.Id,
                    UsefulData = JsonConvert.SerializeObject(accountsToFollow)
                };
                workerTaskList.Add(newTask);
            }

            if (workerTaskList.Count() == accounts.Count() || accountsToWork.Count() == 0)
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }

            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
        }
    }
}