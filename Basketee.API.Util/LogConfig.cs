using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.Util
{
    public static class LogConfig
    {
        public static void ProductionConfig()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%property{date1} %property{RequestIp} %property{customMessage}%newline";
            patternLayout.ActivateOptions();
            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;
            roller.LockingModel = new FileAppender.MinimalLock();
            roller.File = @"logs\";
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "2048KB";
            roller.DatePattern = "dd.MM.yyyy'.log'";
            roller.RollingStyle = RollingFileAppender.RollingMode.Composite;
            roller.StaticLogFileName = false;
            roller.PreserveLogFileNameExtension = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);
            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);
            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
        }

        public static void DebugConfig()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date|||[%thread]|||%-5level|||%property{method_name}|||%property{type}|||%property{model}|||%message%newline";
            patternLayout.ActivateOptions();
            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;
            roller.LockingModel = new FileAppender.MinimalLock();
            roller.File = @"logs\";
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "2048KB";
            roller.DatePattern = "dd.MM.yyyy'.log'";
            roller.RollingStyle = RollingFileAppender.RollingMode.Composite;
            roller.StaticLogFileName = false;
            roller.PreserveLogFileNameExtension = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);
            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);
            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
        }
    }
}
