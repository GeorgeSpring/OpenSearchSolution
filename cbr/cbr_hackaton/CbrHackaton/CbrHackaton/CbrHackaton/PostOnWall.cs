using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace CbrHackaton
{
    public class PostOnWall
    {
        public void Auth(string login, string password)
        {
            var api = new VkApi();

            api.Authorize(new ApiAuthParams()
            {
                ApplicationId = 7111283,
                Login = login,
                Password = password,
                Settings = Settings.All
            });
            api.Wall.Post(new WallPostParams() { Message = "Ё маёо, этот сервис создали боги" });
        }
    }
}
