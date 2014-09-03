using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Wpms.App.Core.Framework.Services;

namespace WorkQualityReport.Service
{
    public class QualityReportServiceFactory : ServiceBase
    {
        private static IUnityContainer _container;
        private static readonly object Asyn = new object();
        /// <summary>
        /// 获取Container
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer GetContainer()
        {
            if (_container == null)
            {
                lock (Asyn)
                {
                    if (_container == null)
                    {
                        _container = new UnityContainer();
                        LoadConfig(_container);
                    }
                }
            }
            return _container;
        }
        protected override void ConfigureContainer(IUnityContainer container)
        {
            LoadConfig(container);
            _container = container;
        }

        public static void LoadConfig(IUnityContainer container)
        {
            string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"QualityReport\Web.config");
            if (!File.Exists(configFile))
            {
                configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\QualityReport\Web.config");
            }
            container.LoadConfiguration(GetConfigurationSection(configFile));
        }
    }
}
