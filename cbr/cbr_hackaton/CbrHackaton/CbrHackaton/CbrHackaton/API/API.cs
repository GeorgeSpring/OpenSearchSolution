using CbrHackaton.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CbrHackaton.API
{
    public class API
    {
        private static string Path = "http://opensolutionsearch.u0811563.plsk.regruhosting.ru/api/v1/";
        private static string LogPath = $"{Path}Auth/login";
        private static string RegPath = $"{Path}Auth/register";
        private static string QuestionPath = $"{Path}question";
        private static string AnswerPath = $"{Path}question/answer";
        private static string StatPath = $"{Path}question/percentage/";
        public static async Task<string> Register(UserModel user)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var str = JsonConvert.SerializeObject(user);
                StringContent sc = new StringContent(str);
                sc.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                var resp = await httpClient.PostAsync(RegPath, sc);
                string respCon = await resp.Content.ReadAsStringAsync();
                var stuff = JObject.Parse(respCon);
                var ss = stuff.GetValue("resultQueryInfo").ToObject<ResultQueryInfo>();
                if (ss.HasError != false)
                {
                    return stuff.GetValue("userName").ToObject<string>();
                }
                throw new CustomException(ss.Messages[0].Text.Value);
            }
            throw new CustomException("Ошибка регистрации!");
        }

        public static async Task<string> Login(UserModel user)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var str = $@"{{ ""userName"": ""{user.Phone}"", ""password"": ""{user.Password}"" }}";
                StringContent sc = new StringContent(str);
                sc.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                var resp = await httpClient.PostAsync(LogPath, sc);
                string respCon = await resp.Content.ReadAsStringAsync();
                var stuff = JObject.Parse(respCon);
                var ss = stuff.GetValue("resultQueryInfo").ToObject<ResultQueryInfo>();
                if (!ss.HasError)
                {
                    return stuff.GetValue("token").ToObject<string>();
                }
                throw new CustomException(ss.Messages[0].Text.Value);
            }
            throw new CustomException("Ошибка авторизации!");
        }
        public static async Task<List<QuestionModel>> GetQuestions(string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var resp = await httpClient.GetAsync(QuestionPath);
                string respCon = await resp.Content.ReadAsStringAsync();
                var stuff = JObject.Parse(respCon);
                var ss = stuff.GetValue("resultQueryInfo").ToObject<ResultQueryInfo>();
                if (!ss.HasError)
                {
                    return stuff.GetValue("items").ToObject<List<QuestionModel>>();
                }
                throw new CustomException(ss.Messages[0].Text.Value);
            }
            throw new CustomException("Ошибка получения данных!");
        }
        public static async Task<bool> SetAnswer(string token, List<int> answersIds)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var str = $@"{{ ""answerIds"": [{string.Join(",", answersIds)}]}}";
                StringContent sc = new StringContent(str);
                sc.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                var resp = await httpClient.PostAsync(AnswerPath, sc);
                string respCon = await resp.Content.ReadAsStringAsync();
                var stuff = JObject.Parse(respCon);
                var ss = stuff.GetValue("resultQueryInfo").ToObject<ResultQueryInfo>();
                if (!ss.HasError)
                {
                    return stuff.GetValue("isSuccess").ToObject<bool>();
                }
                throw new CustomException(ss.Messages[0].Text.Value);
            }
            throw new CustomException("Ошибка получения данных!");
        }

        public static async Task<List<StatisticsModel>> GetStat(int questId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var resp = await httpClient.GetAsync(StatPath + questId);
                string respCon = await resp.Content.ReadAsStringAsync();
                var stuff = JObject.Parse(respCon);
                var ss = stuff.GetValue("resultQueryInfo").ToObject<ResultQueryInfo>();
                if (!ss.HasError)
                {
                    return stuff.GetValue("items").ToObject<List<StatisticsModel>>();
                }
                throw new CustomException(ss.Messages[0].Text.Value);
            }
            throw new CustomException("Ошибка получения данных!");
        }
    }
    public class Message
    {
        public MessageElem Text { get; set; }
        public int Type { get; set; }
    }

    public class MessageElem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class ResultQueryInfo
    {
        public List<Message> Messages { get; set; }
        public bool HasError { get; set; }
    }
    public class CustomException : Exception
    {
        public CustomException(string Text) : base(Text)
        {
        }
    }

}
