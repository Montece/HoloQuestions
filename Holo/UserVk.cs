using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Holo
{
    public static class UserVk
    {
        private static string LOGIN;
        private static string PASSWORD;
        private static ulong APPID;

        private const long GroupID = -200049149;
        private const long AlbumID = 285115458;

        private static readonly Random rand = new Random();
        private static VkApi vkApi;

        public static void Init()
        {
            LOGIN = Environment.GetEnvironmentVariable("VkLogin", EnvironmentVariableTarget.User);
            PASSWORD = Environment.GetEnvironmentVariable("VkPassword", EnvironmentVariableTarget.User);
            APPID = ulong.Parse(Environment.GetEnvironmentVariable("HoloAppID", EnvironmentVariableTarget.User));

            ServiceCollection collection = new ServiceCollection();
            collection.AddAudioBypass();
            vkApi = new VkApi(collection);
            vkApi.OnTokenExpires += VkApi_OnTokenExpires;
        }

        public static void Authorizate()
        {
            vkApi.Authorize(new ApiAuthParams()
            {                
                Login = LOGIN,
                Password = PASSWORD,
                ApplicationId = APPID,
                Settings = Settings.All,
            });   
        }

        private static void VkApi_OnTokenExpires(VkApi sender)
        {
            Authorizate();
        }

        public static void CreatePost(string text)
        {
            var Photos = vkApi.Photo.Get(new PhotoGetParams()
            {
                OwnerId = GroupID,
                AlbumId = PhotoAlbumType.Id(AlbumID)
            });

            var Attach = new List<MediaAttachment>()
            {
                Photos[rand.Next(0, Photos.Count)]
            };

            vkApi.Wall.Post(new WallPostParams()
            {
                OwnerId = GroupID,
                Attachments = Attach,
                Message = text,
                FromGroup = true,
                Signed = false,
                FriendsOnly = false,
                MarkAsAds = false,
                CloseComments = false,
                MuteNotifications = false
            });
        }
    }
}
