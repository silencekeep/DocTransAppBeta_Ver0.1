using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocTransAppBeta1.LLMTranslationMod
{
    /// <summary>
    /// 用于本地使用Ollama接口进行翻译的工具。
    /// 我打算找找本地的翻译工具那种的，实验更快，也能当作“翻译源选择灵活”这一卖点。
    /// </summary>
    public class OllamaApis
    {
        private static string model_name = "qwen2.5:3b-instruct";
        private static readonly string apiUrl = "http://localhost:11434/api/chat";  // 你需要根据实际的 URL 调整
        public static bool TranslateHTMLSync(string input, out string output)
        {
            output = string.Empty;

            using (var client = new HttpClient())
            {
                try
                {
                    // 设置请求头
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                    // 定义请求体，包含系统提示词和用户输入
                    var systemMessage = @"Your task is to translate the text content within an HTML table. The input will be an HTML snippet containing a table with text that needs translation. You should only translate the text inside the table cells and return the translated HTML snippet without altering the structure or any other content.";

                    // 创建 JSON 请求体
                    var data = new
                    {
                        messages = new[]
                        {
                        new
                        {
                            role = "system",
                            content = systemMessage
                        },
                        new
                        {
                            role = "user",
                            content = string.Format("Please segment the following unsegmented English HTML text and return only the translated result in the table cells, without any additional content, even though here are just one word or one letter behind the colon, the HTML table do not need to be translated, just simply output it and translate the following HTML table content into {0}, and return the translated content with the original HTML structure, without altering the structure of the table and without any additional content:\n", "Chinese") + input
                        }
                    },
                        model = model_name,
                        stream = false,
                        temperature = 0.01,
                        max_tokens = 262144
                    };

                    // 将数据序列化为 JSONJsonSerializer.Serialize
                    // 将数据序列化为 JSONJsonSerializer.Serialize

                    var jsonData = JsonConvert.SerializeObject(data);

                    // 创建 HTTP 请求内容
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // 发送 POST 请求
                    var response = client.PostAsync(apiUrl, content).Result;

                    // 检查响应状态并获取返回的内容
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;
                        dynamic result = JsonConvert.DeserializeObject(responseBody);
                        BetaVersionDebugPrinter.WriteLine("表格翻译ok", "OllamaApis");
                        // 提取返回内容中的 'message.content' 字段
                        output = result.message.content;
                        return true;
                    }
                    else
                    {
                        // 请求失败
                        Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return false;
                }
            }
        }
        public static bool TranslateTextSync(string input, out string output)
        {
            output = string.Empty;

            using (var client = new HttpClient())
            {
                try
                {
                    // 设置请求头
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                    // 定义请求体，包含系统提示词和用户输入
                    var systemMessage = @"You are an AI assistant specialized in translating text accurately and efficiently. Your task is to translate the given text into the specified language. Even if the input consists of a single word, provide its translation. The output should contain only the translated text, maintaining the original line breaks without any additional information.";

                    // 创建 JSON 请求体
                    var data = new
                    {
                        messages = new[]
                        {
                        new
                        {
                            role = "system",
                            content = systemMessage
                        },
                        new
                        {
                            role = "user",
                            content = string.Format("Translate the following text into {0}:","Chinese") + input
                        }
                    },
                        model = model_name,
                        stream = false,
                        temperature = 0.01,
                        max_tokens = 262144
                    };

                    // 将数据序列化为 JSONJsonSerializer.Serialize
                    // 将数据序列化为 JSONJsonSerializer.Serialize
                    
                    var jsonData = JsonConvert.SerializeObject(data);

                    // 创建 HTTP 请求内容
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // 发送 POST 请求
                    var response = client.PostAsync(apiUrl, content).Result;

                    // 检查响应状态并获取返回的内容
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;
                        dynamic result = JsonConvert.DeserializeObject(responseBody);
                        BetaVersionDebugPrinter.WriteLine("翻译ok", "OllamaApis");
                        // 提取返回内容中的 'message.content' 字段
                        output = result.message.content;
                        return true;
                    }
                    else
                    {
                        // 请求失败
                        Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return false;
                }
            }
        }
        public static bool DoTextSegmentSync(string input, out string output)
        {
            output = string.Empty;

            using (var client = new HttpClient())
            {
                try
                {
                    // 设置请求头
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                    // 定义请求体，包含系统提示词和用户输入
                    var systemMessage = @"Your task is just to segment the sentences input by user role.\r\n**DONT DO ANYTHING ELSE**";

                    // 创建 JSON 请求体
                    var data = new
                    {
                        messages = new[]
                        {
                        new
                        {
                            role = "system",
                            content = systemMessage
                        },
                        new
                        {
                            role = "user",
                            content = "Please segment the following unsegmented English text and return only the segmented result, without any additional content, even though here are just one word or one letter behind the colon, the word do not need to be segmented, just simply output it:\n" + input
                        }
                    },
                        model = model_name,
                        stream = false,
                        temperature = 0.01,
                        max_tokens = 262144
                    };

                    // 将数据序列化为 JSON
                    var jsonData = JsonConvert.SerializeObject(data);

                    // 创建 HTTP 请求内容
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // 发送 POST 请求
                    var response = client.PostAsync(apiUrl, content).Result;

                    // 检查响应状态并获取返回的内容
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;
                        dynamic result = JsonConvert.DeserializeObject(responseBody);

                        // 提取返回内容中的 'message.content' 字段
                        output = result.message.content;
                        BetaVersionDebugPrinter.WriteLine("字符串降噪ok", "OllamaApis");
                        return true;
                    }
                    else
                    {
                        // 请求失败
                        Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
