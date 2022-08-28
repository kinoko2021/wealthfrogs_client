using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Models
{
    public class LevelItem
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string buyLevel { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public double buyPrice { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public int buyVolume { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string sellLevel { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public double sellPrice { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public int sellVolume { get; set; }
    }
}
