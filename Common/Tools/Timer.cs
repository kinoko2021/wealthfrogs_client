using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WealthFrogs.Common.Tools
{
    // 模仿 JavaScript 里的 setTimeout 和 setInterval
    public static class Timer
    { 
         public static void SetTimeout(double interval, Action action)
         { 
             System.Timers.Timer timer = new System.Timers.Timer(interval); 
             timer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e) 
             { 
                 timer.Enabled = false; 
                 action(); 
             }; 
             timer.Enabled = true; 
         }

         public static void SetInterval(double interval, Action<ElapsedEventArgs> action)
         {
           System.Timers.Timer timer = new System.Timers.Timer(interval);
           timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
         {
           action(e);
           };
         timer.Enabled = true;
         } 


    }
}
