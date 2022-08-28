using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Models
{
    public class StockOrderRequestParam
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string stockId { get; set; }

        /// <summary>
        /// 股票价格
        /// </summary>
        public double price { get; set; }

        /// <summary>
        /// 股数
        /// </summary>
        public int payload { get; set; }

        /// <summary>
        /// 0: buy, 1: sell
        /// </summary>
        public int direction { get; set; }
    }
}
