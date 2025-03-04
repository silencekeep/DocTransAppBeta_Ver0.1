using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * DocLayoutCollection.cs
 * DocLayoutYOLO后推理数据集合存储方法
 * 依赖第三方包：无
 */
namespace DocTransAppBeta1.DocModules
{
    /// <summary>
    /// 构建的简单集合，多出来功能支持根据标签分类，不过发现没卵用
    /// </summary>
    public class DocLayoutCollection : List<DocLayoutItem>
    {
        /// <summary>
        /// 添加排版元素到排版结果集合中。
        /// </summary>
        /// <param name="item">排版元素</param>
        public new void Add(DocLayoutItem item)
        {
            base.Add(item);
        }

        public new DocLayoutItem this[int index]
        {
            get => base[index];
            set => base[index] = value;
        }
        /// <summary>
        /// 从集合中获取所有布局元素枚举，按Y轴大小排序。
        /// </summary>
        /// <param name="label">标签类型</param>
        /// <returns></returns>
        public IEnumerable<DocLayoutItem> GetItemsByYAxis()
        {
            return this.OrderBy(item => item.BoxFloat.Y);
        }
        /// <summary>
        /// 从集合中获取指定类型的所有布局元素枚举。
        /// </summary>
        /// <param name="label">标签类型</param>
        /// <returns></returns>
        public IEnumerable<DocLayoutItem> GetItemsByLabel(DocLayoutLabel label)
        {
            return this.Where(item => item.Label == label);
        }

        /// <summary>
        /// 从集合中获取指定类型的所有布局元素，并返回一个新的布局元素集合。
        /// </summary>
        /// <param name="label">标签类型</param>
        /// <returns></returns>
        public DocLayoutCollection GetCollectionByLabel(DocLayoutLabel label)
        {
            var collection = new DocLayoutCollection();
            foreach (var item in this.Where(item => item.Label == label))
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}
