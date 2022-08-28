using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Models.DTOs
{
    public class StockOrderDto
    {
        /// <summary>
        /// 订单 id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 订单用户 id
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 股票代码
        /// </summary>
        public string stock_id { get; set; }
        /// <summary>
        /// 买:1 卖:2
        /// </summary>
        public int direction { get; set; }
        /// <summary>
        /// 已报:0 部成:1 已撤:2 部成已撤:3 已成:4
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 报单数
        /// </summary>
        public int payload { get; set; }
        /// <summary>
        /// 成交数
        /// </summary>
        public int deal { get; set; }
        /// <summary>
        /// 报价
        /// </summary>
        public double price { get; set; }
        /// <summary>
        /// 下单时间戳
        /// </summary>
        public long timestamp { get; set; }
    }
}
