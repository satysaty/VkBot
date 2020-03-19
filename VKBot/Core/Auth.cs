using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VKBot.Core
{
    public class Auth
    {
        VkApi Api;

        public Auth(VkApi api)
        {
            Api = api;
        }

        public async Task<bool> Token(string token)
        {
            try
            {        
                await Api.AuthorizeAsync(new ApiAuthParams
                {
                    AccessToken = token,
                    Settings = Settings.All
                });

                if (Api.IsAuthorized)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> Login(string login, string password)
        {
            try
            {
                await Api.AuthorizeAsync(new ApiAuthParams
                {
                    Login = login,
                    Password = password,
                    Settings = Settings.All
                });

                if (Api.IsAuthorized)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Logout()
        {
            try
            {
                await Api.LogOutAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
