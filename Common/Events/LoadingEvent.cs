using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Common.Events
{

    public class LoadingModel
    {
        public bool IsLoading { get; set; }
    }

    internal class LoadingEvent : PubSubEvent<LoadingModel>
    {
    }
}
