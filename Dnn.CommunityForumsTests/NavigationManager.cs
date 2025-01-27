﻿// Copyright (c) by DNN Community
//
// DNN Community licenses this file to you under the MIT license.
//
// See the LICENSE file in the project root for more information.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

namespace DotNetNuke.Modules.ActiveForums.Services.Tests
{
    using System.Linq;
    using System.Threading;

    using DotNetNuke.Abstractions;
    using DotNetNuke.Abstractions.Portals;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Services.Localization;

    /// <summary>The default <see cref="INavigationManager"/> implementation.</summary>
    internal class NavigationManager : INavigationManager
    {
        private readonly IPortalController portalController;

        /// <summary>Initializes a new instance of the <see cref="NavigationManager"/> class.</summary>
        /// <param name="portalController">An <see cref="IPortalController"/> instance.</param>
        public NavigationManager(IPortalController portalController)
        {
            this.portalController = portalController;
        }

        /// <summary>Gets the URL to the current page.</summary>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL()
        {
            return this.NavigateURL(TabController.CurrentPage.TabID, Null.NullString);
        }

        /// <summary>Gets the URL to the given page.</summary>
        /// <param name="tabID">The tab ID.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(int tabID)
        {
            return this.NavigateURL(tabID, Null.NullString);
        }

        /// <summary>Gets the URL to the given page.</summary>
        /// <param name="tabID">The tab ID.</param>
        /// <param name="isSuperTab">if set to <c>true</c> the page is a "super-tab," i.e. a host-level page.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(int tabID, bool isSuperTab)
        {
            var portalSettings = this.portalController.GetCurrentSettings();
            return this.NavigateURL(tabID, isSuperTab, portalSettings, Null.NullString, "en-US");
        }

        /// <summary>Gets the URL to show the control associated with the given control key.</summary>
        /// <param name="controlKey">The control key, or <see cref="string.Empty"/> or <c>null</c>.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(string controlKey)
        {
            if (controlKey == "Access Denied")
            {
                return DotNetNuke.Common.Globals.AccessDeniedURL();
            }

            return this.NavigateURL(TabController.CurrentPage.TabID, controlKey);
        }

        /// <summary>Gets the URL to show the control associated with the given control key.</summary>
        /// <param name="controlKey">The control key, or <see cref="string.Empty"/> or <c>null</c>.</param>
        /// <param name="additionalParameters">Any additional parameters, in <c>"key=value"</c> format.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(string controlKey, params string[] additionalParameters)
        {
            return this.NavigateURL(TabController.CurrentPage?.TabID ?? -1, controlKey, additionalParameters);
        }

        /// <summary>Gets the URL to show the control associated with the given control key on the given page.</summary>
        /// <param name="tabID">The tab ID.</param>
        /// <param name="controlKey">The control key, or <see cref="string.Empty"/> or <c>null</c>.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(int tabID, string controlKey)
        {
            return this.NavigateURL(tabID, this.portalController.GetCurrentSettings(), controlKey, null);
        }

        /// <summary>Gets the URL to show the given page.</summary>
        /// <param name="tabID">The tab ID.</param>
        /// <param name="controlKey">The control key, or <see cref="string.Empty"/> or <c>null</c>.</param>
        /// <param name="additionalParameters">Any additional parameters.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(int tabID, string controlKey, params string[] additionalParameters)
        {
            return this.NavigateURL(tabID, this.portalController.GetCurrentSettings(), controlKey, additionalParameters);
        }

        /// <summary>Gets the URL to show the given page.</summary>
        /// <param name="tabID">The tab ID.</param>
        /// <param name="settings">The portal settings.</param>
        /// <param name="controlKey">The control key, or <see cref="string.Empty"/> or <c>null</c>.</param>
        /// <param name="additionalParameters">Any additional parameters.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(int tabID, IPortalSettings settings, string controlKey, params string[] additionalParameters)
        {
            var isSuperTab = DotNetNuke.Common.Globals.IsHostTab(tabID);
            return this.NavigateURL(tabID, isSuperTab, settings, controlKey, additionalParameters);
        }

        /// <summary>Gets the URL to show the given page.</summary>
        /// <param name="tabID">The tab ID.</param>
        /// <param name="isSuperTab">if set to <c>true</c> the page is a "super-tab," i.e. a host-level page.</param>
        /// <param name="settings">The portal settings.</param>
        /// <param name="controlKey">The control key, or <see cref="string.Empty"/> or <c>null</c>.</param>
        /// <param name="additionalParameters">Any additional parameters.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(int tabID, bool isSuperTab, IPortalSettings settings, string controlKey, params string[] additionalParameters)
        {
            return this.NavigateURL(tabID, isSuperTab, settings, controlKey, "en-US", additionalParameters);
        }

        /// <summary>Gets the URL to show the given page.</summary>
        /// <param name="tabID">The tab ID.</param>
        /// <param name="isSuperTab">if set to <c>true</c> the page is a "super-tab," i.e. a host-level page.</param>
        /// <param name="settings">The portal settings.</param>
        /// <param name="controlKey">The control key, or <see cref="string.Empty"/> or <c>null</c>.</param>
        /// <param name="language">The language code.</param>
        /// <param name="additionalParameters">Any additional parameters.</param>
        /// <returns>Formatted URL.</returns>
        public string NavigateURL(int tabID, bool isSuperTab, IPortalSettings settings, string controlKey, string language, params string[] additionalParameters)
        {
            return this.NavigateURL(tabID, isSuperTab, settings, controlKey, language, DotNetNuke.Common.Globals.glbDefaultPage, additionalParameters);
        }

        /// <summary>Gets the URL to show the given page.</summary>
        /// <param name="tabID">The tab ID.</param>
        /// <param name="isSuperTab">if set to <c>true</c> the page is a "super-tab," i.e. a host-level page.</param>
        /// <param name="settings">The portal settings.</param>
        /// <param name="controlKey">The control key, or <see cref="string.Empty"/> or <c>null</c>.</param>
        /// <param name="language">The language code.</param>
        /// <param name="pageName">The page name to pass to <see cref="FriendlyUrl(DotNetNuke.Entities.Tabs.TabInfo,string,string)"/>.</param>
        /// <param name="additionalParameters">Any additional parameters.</param>
        /// <returns>Formatted url.</returns>
        public string NavigateURL(int tabID, bool isSuperTab, IPortalSettings settings, string controlKey, string language, string pageName, params string[] additionalParameters)
        {
            var url = tabID == Null.NullInteger ? DotNetNuke.Common.Globals.ApplicationURL() : DotNetNuke.Common.Globals.ApplicationURL(tabID);
            if (!string.IsNullOrEmpty(controlKey))
            {
                url += "&ctl=" + controlKey;
            }

            if (additionalParameters != null)
            {
                url = additionalParameters.Where(parameter => !string.IsNullOrEmpty(parameter)).Aggregate(url, (current, parameter) => current + ("&" + parameter));
            }

            if (isSuperTab)
            {
                url += "&portalid=" + settings.PortalId;
            }

            TabInfo tab = null;

            if (settings != null)
            {
                tab = TabController.Instance.GetTab(tabID, isSuperTab ? Null.NullInteger : settings.PortalId, false);
            }

            // only add language to url if more than one locale is enabled
            if (settings != null && language != null && LocaleController.Instance.GetLocales(settings.PortalId).Count > 1)
            {
                if (settings.ContentLocalizationEnabled)
                {
                    if (language == string.Empty)
                    {
                        if (tab != null && !string.IsNullOrEmpty(tab.CultureCode))
                        {
                            url += "&language=" + tab.CultureCode;
                        }
                    }
                    else
                    {
                        url += "&language=" + language;
                    }
                }
                else if (settings.EnableUrlLanguage)
                {
                    // legacy pre 5.5 behavior
                    if (language == string.Empty)
                    {
                        url += "&language=" + Thread.CurrentThread.CurrentCulture.Name;
                    }
                    else
                    {
                        url += "&language=" + language;
                    }
                }
            }

            if (Host.UseFriendlyUrls || Config.GetFriendlyUrlProvider() == "advanced")
            {
                if (string.IsNullOrEmpty(pageName))
                {
                    pageName = DotNetNuke.Common.Globals.glbDefaultPage;
                }

                url = (settings == null) ? DotNetNuke.Common.Globals.FriendlyUrl(tab, url, pageName) : DotNetNuke.Common.Globals.FriendlyUrl(tab, url, pageName, settings);
            }
            else
            {
                url = DotNetNuke.Common.Globals.ResolveUrl(url);
            }

            return url;
        }
    }
}
