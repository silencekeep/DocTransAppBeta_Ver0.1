using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using LangChain.Databases.Sqlite;
using LangChain.Providers;
using LangChain.Providers.OpenAI;
using LangChain.Providers.Ollama;
using LangChain.Splitters.Text;
using LangChain.Databases;
using System.Numerics;

namespace DocTransAppBeta1.LLMTranslationMod
{
    public static class DeepSeekLangChain
    {
        private const string ApiKey = "sk-03b7d3c94bcc432ea14c45b2d501c58f";
        private const string ApiUrl = "https://api.deepseek.com/v1/chat/completions";
        private static readonly HttpClient _client = new HttpClient();

        private static readonly OpenAiProvider Provider = new OpenAiProvider(
            apiKey: ApiKey,
            customEndpoint: "https://api.deepseek.com/v1"
        );

        private static readonly OpenAiChatModel LLM = new OpenAiChatModel(Provider, "deepseek-chat");
        private static readonly OllamaEmbeddingModel EmbeddingModel = new OllamaEmbeddingModel(new OllamaProvider(), id: "all-minilm");
        private static readonly SqLiteVectorDatabase VectorDatabase = new SqLiteVectorDatabase(dataSource: "vectors.db");
        private static readonly IVectorCollection _translationCollection;
        static DeepSeekLangChain()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            // 单次初始化集合
            _translationCollection = VectorDatabase
                .GetOrCreateCollectionAsync("translations", dimensions: 768)
                .GetAwaiter()
                .GetResult();
        }

        public static bool TranslateTextSync(string input, out string output)
        {
            output = string.Empty;
            if (ProcessTextSync(
                "You are an AI assistant specialized in translating text accurately and efficiently.",
                $"Translate the following text into Chinese: {input}",
                out output
            ))
            {
                BetaVersionDebugPrinter.WriteLine("翻译ok", "DeepSeek");

                // 存入向量数据库
                SaveTranslationToVectorDB(input, output);

                return true;
            }
            else
            {
                output = input;
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
                BetaVersionDebugPrinter.WriteLine("字符串降噪ok", "DeepSeek");
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
                BetaVersionDebugPrinter.WriteLine("表格翻译ok", "DeepSeek");
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
                var requestBody = new
                {
                    model = "deepseek-chat",
                    messages = new[]
                    {
                        new { role = "system", content = systemMessage },
                        new { role = "user", content = userMessage }
                    },
                    temperature = 0.01,
                    max_tokens = 2000
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

        private static async Task SaveTranslationToVectorDB(string originalText, string translatedText)
        {
            try
            {
                // 生成嵌入向量（保持原有逻辑）
                var request = new EmbeddingRequest { Strings = new List<string> { originalText } };
                var embeddingResponse = await EmbeddingModel.CreateEmbeddingsAsync(request);
                var embedding = embeddingResponse.Values.FirstOrDefault();

                if (embedding == null) return;

                // 创建并存储向量
                await _translationCollection.AddAsync(new List<Vector>
                {
                    new Vector
                    {
                        Id = Guid.NewGuid().ToString(),
                        Embedding = embedding,
                        Text = originalText,
                        Metadata = new Dictionary<string, object>
                        {
                            { "translatedText", translatedText },
                            { "created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"存储失败（简化处理）: {ex.Message}");
                //// 简单删除数据库并重建（仅用于测试环境）
                //if (ex.Message.Contains("already exists"))
                //{
                //    //File.Delete("vectors.db");
                //    await VectorDatabase.GetOrCreateCollectionAsync("translations", 768);
                //}
            }
        }

    }
}