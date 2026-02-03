using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace AvsCard.Wpf.Test.Logging
{
    public static class LoggerFactoryBuilder
    {
        #region Public Methods

        public static ILoggerFactory Create()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.test.json", optional: true)
                .Build();

            return LoggerFactory.Create(builder =>
            {
                builder.AddConfiguration(config.GetSection("Logging"));
                builder.AddConsole();
            });
        }

        #endregion Public Methods
    }
}