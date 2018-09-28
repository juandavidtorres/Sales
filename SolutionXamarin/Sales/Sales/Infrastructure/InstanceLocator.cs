using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Infrastructure
{
    using ViewModels;
    class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator() {
            this.Main = new MainViewModel();
        }
    }
}
