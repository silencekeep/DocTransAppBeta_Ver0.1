using PaddleOCRSharp;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTransAppBeta1
{
    /// <summary>
    /// Paddle表格分析，也是迟早要替掉
    /// </summary>
    public class StructServiceHost
    {

        //Model Configs
        //private static OCRModelConfig? ocr_config = null;
        private static StructureModelConfig? struct_config = null;

        //Parameters
        //private static OCRParameter ocr_param = new OCRParameter();
        private static StructureParameter struct_param = new StructureParameter();

        static StructServiceHost()
        {
            //ocr_param.cpu_math_library_num_threads = 5;
            //ocr_param.use_gpu = false;
            //ocr_param.cls = true;
            //ocr_param.use_angle_cls = true;

            struct_param.cpu_math_library_num_threads = 10;
            //struct_param.use_gpu = false;
            //struct_param.cls = true;
            //struct_param.use_angle_cls = true;

            //ocr_engine = new PaddleOCREngine(ocr_config, ocr_param);
            //struct_engine = new PaddleStructureEngine(struct_config, struct_param);
        }
        //Engines
        //private static PaddleOCREngine ocr_engine;
        private readonly PaddleStructureEngine struct_engine;
        public StructServiceHost()
        {
            //var stdout = Console.Out;
            //var stderr = Console.Error;
            //Console.SetOut(TextWriter.Null);
            //Console.SetError(TextWriter.Null);
            //var defaultHandle = Utility.DllExportCall.GetStdHandle(Utility.DllExportCall.STD_OUTPUT_HANDLE);
            //2.2 设置输出到匿名管道
            //Utility.DllExportCall.SetStdHandle(Utility.DllExportCall.STD_OUTPUT_HANDLE, new AnonymousPipeServerStream(PipeDirection.Out).SafePipeHandle);
            struct_engine = new PaddleStructureEngine(struct_config, struct_param);
            //Console.SetOut(stdout);
            //Console.SetError(stderr);SetStdHandle(STD_OUTPUT_HANDLE,defaultHandle);
            //Utility.DllExportCall.SetStdHandle(Utility.DllExportCall.STD_OUTPUT_HANDLE, defaultHandle);
            //Console.SetOut(stdout);
            //Console.SetError(stderr);
        }

        public string ImageParseTable(Image img)
        {
            return struct_engine.StructureDetect(img);
        }
    }
}
