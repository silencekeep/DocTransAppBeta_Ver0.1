using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DocTransAppBeta1
{
    /// <summary>
    /// 用于调整配置的配置文件，我想办法把PaddleOCR迁移到GPU中，这就更不需要这个配置了
    /// </summary>
    public class DocTransConfiguration
    {
        //单例模式支持
        private static DocTransConfiguration? __config = null;
        //public string doclayout_model_path { get; set; }
        //public int doclayout_model_input_dimension { get; set; }
        //还没有实现GPU推理
        //public bool doclayout_use_gpu { get; set; }
        

        public static DocTransConfiguration GetGlobalConfig()
        {
            if (__config == null) throw new Exception("还未主动加载配置文件。");
            return __config;
        }
        public static bool UpdateCurrentConfig(DocTransConfiguration config)
        {
            if (__config == null) return false;
            __config = config;
            return true;
        }
        public DocTransConfiguration()
        {

        }
        public static void LoadConfig()
        {
            if (!File.Exists("./Config.json"))
            {
                __config = new DocTransConfiguration();
                SaveConfig();
            }
            else
            {
                var jsonString = File.ReadAllText("./Config.json");
                __config = JsonSerializer.Deserialize<DocTransConfiguration>(jsonString);
            }
                
        }
        public static void SaveConfig()
        {
            if (__config == null) return;
            var jsonString = JsonSerializer.Serialize(__config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("./Config.json", jsonString);
        }
    }
}
