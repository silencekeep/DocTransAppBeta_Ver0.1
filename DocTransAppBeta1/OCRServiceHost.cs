using PaddleOCRSharp;
using System.IO.Pipes;
using System.Text;

namespace DocTransAppBeta1
{
    /// <summary>
    /// OCR推理器，我迟早要使用PaddleSharp重构
    /// </summary>
    public class OCRServiceHost
    {
        //Model Configs
        private static OCRModelConfig? ocr_config = null;
        //private static StructureModelConfig? struct_config = null;

        //Parameters
        private static OCRParameter ocr_param = new OCRParameter();
        //private static StructureParameter struct_param = new StructureParameter();

        static OCRServiceHost()
        {
            //PaddleOCRSharp.EngineBase.
            ocr_param.cpu_math_library_num_threads = 10;
            ocr_param.use_gpu = false;
            //ocr_param.cls = true;
            //ocr_param.use_angle_cls = true;

            //struct_param.cpu_math_library_num_threads = 5;
            //struct_param.use_gpu = false;
            //struct_param.cls = true;
            //struct_param.use_angle_cls = true;
            //struct_engine = new PaddleStructureEngine(struct_config, struct_param);
        }
        //Engines
        private readonly PaddleOCREngine ocr_engine;
        //private static PaddleStructureEngine? struct_engine;
        public OCRServiceHost()
        {
            //var stdout = Console.Out;
            //var stderr = Console.Error;
            //Console.SetOut(TextWriter.Null);
            //Console.SetError(TextWriter.Null);
            //2.1 获取输出句柄
            //var defaultHandle = Utility.DllExportCall.GetStdHandle(Utility.DllExportCall.STD_OUTPUT_HANDLE);
            //2.2 设置输出到匿名管道
            //Utility.DllExportCall.SetStdHandle(Utility.DllExportCall.STD_OUTPUT_HANDLE, new AnonymousPipeServerStream(PipeDirection.Out).SafePipeHandle);
            ocr_engine = new PaddleOCREngine(ocr_config, ocr_param);
            //Console.SetOut(stdout);
            //Console.SetError(stderr);SetStdHandle(STD_OUTPUT_HANDLE,defaultHandle);
            //Utility.DllExportCall.SetStdHandle(Utility.DllExportCall.STD_OUTPUT_HANDLE, defaultHandle);
        }
        public string ImageRecognizeRaw(Image img)
        {
            return ocr_engine.DetectText(img).Text;//Please segment the following unsegmented English text and return only the segmented result, without any additional content:contextonataskswitchtobedelayeduntilanx87FPU/MMX/SSE/SSE2/SSE3/SSSE3/SSE4instructionis
        }
        private static int __Min_Y_in_ListofOCRPoint(List<OCRPoint> list)
        {
            int ret = int.MaxValue;
            if (list is null) return ret;
            foreach (OCRPoint ocrPoint in list)
            {
                if (ocrPoint.Y < ret) ret = ocrPoint.Y;
            }
            return ret;
        }
        public IEnumerable<string> ImageRecognizeWithCRLF(Image img, int align_arg = 8)
        {

            int temp = 0;
            StringBuilder sb = new StringBuilder();
            var list = ocr_engine.DetectText(img).TextBlocks;//Please segment the following unsegmented English text and return only the segmented result, without any additional content:contextonataskswitchtobedelayeduntilanx87FPU/MMX/SSE/SSE2/SSE3/SSSE3/SSE4instructionis
            //哑对象
            //list.Append(new TextBlock() { BoxPoints = new List<OCRPoint>() { new OCRPoint(int.MaxValue, int.MaxValue) } });
            foreach (var block in list)
            {
                int st = __Min_Y_in_ListofOCRPoint(block.BoxPoints);
                if (string.IsNullOrEmpty(block.Text) || st == int.MaxValue)
                {
                    continue;
                }
                else if (st - temp >= align_arg)
                {
                    temp = st;
                    if (sb.Length > 0) yield return sb.ToString().Trim();
                    sb.Clear();
                    sb.Append(block.Text + " ");
                }
                else sb.Append(block.Text + " ");
            }
            if (sb.Length > 0) yield return sb.ToString().Trim();
        }
    }
}
