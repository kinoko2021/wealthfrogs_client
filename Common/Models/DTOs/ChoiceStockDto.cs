using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Models
{
    public class ChoiceStockDto
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string stock_id { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 现价
        /// </summary>
        public double price { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public double fluctuation { get; set; }
        /// <summary>
        /// 涨跌百分比
        /// </summary>
        public double fluctuation_ratio { get; set; }
    }
}
