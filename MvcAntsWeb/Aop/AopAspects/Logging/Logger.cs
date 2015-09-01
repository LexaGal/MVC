using System.Reflection;
using log4net;
using log4net.Config;

namespace Aop.AopAspects.Logging
{
    public static class Logger
    {
        public static ILog Log;

        public static void Configure()
        {
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            XmlConfigurator.Configure();
        }
    }
}