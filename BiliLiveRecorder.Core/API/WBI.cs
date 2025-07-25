﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BiliLiveRecorder.Core.API
{
    internal class WBI
    {
        private static HttpClient _httpClient = new();

        private static readonly int[] MixinKeyEncTab =
        {
        46, 47, 18, 2, 53, 8, 23, 32, 15, 50, 10, 31, 58, 3, 45, 35, 27, 43, 5, 49, 33, 9, 42, 19, 29, 28, 14, 39,
        12, 38, 41, 13, 37, 48, 7, 16, 24, 55, 40, 61, 26, 17, 0, 1, 60, 51, 30, 4, 22, 25, 54, 21, 56, 59, 6, 63,
        57, 62, 11, 36, 20, 34, 44, 52
    };

        //对 imgKey 和 subKey 进行字符顺序打乱编码
        private static string GetMixinKey(string orig)
        {
            return MixinKeyEncTab.Aggregate("", (s, i) => s + orig[i])[..32];
        }

        private static Dictionary<string, string> EncWbi(Dictionary<string, string> parameters, string imgKey,
            string subKey)
        {
            string mixinKey = GetMixinKey(imgKey + subKey);
            string currTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            //添加 wts 字段
            parameters["wts"] = currTime;
            // 按照 key 重排参数
            parameters = parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            //过滤 value 中的 "!'()*" 字符
            parameters = parameters.ToDictionary(
                kvp => kvp.Key,
                kvp => new string(kvp.Value.Where(chr => !"!'()*".Contains(chr)).ToArray())
            );
            // 序列化参数
            string query = new FormUrlEncodedContent(parameters).ReadAsStringAsync().Result;
            //计算 w_rid
            using MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query + mixinKey));
            string wbiSign = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            parameters["w_rid"] = wbiSign;

            return parameters;
        }

        // 获取最新的 img_key 和 sub_key
        private static async Task<(string, string)> GetWbiKeys()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            httpClient.DefaultRequestHeaders.Referrer = new Uri("https://www.bilibili.com/");

            HttpResponseMessage responseMessage = await httpClient.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.bilibili.com/x/web-interface/nav"),
            });

            JsonNode response = JsonNode.Parse(await responseMessage.Content.ReadAsStringAsync())!;

            string imgUrl = (string)response["data"]!["wbi_img"]!["img_url"]!;
            imgUrl = imgUrl.Split("/")[^1].Split(".")[0];

            string subUrl = (string)response["data"]!["wbi_img"]!["sub_url"]!;
            subUrl = subUrl.Split("/")[^1].Split(".")[0];
            return (imgUrl, subUrl);
        }


        public static async Task<string> Main(string ID)
        {
            var (imgKey, subKey) = await GetWbiKeys();

            Dictionary<string, string> signedParams = EncWbi(
                parameters: new Dictionary<string, string>
                {
                { "id", ID }
                },
                imgKey: imgKey,
                subKey: subKey
            );

            string query = await new FormUrlEncodedContent(signedParams).ReadAsStringAsync();

            return query;
        }
        public static async Task<string> GetWBIByID(string ID)
        {
            var (imgKey, subKey) = await GetWbiKeys();

            Dictionary<string, string> signedParams = EncWbi(
                parameters: new Dictionary<string, string>
                {
                { "room_id", ID }
                },
                imgKey: imgKey,
                subKey: subKey
            );

            string query = await new FormUrlEncodedContent(signedParams).ReadAsStringAsync();

            return query;
        }
    }
}
