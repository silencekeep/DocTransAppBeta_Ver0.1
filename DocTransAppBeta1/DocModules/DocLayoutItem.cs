using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DocTransAppBeta1.DocModules.DocLayoutAnalyzer;

namespace DocTransAppBeta1.DocModules
{
    /// <summary>
    /// 预测标签枚举
    /// </summary>
    public enum DocLayoutLabel
    {
        title, plain_text, abandon, figure, figure_caption,
        table, table_caption, table_footnote, isolate_formula, formula_caption,
        unknown
    }
    /// <summary>
    /// 推理结果类，要与DocLayoutAnalyzer共用。
    /// </summary>
    public class DocLayoutItem
    {
        private DocLayoutLabel label = DocLayoutLabel.unknown;
        public DocLayoutLabel Label { 
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }
        public string LabelName
        {
            get
            {
                return label.ToString();
            }
        }
        public int LabelNumber 
        { 
            get 
            { 
                return (int)label; 
            } 
        }
        public DocLayoutItem(DocLayoutLabel label, float confidence, RectangleF rectangle)
        {
            Label = label;
            Confidence = confidence;
            rectangleF = rectangle;
        }
        public DocLayoutItem(DocLayoutInferenceResult inferenceResult)
        {
            Label = inferenceResult.label;
            Confidence = inferenceResult.confidence;
            var arr = inferenceResult.bbox;
            rectangleF = new RectangleF(arr[0], arr[1], arr[2] - arr[0], arr[3] - arr[1]);
        }
        public float Confidence { get; set; }

        public RectangleF rectangleF;
        public Rectangle Box
        {
            get
            {
                return new Rectangle(
                    (int)Math.Floor(rectangleF.X),
                    (int)Math.Floor(rectangleF.Y),
                    (int)Math.Ceiling(rectangleF.Width),
                    (int)Math.Ceiling(rectangleF.Height)
                );
            }
        }
        public RectangleF BoxFloat
        {
            get
            {
                return rectangleF;
            }
            set
            {
                rectangleF = value;
            }
        }
        public void SetBoxByDim4FloatArray(float[] arr)
        {
            rectangleF = new RectangleF(arr[0], arr[1], arr[2] - arr[0], arr[3] - arr[1]);
        }
    }
}
