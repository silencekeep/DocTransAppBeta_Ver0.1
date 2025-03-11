
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DocTransAppBeta1.LLMTranslationMod
{
    public static class ChatGPT
    {
        private const string ApiKey = "sk-XxR6AbrR7uDySuOKHGDnU0Surt8cFHnnyHiPtnqm7Vcl1p08";  
        private const string ApiUrl = "https://api.chatanywhere.org/v1/chat/completions";
        private static readonly HttpClient _client = new HttpClient();

        static ChatGPT()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        //public static bool TranslateTextSync(string input, out string output)
        //{
        //    output = string.Empty;
        //    return ProcessTextSync(
        //        "You are an AI assistant specialized in translating text accurately and efficiently. Your task is to translate the given text into the specified language. Even if the input consists of a single word, provide its translation. The output should contain only the translated text, maintaining the original line breaks without any additional information.",
        //        $"Translate the following text into Chinese: {input}",
        //        out output
        //    );
        //}

        //public static bool DoTextSegmentSync(string input, out string output)
        //{
        //    output = string.Empty;
        //    return ProcessTextSync(
        //        "Your task is just to segment the sentences input by user role.\r\n**DONT DO ANYTHING ELSE**",
        //        $"Please segment the following unsegmented English text and return only the segmented result, without any additional content, even though here are just one word or one letter behind the colon, the word do not need to be segmented, just simply output it:\n{input}",
        //        out output
        //    );
        //}

        //public static bool TranslateHTMLSync(string input, out string output)
        //{
        //    output = string.Empty;
        //    return ProcessTextSync(
        //        "Your task is to translate the text content within an HTML table. The input will be an HTML snippet containing a table with text that needs translation. You should only translate the text inside the table cells and return the translated HTML snippet without altering the structure or any other content.",
        //        $"Please segment the following unsegmented English HTML text and return only the translated result in the table cells, without any additional content, even though here are just one word or one letter behind the colon, the HTML table do not need to be translated, just simply output it and translate the following HTML table content into {0}, and return the translated content with the original HTML structure, without altering the structure of the table and without any additional content:\n, translate into Chinese:\n{input}",
        //        out output
        //    );
        //}

        public static bool TranslateTextSync(string input, out string output)
        {
            output = string.Empty;
            if (ProcessTextSync(
                "You are an AI assistant specialized in translating text accurately and efficiently. Your task is to translate the given text into the specified language. Even if the input consists of a single word, provide its translation. The output should contain only the translated text, maintaining the original line breaks without any additional information.",
                $"Translate the following text into Chinese: {input}",
                out output
            ))
            {
                BetaVersionDebugPrinter.WriteLine("翻译ok", "ChatGPT");
                return true;
            }
            else
            {
                output = input;  // 失败时返回原文
                return false;
            }
        }

        public static bool DoTextSegmentSync(string input, out string output)
        {
            output = string.Empty;
            if (ProcessTextSync(
                "Your task is just to segment the sentences input by user role.\r\n**DONT DO ANYTHING ELSE**",
                $"Please segment the following unsegmented English text and return only the segmented result, without any additional content, even though here are just one word or one letter behind the colon, the word do not need to be segmented, just simply output it:\n{input}",
                out output
            ))
            {
                BetaVersionDebugPrinter.WriteLine("字符串降噪ok", "ChatGPT");
                return true;
            }
            else
            {
                output = input;  // 失败时返回原文
                return false;
            }
        }

        public static bool TranslateHTMLSync(string input, out string output)
        {
            output = string.Empty;
            if (ProcessTextSync(
                "Your task is to translate the text content within an HTML table. The input will be an HTML snippet containing a table with text that needs translation. You should only translate the text inside the table cells and return the translated HTML snippet without altering the structure or any other content.",
                $"Please segment the following unsegmented English HTML text and return only the translated result in the table cells, without any additional content, even though here are just one word or one letter behind the colon, the HTML table do not need to be translated, just simply output it and translate the following HTML table content into Chinese, and return the translated content with the original HTML structure, without altering the structure of the table and without any additional content:\n{input}",
                out output
            ))
            {
                BetaVersionDebugPrinter.WriteLine("表格翻译ok", "ChatGPT");
                return true;
            }
            else
            {
                output = input;  // 失败时返回原文
                return false;
            }
        }

        private static bool ProcessTextSync(string systemMessage, string userMessage, out string output)
        {
            output = string.Empty;

            try
            {
                BetaVersionDebugPrinter.WriteLine("开始发送请求", "ChatGPT");
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = systemMessage },
                        new { role = "user", content = userMessage }
                    },
                    temperature = 0.01,
                    max_tokens = 2048
                };

                var requestJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(ApiUrl, requestContent).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"请求失败: {response.StatusCode}");
                    return false;
                }

                var content = response.Content.ReadAsStringAsync().Result;
                dynamic result = JsonConvert.DeserializeObject(content);

                if (result?.choices != null && result.choices.Count > 0)
                {
                    output = result.choices[0].message.content.ToString().Trim();
                    return true;
                }

                Console.WriteLine("API未返回结果");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求异常: {ex.Message}");
                return false;
            }
        }
    }
}
