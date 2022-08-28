using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Models
{
    public class StockDetailDto
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string stock_id { get; set; }
        /// <summary>
        /// 股票名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 现价
        /// </summary>
        public double price { get; set; }
        /// <summary>
        /// 昨收
        /// </summary>
        public double closing_price { get; set; }
        /// <summary>
        /// 今开
        /// </summary>
        public double opening_price { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public int volume { get; set; }
        /// <summary>
        /// 外盘
        /// </summary>
        public int foreign { get; set; }
        /// <summary>
        /// 内盘
        /// </summary>
        public int domestic { get; set; }
        /// <summary>
        /// 买档
        /// </summary>
        public List<List<double>> buy_level { get; set; }
        /// <summary>
        /// 卖档
        /// </summary>
        public List<List<double>> sell_level { get; set; }
        /// <summary>
        /// 涨跌
        /// </summary>
        public double fluctuation { get; set; }
        /// <summary>
        /// 涨跌幅百分比
        /// </summary>
        public double fluctuation_ratio { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public double top_price { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public double bottom_price { get; set; }
        /// <summary>
        /// 换手率
        /// </summary>
        public double turnover_rate { get; set; }
        /// <summary>
        /// ttm 市盈率
        /// </summary>
        public double ttm { get; set; }
        /// <summary>
        /// 涨停价
        /// </summary>
        public double max_price { get; set; }
        /// <summary>
        /// 跌停价
        /// </summary>
        public double min_price { get; set; }
    }

}
