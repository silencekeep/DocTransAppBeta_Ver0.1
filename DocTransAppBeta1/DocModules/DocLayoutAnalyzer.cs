using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * DocLayoutAnalyzer.cs
 * DocLayoutYOLO ONNX推理器
 * 依赖第三方包：ONNXRUNTIME
 */
namespace DocTransAppBeta1.DocModules
{
    /// <summary>
    /// 用于使用DocLayoutYOLO进行文章版面分析的对象
    /// </summary>
    public class DocLayoutAnalyzer : IDisposable
    {
        /// <summary>
        /// 用于编号转标签枚举的静态方法
        /// </summary>
        /// <param name="number">编号</param>
        /// <returns></returns>
        public static DocLayoutLabel NumberToLabel(int number)
        {
            return number > 9 && number < 0 ? DocLayoutLabel.unknown : (DocLayoutLabel)number;
        }
        /// <summary>
        /// 用于标签名称转枚举的静态方法
        /// </summary>
        /// <param name="name">标签名称</param>
        /// <returns></returns>
        public static DocLayoutLabel NumberToLabel(string name)
        {
            return Enum.TryParse(name, out DocLayoutLabel label) ? label : DocLayoutLabel.unknown;
        }

        /// <summary>
        /// 适应于ONNX模型输出的内部结构
        /// </summary>
        public struct DocLayoutInferenceResult
        {
            public DocLayoutInferenceResult(int cls, float confidence, float[] bbox) : this()
            {
                this.label = NumberToLabel(cls);
                this.confidence = confidence;
                this.bbox = bbox;
            }

            public DocLayoutLabel label { get; set; }
            public string? label_name
            {
                get
                {
                    try { return label.ToString(); }
                    catch { return null; }
                }
            }
            public float confidence { get; set; }
            public float[] bbox { get; set; }
        }

        /// <summary>
        /// 推理会话对象，用的是输入1280*1280的模型，效果目前最好，也不用提供什么选项来换了。
        /// </summary>
        private InferenceSession? session;
        private SessionOptions? sessionOptions;
        private const int INPUT_DIM = 1280;// DocTransConfiguration.GetGlobalConfig().doclayout_model_input_dimension;

        // = "C:\\Users\\56279\\Desktop\\MultiModalTranslator\\YOLO\\doclayout_yolo_docstructbench_imgsz1280_2501.onnx"
        /// <summary>
        /// 版面解析推理器的构造函数。
        /// </summary>
        public DocLayoutAnalyzer()
        {
            //CUDA硬性要求，不用GPU版本了
            //固定的相对路径，就这样就行，csproj中有复制命令，是从项目中复制到OutDir的。
            //var options = null;//SessionOptions.Make//SessionOptions.MakeSessionOptionWithCudaProvider();
            sessionOptions = new SessionOptions();
            //sessionOptions.AppendExecutionProvider_CUDA(0);
            //SessionOptions.MakeSessionOptionWithCudaProvider();
            session = new InferenceSession("./inference/doclayout_yolo/doclayout_yolo_docstructbench_imgsz1280_2501.onnx", sessionOptions);//, options);
        }
        /// <summary>
        /// 将System.Drawing.Image转换为模型输入张量。
        /// </summary>
        /// <param name="src">输入的图片Image</param>
        /// <param name="ratio">引用返回解析的比例系数</param>
        /// <param name="offset">引用返回填充后的偏移量，正为竖版偏移，负为横板偏移</param>
        /// <returns>可供输入DocLayout-YOLOv10l的ONNX稠密张量输入</returns>
        public static DenseTensor<float> ConvertImageToTensor(Image src, out float ratio, out int offset)
        {
            // Resize the image to the required input size
            int max_w4h = Math.Max(src.Width, src.Height);
            int min_w4h = Math.Min(src.Width, src.Height);
            ratio = 1.0f * max_w4h / INPUT_DIM;
            float inner_ratio = 1.0f * INPUT_DIM / max_w4h;
            Size newSize = new Size((int)(inner_ratio * src.Width), (int)(inner_ratio * src.Height));
            Bitmap resizedImage = new Bitmap(newSize.Width, newSize.Height);

            // 使用 Graphics 对象进行图像调整大小
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                // 设置高质量的图像插值模式
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // 调整大小并绘制图像
                g.DrawImage(src, 0, 0, newSize.Width, newSize.Height);
            }
            //Cv2.Resize(src, resizedMat, );
            Bitmap paddedImage = new Bitmap(INPUT_DIM, INPUT_DIM);
            //resizedMat Still Not Holding to 1024*1024
            //Mat paddedMat = new Mat(INPUT_DIM, INPUT_DIM, resizedMat.Type(), Scalar.All(0));
            int off = Math.Abs(INPUT_DIM - (int)(min_w4h * inner_ratio)) / 2;
            Rectangle roi = new Rectangle(0, 0, INPUT_DIM, INPUT_DIM);
            offset = 0;
            //new Rect(0, 0, resizedMat.Cols - 0, resizedMat.Rows + offset);
            //if (resizedMat.Cols > resizedMat.Rows)
            //{
            //    roi = new Rect(0, off, resizedMat.Cols, resizedMat.Rows);
            //    offset = -off;
            //}
            //else if (resizedMat.Cols < resizedMat.Rows)
            //{
            //    roi = new Rect(off, 0, resizedMat.Cols, resizedMat.Rows);
            //    offset = off;
            //}
            using (Graphics g = Graphics.FromImage(paddedImage))
            {
                //图像插值模式
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(resizedImage, roi.X, roi.Y);
            }
            //resizedMat.CopyTo(paddedMat[roi]);

            // Convert BGR to RGB
            var finalMat = paddedImage;//new Mat();
            //Cv2.CvtColor(paddedMat, rgbMat, ColorConversionCodes.BGR2RGB);
            //Cv2.CvtColor(paddedMat, finalMat, ColorConversionCodes.BGR2GRAY);
            // Create a DenseTensor to hold the image data
            var tensor = new DenseTensor<float>(new[] { 1, 3, INPUT_DIM, INPUT_DIM });

            // Extract pixel data from the Mat and normalize it
            for (int y = 0; y < INPUT_DIM; y++)
            {
                for (int x = 0; x < INPUT_DIM; x++)
                {
                    Color pixel = finalMat.GetPixel(x, y);
                    //Vec3b pixel = finalMat.At<Vec3b>(y, x);
                    tensor[0, 0, y, x] = (pixel.R / 255f); // R channel
                    tensor[0, 1, y, x] = (pixel.G / 255f); // G channel
                    tensor[0, 2, y, x] = (pixel.B / 255f); // B channel
                }
            }
            //Cv2.ImShow("Converted", rgbMat);
            return tensor;
        }
        /// <summary>
        /// 将推理结果适应原图片尺寸
        /// </summary>
        /// <param name="predictions">推理结果</param>
        /// <param name="ratio">比例系数</param>
        /// <param name="offset">偏移系数</param>
        public static void OutputFittingWithRatio(ref List<DocLayoutInferenceResult> predictions, float ratio, int offset)
        {
            foreach (var item in predictions)
            {
                if (offset > 0)
                {
                    item.bbox[0] -= offset;
                    item.bbox[2] -= offset;
                }
                else if (offset < 0)
                {
                    item.bbox[1] += offset;
                    item.bbox[3] += offset;
                }
                item.bbox[0] *= ratio;
                item.bbox[1] *= ratio;
                item.bbox[2] *= ratio;
                item.bbox[3] *= ratio;
                //selectedPredictions.Add(item);
            }
        }
        /// <summary>
        /// 交并比计算
        /// </summary>
        /// <param name="bbox1">集合1</param>
        /// <param name="bbox2">集合2</param>
        /// <returns></returns>
        private float CalculateIoU(float[] bbox1, float[] bbox2)
        {
            // 计算两边界框的 IOU（Intersection over Union）
            var x1 = Math.Max(bbox1[0], bbox2[0]);
            var y1 = Math.Max(bbox1[1], bbox2[1]);
            var x2 = Math.Min(bbox1[2], bbox2[2]);
            var y2 = Math.Min(bbox1[3], bbox2[3]);

            var intersectionArea = Math.Max(0, x2 - x1) * Math.Max(0, y2 - y1);
            var area1 = (bbox1[2] - bbox1[0]) * (bbox1[3] - bbox1[1]);
            var area2 = (bbox2[2] - bbox2[0]) * (bbox2[3] - bbox2[1]);

            var unionArea = area1 + area2 - intersectionArea;

            return intersectionArea / unionArea;
        }
        /// <summary>
        /// 应用NMS非极大值抑制到输出结果
        /// </summary>
        /// <param name="predictions">输入</param>
        /// <param name="iouThreshold">交并比阈值，0~1</param>
        /// <returns></returns>
        private List<DocLayoutInferenceResult> ApplyNMS(List<DocLayoutInferenceResult> predictions, float iouThreshold)
        {
            var selectedPredictions = new List<DocLayoutInferenceResult>();

            while (predictions.Count > 0)
            {
                // 选择置信度最高的框
                var current = predictions[0];
                selectedPredictions.Add(current);

                // 过滤掉与当前框有较大重叠的框
                predictions = predictions.Skip(1)
                                          .Where(pred => CalculateIoU(current.bbox, pred.bbox) < iouThreshold)
                                          .ToList();
            }

            return selectedPredictions;
        }
        /// <summary>
        /// 对文档图片进行版面分析推理
        /// </summary>
        /// <param name="inputTensor">输入张量</param>
        /// <returns>推理结果，结构符合List<(string label, float confidence, float[] bbox)></returns>
        public List<DocLayoutInferenceResult> Inference(DenseTensor<float> inputTensor)
        {
            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("images", inputTensor)
            };

            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);

            var outputTensor = results.First().AsTensor<float>();

            var predictions = new List<DocLayoutInferenceResult>();

            for (int i = 0; i < outputTensor.Length / 6; i++)
            {
                var conf = outputTensor[0, i, 4];
                if (conf > 0.25) // 设置置信度阈值
                {
                    var cls = (int)outputTensor[0, i, 5];
                    var bbox = new float[]
                    {
                        outputTensor[0, i, 0],
                        outputTensor[0, i, 1],
                        outputTensor[0, i, 2],
                        outputTensor[0, i, 3]
                    };
                    predictions.Add(new DocLayoutInferenceResult(cls, conf, bbox));
                }
            }
            //可调参数，交并比阈值（IoU Threshold）
            var results_NMSed = ApplyNMS(predictions, 0.2f);


            return results_NMSed;
        }
        public static DocLayoutCollection DocLayoutInferenceResultTransform(List<DocLayoutInferenceResult> list)
        {
            DocLayoutCollection collection = new DocLayoutCollection();
            foreach(var item in list)
            {
                collection.Add(new DocLayoutItem(item));
            }
            return collection;
        }

        /// <summary>
        /// 标签编号-绘制框颜色字典
        /// </summary>
        internal static readonly Dictionary<int, System.Drawing.Color> labelNumToColors = new Dictionary<int, System.Drawing.Color>
        {
            { 0, System.Drawing.Color.Red },
            { 1, System.Drawing.Color.Black },
            { 2, System.Drawing.Color.DarkCyan },
            { 3, System.Drawing.Color.Green },
            { 4, System.Drawing.Color.OrangeRed },
            { 5, System.Drawing.Color.Purple },
            { 6, System.Drawing.Color.DarkOrange },
            { 7, System.Drawing.Color.Brown },
            { 8, System.Drawing.Color.BlueViolet },
            { 9, System.Drawing.Color.PaleVioletRed },
            { 10, System.Drawing.Color.FromArgb(255,0,255) }
        };
        private bool disposedValue;

        /// <summary>
        /// 在图片上绘制方框
        /// </summary>
        /// <param name="image">System.Drawing.Image位图</param>
        /// <param name="predictions">版面分析条目集合</param>
        /// <returns></returns>
        public static Image DrawBoundingBoxesOnImage(Image image, DocLayoutCollection predictions)
        {
            //深拷贝
            image = new Bitmap(image);
            using (Graphics g = Graphics.FromImage(image))
            {
                foreach (var prediction in predictions)
                {
                    Pen pen = new Pen(labelNumToColors[prediction.LabelNumber], 3); // 红色边框，宽度为3
                    Font font = new Font("Arial", 20);
                    SolidBrush brush = new SolidBrush(labelNumToColors[prediction.LabelNumber]);

                    string label = prediction.LabelName;
                    float confidence = prediction.Confidence;
                    Rectangle bbox = prediction.Box;

                    g.DrawRectangle(pen, bbox);

                    string labelText = $"{label} {confidence * 1.0f:F4}";
                    g.DrawString(labelText, font, brush, bbox.X, bbox.Y - 40); // 在框上加标签
                }
            }

            return image;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                this.session = null;
                this.sessionOptions = null;
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~DocLayoutAnalyzer()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
