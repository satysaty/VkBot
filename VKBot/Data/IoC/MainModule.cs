using Autofac;
using Microsoft.Extensions.DependencyInjection;
using VKBot.Core;
using VKBot.Core.Action;
using VKBot.Core.Bot;
using VKBot.Core.Captcha;
using VKBot.Data;
using VKBot.Interfaces;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Utils.AntiCaptcha;

namespace VKBot.IoC
{
    class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainView>();
            builder.RegisterType<DataContext>();
            builder.RegisterType<AuthView>();
            builder.RegisterType<MainView>().SingleInstance();

            builder.RegisterType<SqlLog>().As<ILog>();
            builder.RegisterType<SqlLog>();

            builder.RegisterType<BotProcessor>();

            builder.RegisterType<DailyFriends>();

            builder.RegisterType<Parser>();
            builder.RegisterType<OnlineAlways>().SingleInstance();

            var services = new ServiceCollection();
            services.AddAudioBypass();
            services.AddSingleton<ICaptchaSolver, CaptchaSolver>();
            builder.RegisterType<VkApi>().WithParameter("serviceCollection", services).SingleInstance();

            base.Load(builder);
        }
    }
}
