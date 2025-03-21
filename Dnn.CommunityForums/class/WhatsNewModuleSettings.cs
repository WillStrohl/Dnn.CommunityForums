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

namespace DotNetNuke.Modules.ActiveForums
{
    using System;
    using System.Collections;

    using DotNetNuke.Entities.Modules;

    public class WhatsNewModuleSettings
    {
        public const string RowsSettingsKey = "AFTopPostsNumber";
        public const string ForumsSettingsKey = "AFTopPostsForums";
        public const string RSSEnabledSettingsKey = "AFTopPostsRSS";
        public const string RSSIgnoreSecuritySettingsKey = "AFTopPostsSecurity";
        public const string RSSIncludeBodySettingsKey = "AFTopPostsBody";
        public const string RSSCacheTimeoutSettingsKey = "AFTopPostsCache";
        public const string TopicsOnlySettingsKey = "AFTopPostsTopicsOnly";
        public const string RandomOrderSettingsKey = "AFTopPostsRandomOrder";
        public const string TagsSettingsKey = "AFTopPostsTags";
        public const string HeaderSettingsKey = "AFTopPostsHeader";
        public const string FooterSettingsKey = "AFTopPostsFooter";
        public const string FormatSettingsKey = "AFTopPostsFormat";

        public const int DefaultRows = 5;
        public const string DefaultForums = "";
        public const bool DefaultRSSEnabled = true;
        public const bool DefaultRSSIgnoreSecurity = false;
        public const bool DefaultRSSIncludeBody = false;
        public const int DefaultRSSCacheTimeout = 30;
        public const bool DefaultTopicsOnly = true;
        public const bool DefaultRandomOrder = false;
        public const string DefaultTags = "";
        public const string DefaultHeader = "<div style=\"padding:25px;padding-top:35px;\">";
        public const string DefaultFooter = "[RSSICONLINK]</div>";
        public const string DefaultFormat = "<div style=\"padding-bottom:5px;\" class=\"Normal\">[SUBJECTLINK]</div>";

        public int Rows { get; set; }

        public string Forums { get; set; }

        public bool RSSEnabled { get; set; }

        public bool RSSIgnoreSecurity { get; set; }

        public bool RSSIncludeBody { get; set; }

        public int RSSCacheTimeout { get; set; }

        public bool TopicsOnly { get; set; }

        public bool RandomOrder { get; set; }

        public string Tags { get; set; }

        public string Header { get; set; }

        public string Footer { get; set; }

        public string Format { get; set; }

        public bool Save(ModuleController moduleController, int moduleId)
        {
            try
            {
                if (moduleController == null || moduleId < 0)
                {
                    return false;
                }

                moduleController.UpdateModuleSetting(moduleId, ForumsSettingsKey, this.Forums);
                moduleController.UpdateModuleSetting(moduleId, RowsSettingsKey, this.Rows.ToString());
                moduleController.UpdateModuleSetting(moduleId, FormatSettingsKey, this.Format);
                moduleController.UpdateModuleSetting(moduleId, HeaderSettingsKey, this.Header);
                moduleController.UpdateModuleSetting(moduleId, FooterSettingsKey, this.Footer);
                moduleController.UpdateModuleSetting(moduleId, RSSEnabledSettingsKey, this.RSSEnabled.ToString());
                moduleController.UpdateModuleSetting(moduleId, TopicsOnlySettingsKey, this.TopicsOnly.ToString());
                moduleController.UpdateModuleSetting(moduleId, RandomOrderSettingsKey, this.RandomOrder.ToString());
                moduleController.UpdateModuleSetting(moduleId, TagsSettingsKey, this.Tags);
                moduleController.UpdateModuleSetting(moduleId, RSSIgnoreSecuritySettingsKey, this.RSSIgnoreSecurity.ToString());
                moduleController.UpdateModuleSetting(moduleId, RSSIncludeBodySettingsKey, this.RSSIncludeBody.ToString());
                moduleController.UpdateModuleSetting(moduleId, RSSCacheTimeoutSettingsKey, this.RSSCacheTimeout.ToString());

                // Clear the cache
                DataCache.SettingsCacheClear(moduleId, string.Format(CacheKeys.WhatsNew, moduleId));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static WhatsNewModuleSettings CreateFromModuleSettings(Hashtable moduleSettings)
        {
            if (moduleSettings == null)
            {
                return new WhatsNewModuleSettings
                {
                    Rows = DefaultRows,
                    Forums = DefaultForums,
                    RSSEnabled = DefaultRSSEnabled,
                    RSSIgnoreSecurity = DefaultRSSIgnoreSecurity,
                    RSSIncludeBody = DefaultRSSIncludeBody,
                    RSSCacheTimeout = DefaultRSSCacheTimeout,
                    TopicsOnly = DefaultTopicsOnly,
                    RandomOrder = DefaultRandomOrder,
                    Tags = DefaultTags,
                    Header = DefaultHeader,
                    Footer = DefaultFooter,
                    Format = DefaultFormat,
                };
            }

            return new WhatsNewModuleSettings
            {
                Rows = Utilities.IsNumeric(moduleSettings[RowsSettingsKey]) ? Convert.ToInt32(moduleSettings[RowsSettingsKey]) : DefaultRows,
                Forums = (moduleSettings[ForumsSettingsKey] != null) ? Convert.ToString(moduleSettings[ForumsSettingsKey]) : DefaultForums,
                RSSEnabled = Utilities.IsNumeric(moduleSettings[RSSEnabledSettingsKey]) ? Convert.ToBoolean(moduleSettings[RSSEnabledSettingsKey]) : DefaultRSSEnabled,
                RSSIgnoreSecurity = Utilities.IsNumeric(moduleSettings[RSSIgnoreSecuritySettingsKey]) ? Convert.ToBoolean(moduleSettings[RSSIgnoreSecuritySettingsKey]) : DefaultRSSIgnoreSecurity,
                RSSIncludeBody = Utilities.IsNumeric(moduleSettings[RSSIncludeBodySettingsKey]) ? Convert.ToBoolean(moduleSettings[RSSIncludeBodySettingsKey]) : DefaultRSSIncludeBody,
                RSSCacheTimeout = Utilities.IsNumeric(moduleSettings[RSSCacheTimeoutSettingsKey]) ? Convert.ToInt32(moduleSettings[RSSCacheTimeoutSettingsKey]) : DefaultRSSCacheTimeout,
                TopicsOnly = Utilities.IsNumeric(moduleSettings[TopicsOnlySettingsKey]) ? Convert.ToBoolean(moduleSettings[TopicsOnlySettingsKey]) : DefaultTopicsOnly,
                RandomOrder = Utilities.IsNumeric(moduleSettings[RandomOrderSettingsKey]) ? Convert.ToBoolean(moduleSettings[RandomOrderSettingsKey]) : DefaultRandomOrder,
                Tags = (moduleSettings[TagsSettingsKey] != null) ? Convert.ToString(moduleSettings[TagsSettingsKey]) : DefaultTags,
                Header = (moduleSettings[HeaderSettingsKey] != null) ? Convert.ToString(moduleSettings[HeaderSettingsKey]) : DefaultHeader,
                Footer = (moduleSettings[FooterSettingsKey] != null) ? Convert.ToString(moduleSettings[FooterSettingsKey]) : DefaultFooter,
                Format = (moduleSettings[FormatSettingsKey] != null) ? Convert.ToString(moduleSettings[FormatSettingsKey]) : DefaultFormat,
            };
        }
    }
}
