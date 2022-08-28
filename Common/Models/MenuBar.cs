using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Models
{
    /// <summary>
    /// 导航 drawer 实体类
    /// </summary>
    public class MenuBar : BindableBase
    {
        public string Icon { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// 菜单命名空间
        /// </summary>
        public string NameSpace { get; set; }
    }
}
