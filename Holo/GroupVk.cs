using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model.GroupUpdate;
using VkNet.Enums.SafetyEnums;
using System;
using VkNet.Enums;
using Holo.SIGame;
using Holo.SIGame.Enums;
using VkNet.Model.Attachments;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading;
using Holo.Websites.Website_Shikimori;
using Holo.Themes;
using VkNet.Enums.Filters;
using System.Collections.ObjectModel;
using VkNet.Utils;

namespace Holo
{
    public static class GroupVk
    {
        private readonly static string TOKEN;
        private const ulong ID = 200049149;

        private static readonly VkApi vkApi = new VkApi();
        private static readonly Random random = new Random();
        private static Dictionary<long, VkUser> Users = new Dictionary<long, VkUser>();

        private static bool botActive;

        static GroupVk()
        {
            vkApi.OnTokenExpires += VkApi_OnTokenExpires;
            botActive = false;
            TOKEN = Environment.GetEnvironmentVariable("HoloToken", EnvironmentVariableTarget.User);
        }

        public static void StartBot()
        {
            Authorizate();
            Users = VkUser.LoadAllUsers();
            botActive = true;
            var information = vkApi.Groups.GetLongPollServer(ID);
            BotsLongPollHistoryResponse events = null;
            string ts = information.Ts;

            while (botActive)
            {
                events = GetGroupEvents(information, ts);
                if (events == null)
                {
                    information = vkApi.Groups.GetLongPollServer(ID);
                    ts = information.Ts;
                }
                else
                {
                    foreach (GroupUpdate event_ in events.Updates)
                    {
                        new Thread(() => HandleEvents(event_)).Start();
                    }
                    ts = events.Ts;
                }
            }
        }

        public static void Authorizate()
        {
            vkApi.Authorize(new ApiAuthParams()
            {
                AccessToken = TOKEN
            });
        }

        public static void SendMessage(string text, long id, string filepath = "")
        {
            try
            {
                if (filepath != "")
                {
                    //UploadServerInfo info = vkApi.Docs.GetMessagesUploadServer(id);
                    VkResponse response = vkApi.Call("docs.getMessagesUploadServer", new VkParameters()
                    {
                        { "peer_id", id },
                        { "type", DocMessageType.Doc }
                    });

                    using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                        fs.Close();
                    }

                    List<MediaAttachment> attach = new List<MediaAttachment>();
                    using (var uploader = new WebClient())
                    {
                        var uploadResponseInBytes = uploader.UploadFile(response["upload_url"].ToString(), filepath);
                        UploadInfo uploadResponseInString = JsonConvert.DeserializeObject<UploadInfo>(Encoding.UTF8.GetString(uploadResponseInBytes));
                        var attachs = vkApi.Docs.Save(uploadResponseInString.File, Path.GetFileName(filepath), null);
                        attach.Add(attachs[0].Instance);
                    }

                    vkApi.Messages.Send(new MessagesSendParams
                    {
                        Message = text,
                        UserId = id,
                        RandomId = random.Next(),
                        Attachments = attach
                    });
                }
                else
                {
                    vkApi.Messages.Send(new MessagesSendParams
                    {
                        Message = text,
                        UserId = id,
                        RandomId = random.Next()
                    });
                }
            }
            catch (Exception x)
            {
                Output.Error("Ошибка отправки!", x);
            }
        }

        private static void VkApi_OnTokenExpires(VkApi sender)
        {
            Authorizate();
        }

        public static BotsLongPollHistoryResponse GetGroupEvents(LongPollServerResponse response, string ts)
        {
            try
            {
                var events = vkApi.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams()
                {
                    Key = response.Key,
                    Server = response.Server,
                    Ts = ts,
                    Wait = 25
                });
                return events;
            }
            catch
            {
                return null;
            }
        }

        private static void HandleEvents(GroupUpdate event_)
        {
            /*if (event_.Instance != null)
            {
                if (event_.Instance is  )
            }*/

            if (event_.Type == GroupUpdateType.MessageNew && event_.MessageNew.Message.Type == MessageType.Received)
            {
                if (event_.MessageNew.Message.PeerId == event_.MessageNew.Message.FromId)
                {
                    string msg = event_.MessageNew.Message.Text;
                    long id = event_.MessageNew.Message.FromId.Value;

                    VkUser user = null;
                    if (!Users.ContainsKey(id))
                    {
                        User user_vk = vkApi.Users.Get(new long[] { id }, ProfileFields.FirstName | ProfileFields.LastName).FirstOrDefault();
                        user = new VkUser(user_vk.FirstName, user_vk.LastName, id);
                        Users.Add(id, user);
                    }
                    else
                    {
                        user = Users[id];
                    }

                    Output.Print($"{user.Name} {user.Surname[0]}.: {msg}");

                    switch (user.Status)
                    {
                        case VkUserStatus.EnteringShikimori:
                            msg.Replace("http", "htpps");
                            bool result = Uri.TryCreate(msg, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttps);
                            if (result)
                            {
                                string user_id = msg.Replace("https://shikimori.one/", "");

                                if (!Regex.IsMatch(user_id, @"\p{IsCyrillic}"))
                                {
                                    if (Main_Shikimori.UserExists(user_id))
                                    {
                                        if (!user.shikimori.HasUser(user_id))
                                        {
                                            //Проверка, <Volkovich> - что список аниме открыт и есть хотя бы 1 аниме просмотренная
                                            user.shikimori.AddUser(user_id);
                                            SendMessage($"Я успешно записала пользователя с ником {user_id} в твой список. Хочешь добавить еще кого-то? Если нет, скажи - 'хватит'!", id);
                                        }
                                        else
                                        {
                                            SendMessage($"Эй! Ты уже добавил такого пользователя. Хочешь добавить еще кого-то? Если нет, скажи - 'хватит'!", id);
                                        }
                                    }
                                    else
                                    {
                                        SendMessage($"Извини, но такого пользователя не существует. Пропробуй еще раз. Если не хочешь, то так и скажи - 'хватит'!", id);
                                    }
                                }
                                else
                                {
                                    SendMessage($"Извини, но я не умею записывать пользователей с русскими символами в ссылке. Пропробуй другого пользователя, например, меня! Если не хочешь, то так и скажи - 'хватит'!", id);
                                }
                            }
                            else
                            {
                                if (msg.ToLower().Equals("не хочу"))
                                {
                                    SendMessage($"Хорошо, вредина. Если захочешь - пиши. На этом всё. (.", id);
                                    user.Clear();
                                }
                                else
                                if (msg.ToLower().Equals("хватит"))
                                {
                                    if (user.shikimori.GetUsersCount() > 0)
                                    {
                                        user.Status = VkUserStatus.EnteringThemes;
                                        string n = Environment.NewLine;
                                        SendMessage($"Теперь давай разберемся, какие темы ты хочешь увидеть в паке. Введи темы одним сообщением в таком формате: '1 5 4 7 9 6' {n}0 - Все темы{n}1 - {Theme_AnimeByJapane.GetRawTitle()}{n}2 - {Theme_CharacterByImage.GetRawTitle()}{n}3 - {Theme_AnimeByScreenshot.GetRawTitle()}{n}4 - {Theme_AnimeByCoub.GetRawTitle()}{n}5 - {Theme_YearOfAnime.GetRawTitle()}{n}6 - {Theme_AnimeByDisappearance.GetRawTitle()}{n}7 - {Theme_CharacterByDescription.GetRawTitle()}" +
                                            $"{n}8 - {Theme_AnimeByDescription.GetRawTitle()}{n}9 - {Theme_AnimeByAnagramm.GetRawTitle()}{n}10 - {Theme_CharacterByJapanese.GetRawTitle()}{n}11 - {Theme_AnimeByPoster.GetRawTitle()}{n}12 - {Theme_AnimeByOpening.GetRawTitle()}{n}13 - {Theme_AnimeByEnding.GetRawTitle()}", id);
                                    }
                                    else
                                    {
                                        SendMessage($"Прежде чем я начну писать пак, добавь хотя бы одного пользователя, а то чьи аниме я буду добавлять?", id);
                                    }
                                }
                            }
                            break;
                        case VkUserStatus.EnteringThemes:
                            string[] chars = msg.Split(' ');
                            user.package.Themes.Clear();
                            for (int i = 0; i < chars.Length; i++)
                            {
                                switch (chars[i])
                                {
                                    case "0":
                                        user.package.Themes.AddRange(new Theme[]
                                        {
                                            new Theme_AnimeByJapane(),
                                            new Theme_CharacterByImage(),
                                            new Theme_AnimeByScreenshot(),
                                            new Theme_AnimeByCoub(),
                                            new Theme_CharacterByDescription(),
                                            new Theme_AnimeByDescription(),
                                            new Theme_AnimeByAnagramm(),
                                            new Theme_CharacterByJapanese(),
                                            new Theme_AnimeByPoster(),
                                            new Theme_AnimeByDisappearance(),
                                            new Theme_YearOfAnime(),
                                            new Theme_AnimeByOpening(),
                                            new Theme_AnimeByEnding(),
                                        });
                                        break;
                                    case "1":
                                        user.package.Themes.Add(new Theme_AnimeByJapane());
                                        user.AddTheme(1);
                                        break;
                                    case "2":
                                        user.package.Themes.Add(new Theme_CharacterByImage());
                                        user.AddTheme(2);
                                        break;
                                    case "3":
                                        user.package.Themes.Add(new Theme_AnimeByScreenshot());
                                        user.AddTheme(3);
                                        break;
                                    case "4":
                                        user.package.Themes.Add(new Theme_AnimeByCoub());
                                        user.AddTheme(4);
                                        break;
                                    case "5":
                                        user.package.Themes.Add(new Theme_YearOfAnime());
                                        user.AddTheme(5);
                                        break;
                                    case "6":
                                        user.package.Themes.Add(new Theme_AnimeByDisappearance());
                                        user.AddTheme(6);
                                        break;
                                    case "7":
                                        user.package.Themes.Add(new Theme_CharacterByDescription());
                                        user.AddTheme(7);
                                        break;
                                    case "8":
                                        user.package.Themes.Add(new Theme_AnimeByDescription());
                                        user.AddTheme(8);
                                        break;
                                    case "9":
                                        user.package.Themes.Add(new Theme_AnimeByAnagramm());
                                        user.AddTheme(9);
                                        break;
                                    case "10":
                                        user.package.Themes.Add(new Theme_CharacterByJapanese());
                                        user.AddTheme(10);
                                        break;
                                    case "11":
                                        user.package.Themes.Add(new Theme_AnimeByPoster());
                                        user.AddTheme(11);
                                        break; 
                                    case "12":
                                        user.package.Themes.Add(new Theme_AnimeByOpening());
                                        user.AddTheme(12);
                                        break;
                                    case "13":
                                        user.package.Themes.Add(new Theme_AnimeByEnding());
                                        user.AddTheme(13);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            try
                            {
                                user.Status = VkUserStatus.Creating;
                                user.package.ID = Config.CurrentConfig.PacksCount;
                                Config.CurrentConfig.IncPacksCount();

                                string packTypeText = "";

                                switch (user.packageType)
                                {
                                    case PackType.Small:
                                        packTypeText = "маленький пак";
                                        break;
                                    case PackType.Medium:
                                        packTypeText = "средний пак";
                                        break;
                                    case PackType.Big:
                                        packTypeText = "большой пак";
                                        break;
                                    default:
                                        packTypeText = "<неизвестный размер>";
                                        break;
                                }
                                SendMessage($"Я села писать новый {packTypeText}. Закончу минут через... ну короче, как сделаю, так пришлю письмо. А пока что иди поешь! Хоть могучий аппетит — это и минус, но я не вижу причин воздерживаться!)", id);
                                Output.Print($"Скачиваю шикимори #{user.package.ID}");
                                user.shikimori.FillLists();
                                Output.Print($"Заполняю вопросы #{user.package.ID}");
                                string package_path = user.package.Fill(user.shikimori);
                                Output.Print($"Закончила создавать #{user.package.ID}");
                                user.AddPack(user.package.ID);
                                SendMessage($"Эй, ты не уснул там? А я дописала! Правда, Холо - молодец? Ладно, держи, и не забудь по-бла-го-да-рить!", id, package_path);
                                switch (user.packageType)
                                {
                                    case PackType.Small:
                                        user.IncSmallPackagesCount();
                                        break;
                                    case PackType.Medium:
                                        user.IncMediumPackagesCount();
                                        break;
                                    case PackType.Big:
                                        user.IncBigPackagesCount();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (Exception x)
                            {
                                Output.Print(x.ToString());
                                SendMessage($"Упс, что-то пошло не так. Я походу где-то ошиблась. Давай попробуем всё заново?", id);
                            }
                            finally
                            {
                                user.Clear();
                            }
                            break;
                        case VkUserStatus.Creating:
                            SendMessage($"Я делаю пак, не отвлекай!", id);
                            break;
                        case VkUserStatus.Nothing:
                            if (msg.ToLower().Equals("сделать маленький пак"))
                            {
                                SendMessage($"Хорошо, давай займемся написанием маленького пака. Скажи ссылки на профили Shikimori, с которых мне брать анимешки. Но запомни: 1 сообщение - одна ссылка. Например: https://shikimori.one/HoloQuestions. Если же ты передумал, то скажи 'не хочу'.", id);
                                user.packageType = PackType.Small;
                                user.Status = VkUserStatus.EnteringShikimori;
                                user.package = new SIGamePack(user.packageType);
                            }
                            else
                            if (msg.ToLower().Equals("сделать средний пак"))
                            {
                                SendMessage($"Хорошо, давай займемся написанием среднего пака. Скажи ссылки на профили Shikimori, с которых мне брать анимешки. Но запомни: 1 сообщение - одна ссылка. Например: https://shikimori.one/HoloQuestions. Если же ты передумал, то скажи 'не хочу'.", id);
                                user.packageType = PackType.Medium;
                                user.Status = VkUserStatus.EnteringShikimori;
                                user.package = new SIGamePack(user.packageType);
                            }
                            else
                            if (msg.ToLower().Equals("сделать большой пак"))
                            {
                                SendMessage($"Хорошо, давай займемся написанием большого пака. Скажи ссылки на профили Shikimori, с которых мне брать анимешки. Но запомни: 1 сообщение - одна ссылка. Например: https://shikimori.one/HoloQuestions. Если же ты передумал, то скажи 'не хочу'.", id);
                                user.packageType = PackType.Big;
                                user.Status = VkUserStatus.EnteringShikimori;
                                user.package = new SIGamePack(user.packageType);
                            }
                            else
                            if (Regex.IsMatch(msg.ToLower(), "спасибо*"))
                            {
                                SendMessage($"Не за что!) Обращайся!", id);
                            }
                            else
                            if (Regex.IsMatch(msg.ToLower(), "помоги*"))
                            {
                                SendMessage($"Ладно, я тебе расскажу, что здесь да как. Скажи мне 'сделать маленький пак' (раундов: {Config.CurrentConfig.SmallRoundsCount}), 'сделать средний пак' (раундов: {Config.CurrentConfig.MediumRoundsCount}), 'сделать большой пак' (раундов: {Config.CurrentConfig.BigRoundsCount}). Всё просто!", id);
                            }
                            else
                            {
                                SendMessage($"Ой-ой, вечно ты говоришь что-то непонятное. Если не знаешь, как правильно задать вопрос, тогда скажи: 'помоги', и я с радостью тебя верну на истинный путь)", id);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }   
        }

        public static string UploadFileToServer(string url, byte[] fileData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.SendChunked = true;
            request.TransferEncoding = "utf8";

            string encoded = Convert.ToBase64String(fileData);
            string postData = "file1=" + encoded;

            request.ContentType = "multipart/form-data";
            request.ContentLength = Encoding.UTF8.GetByteCount(postData);

            using (var newStream = request.GetRequestStream())
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(postData);
                newStream.Write(postBytes, 0, postBytes.Length);
                newStream.Close();
            }
            var a = request.GetResponse();
            return a.ToString();
        }
    }

    [Serializable]
    public class UploadInfo
    {
        [JsonProperty("file")]
        public string File { get; set; }
    }
}