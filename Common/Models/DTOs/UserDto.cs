using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Common.Models.DTOs
{
    public class UserDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}
