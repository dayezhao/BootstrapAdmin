﻿using Bootstrap.Client.DataAccess;
using Longbow.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Security.Principal;

namespace Bootstrap.Client.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderBarModel : ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        public HeaderBarModel(IIdentity identity)
        {
            var user = UserHelper.RetrieveUserByUserName(identity.Name);
            DisplayName = user.DisplayName;
            UserName = user.UserName;
            SettingsUrl = DictHelper.RetrieveSettingsUrl(AppId);
            ProfilesUrl = DictHelper.RetrieveProfilesUrl(AppId);
            NotisUrl = DictHelper.RetrieveNotisUrl(AppId);

            // set LogoutUrl
            var authHost = ConfigurationManager.Get<BootstrapAdminAuthenticationOptions>().AuthHost;
            var uriBuilder = new UriBuilder(authHost);
            uriBuilder.Path = uriBuilder.Path == "/" ? CookieAuthenticationDefaults.LogoutPath.Value : $"{uriBuilder.Path.TrimEnd('/')}{CookieAuthenticationDefaults.LogoutPath.Value}";
            LogoutUrl = uriBuilder.ToString();

            // set Icon
            var icon = $"/{DictHelper.RetrieveIconFolderPath().Trim('~', '/')}/{user.Icon}";
            Icon = user.Icon.Contains("://", StringComparison.OrdinalIgnoreCase) ? user.Icon : (string.IsNullOrEmpty(ConfigurationManager.GetValue("SimulateUserName", string.Empty)) ? $"{authHost.TrimEnd('/')}{icon}" : "/images/admin.jpg");
            if (!string.IsNullOrEmpty(user.Css)) Theme = user.Css;
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// 获得/设置 用户头像地址
        /// </summary>
        public string Icon { get; }

        /// <summary>
        /// 获得/设置 设置网址
        /// </summary>
        public string SettingsUrl { get; }

        /// <summary>
        /// 获得/设置 个人中心网址
        /// </summary>
        public string ProfilesUrl { get; }

        /// <summary>
        /// 获得 退出登录地址
        /// </summary>
        public string LogoutUrl { get; }

        /// <summary>
        /// 
        /// </summary>
        public string NotisUrl { get; }
    }
}
