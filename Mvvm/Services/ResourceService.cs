using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm.Services
{
    public class ResourceService
    {
        public ResourceService(Func<ResourceManager> get_resource_manager, Func<CultureInfo> get_culture)
        {
            this.get_resource_manager = get_resource_manager;
            this.get_culture = get_culture;
        }

        private Func<ResourceManager> get_resource_manager;
        private Func<CultureInfo> get_culture;

        public ResourceManager ResourceManager => get_resource_manager();

        public CultureInfo Culture => get_culture();
    }
}
