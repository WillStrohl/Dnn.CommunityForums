﻿// Copyright (c) 2013-2024 by DNN Community
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

namespace DotNetNuke.Modules.ActiveForums.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using ComponentModel.DataAnnotations;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using Enums;
    using DotNetNuke.Services.Log.EventLog;
    using DotNetNuke.Services.Tokens;

    [TableName("activeforums_Forums")]
    [PrimaryKey("ForumID", AutoIncrement = true)] /* ForumID because needs to match property name NOT database column name */
    [Scope("ModuleId")]

    // TODO [Cacheable("activeforums_Forums", CacheItemPriority.Low)] /* TODO: DAL2 caching cannot be used until all CRUD methods use DAL2; must update Save method to use DAL2 rather than stored procedure */
    public class ForumInfo : DotNetNuke.Services.Tokens.IPropertyAccess
    {
        private DotNetNuke.Modules.ActiveForums.Entities.ForumGroupInfo forumGroup;
        private List<DotNetNuke.Modules.ActiveForums.Entities.ForumInfo> subforums;
        private DotNetNuke.Modules.ActiveForums.Entities.PermissionInfo security;
        private DotNetNuke.Modules.ActiveForums.Entities.IPostInfo lastPostInfo;
        private DotNetNuke.Modules.ActiveForums.SettingsInfo mainSettings;
        private FeatureSettings featureSettings;
        private PortalSettings portalSettings;
        private ModuleInfo moduleInfo;
        private int? subscriberCount;
        private string rssLink;
        private List<PropertyInfo> properties;
        private string lastPostSubject;

        public ForumInfo()
        {
            this.PortalSettings = Utilities.GetPortalSettings(this.PortalId);
        }

        public ForumInfo(DotNetNuke.Entities.Portals.PortalSettings portalSettings)
        {
            this.PortalSettings = portalSettings;
            this.PortalId = this.PortalSettings.PortalId;
        }

        [ColumnName("ForumId")]
        public int ForumID { get; set; }

        public int PortalId { get; set; }

        public int ModuleId { get; set; }

        public int ForumGroupId { get; set; }

        public int ParentForumId { get; set; }

        public string ForumName { get; set; }

        public string ForumDesc { get; set; }

        public int SortOrder { get; set; }

        public bool Active { get; set; }

        public bool Hidden { get; set; }

        public int TotalTopics { get; set; }

        public int TotalReplies { get; set; }

        [IgnoreColumn]
        public Uri RequestUri { get; set; }

        [IgnoreColumn]
        public string RawUrl { get; set; }

        [IgnoreColumn]
        public int LastPostID => this.LastReplyId == 0 ? this.LastTopicId : this.LastReplyId;

        public string ForumSettingsKey { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int LastTopicId { get; set; }

        public int LastReplyId { get; set; }

        public string LastPostSubject
        {
            get => this.lastPostSubject ?? (this.lastPostSubject = this.LastPost.Topic.Subject);
            set => this.lastPostSubject = value;
        }

        [IgnoreColumn]
        public DotNetNuke.Modules.ActiveForums.Entities.IPostInfo LastPost
        {
            get => this.lastPostInfo ?? (this.lastPostInfo = this.LastReplyId == 0 ? (DotNetNuke.Modules.ActiveForums.Entities.IPostInfo)new DotNetNuke.Modules.ActiveForums.Controllers.TopicController().GetById(this.LastTopicId) : new DotNetNuke.Modules.ActiveForums.Controllers.ReplyController().GetById(this.LastReplyId));
            set => this.lastPostInfo = value;
        }

        [ColumnName("LastPostAuthorName")]
        public string LastPostUserName { get; set; }

        [ColumnName("LastPostAuthorId")]
        public int LastPostUserID { get; set; }

        [ColumnName("LastPostDate")]
        public DateTime LastPostDateTime { get; set; }

        public int PermissionsId { get; set; }

        public string PrefixURL { get; set; }

        public int SocialGroupId { get; set; }

        public bool HasProperties { get; set; }

        [IgnoreColumn]
        public DotNetNuke.Modules.ActiveForums.Entities.ForumGroupInfo ForumGroup
        {
            get => this.forumGroup ?? (this.forumGroup = this.LoadForumGroup());
            set => this.forumGroup = value;
        }

        internal ForumGroupInfo LoadForumGroup()
        {
            var group = new DotNetNuke.Modules.ActiveForums.Controllers.ForumGroupController().GetById(this.ForumGroupId, this.ModuleId);
            if (group == null)
            {
                var log = new LogInfo { LogTypeKey = Abstractions.Logging.EventLogType.ADMIN_ALERT.ToString() };
                log.LogProperties.Add(new LogDetailInfo("Module", Globals.ModuleFriendlyName));
                var message = string.Format(Utilities.GetSharedResource("[RESX:ForumGroupMissingForForum]"), this.ForumGroupId, this.ForumID);
                log.AddProperty("Message", message);
                LogController.Instance.AddLog(log);

                var ex = new NullReferenceException(string.Format(Utilities.GetSharedResource("[RESX:ForumGroupMissingForForum]"), this.ForumGroupId, this.ForumID));
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                throw ex;
            }

            return group;
        }

        [IgnoreColumn]
        public string GroupName => this.ForumGroup.GroupName;

        [IgnoreColumn]
        public string GroupPrefixURL => this.ForumGroup.PrefixURL;

        [IgnoreColumn]
        public string LastTopicUrl { get; set; }

        [IgnoreColumn]
        public DateTime LastRead { get; set; }

        [IgnoreColumn]
        public string LastPostFirstName
        {
            get
            {
                var name = string.Empty;
                if (this.PortalId >= 0 && this.LastPostUserID > 0)
                {
                    var user = new DotNetNuke.Entities.Users.UserController().GetUser(this.PortalId, this.LastPostUserID);
                    name = user?.FirstName;
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = this.LastPostUserID > 0 ? Utilities.GetSharedResource("[RESX:DeletedUser]") : Utilities.GetSharedResource("[RESX:Anonymous]");
                }

                return name;
            }
        }

        [IgnoreColumn]
        public string LastPostLastName
        {
            get
            {
                var name = string.Empty;
                if (this.PortalId >= 0 && this.LastPostUserID > 0)
                {
                    var user = new DotNetNuke.Entities.Users.UserController().GetUser(this.PortalId, this.LastPostUserID);
                    name = user?.LastName;
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = this.LastPostUserID > 0 ? Utilities.GetSharedResource("[RESX:DeletedUser]") : Utilities.GetSharedResource("[RESX:Anonymous]");
                }

                return name;
            }
        }

        [IgnoreColumn]
        public string LastPostDisplayName
        {
            get
            {
                var name = string.Empty;
                if (this.PortalId >= 0 && this.LastPostUserID > 0)
                {
                    var user = new DotNetNuke.Entities.Users.UserController().GetUser(this.PortalId, this.LastPostUserID);
                    name = user?.DisplayName;
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = this.LastPostUserID > 0 ? Utilities.GetSharedResource("[RESX:DeletedUser]") : Utilities.GetSharedResource("[RESX:Anonymous]");
                }

                return name;
            }
        }

        [IgnoreColumn]
        public bool InheritSecurity => this.PermissionsId == this.ForumGroup.PermissionsId;

        [IgnoreColumn]
        public bool InheritSettings => this.ForumSettingsKey == this.ForumGroup.GroupSettingsKey;

        [IgnoreColumn]
        public int SubscriberCount
        {
            get
            {
                if (this.subscriberCount == null)
                {
                    this.subscriberCount = new DotNetNuke.Modules.ActiveForums.Controllers.SubscriptionController().Count(portalId: this.PortalId, moduleId: this.ModuleId, forumId: this.ForumID);
                    string cachekey = string.Format(CacheKeys.ForumInfo, this.ModuleId, this.ForumID);
                    DataCache.SettingsCacheStore(this.ModuleId, cachekey, this);
                }

                return (int)this.subscriberCount;
            }
        }

        [IgnoreColumn]
        public string ParentForumName => this.ParentForumId > 0 ? new DotNetNuke.Modules.ActiveForums.Controllers.ForumController().GetById(this.ParentForumId, this.ModuleId).ForumName : string.Empty;

        [IgnoreColumn]
        public string ParentForumUrlPrefix => this.ParentForumId > 0 ? new Controllers.ForumController().GetById(this.ParentForumId, this.ModuleId).PrefixURL : string.Empty;

        [IgnoreColumn]
        public int TabId => this.ModuleInfo.TabID;

        [IgnoreColumn]
        public string ForumURL => URL.ForumLink(this.TabId, this);

        [IgnoreColumn]
        public List<DotNetNuke.Modules.ActiveForums.Entities.ForumInfo> SubForums
        {
            get => this.subforums ?? this.LoadSubForums();
            set => this.subforums = value;
        }

        [IgnoreColumn]
        internal List<ForumInfo> LoadSubForums()
        {
            if (this.subforums == null)
            {
                this.subforums = new Controllers.ForumController().GetSubForums(this.ForumID, this.ModuleId).ToList();
                string cachekey = string.Format(CacheKeys.ForumInfo, this.ModuleId, this.ForumID);
                DataCache.SettingsCacheStore(this.ModuleId, cachekey, this);
            }

            return this.subforums;
        }


        [IgnoreColumn]
        public List<DotNetNuke.Modules.ActiveForums.Entities.PropertyInfo> Properties
        {
            get => this.properties ?? (this.properties = this.LoadProperties());
            set => this.properties = value;
        }

        [IgnoreColumn]
        internal List<PropertyInfo> LoadProperties()
        {
            return this.HasProperties ? new DotNetNuke.Modules.ActiveForums.Controllers.PropertyController().Get().Where(p => p.PortalId == this.PortalId && p.ObjectType == 1 && p.ObjectOwnerId == this.ForumID).ToList() : new List<PropertyInfo>();
        }

        [IgnoreColumn]
        public bool GetIsMod(DotNetNuke.Modules.ActiveForums.Entities.ForumUserInfo forumUser)
        {
            return (!(string.IsNullOrEmpty(DotNetNuke.Modules.ActiveForums.Controllers.ForumController.GetForumsForUser(forumUser.UserRoles, this.PortalId, this.ModuleId, "CanApprove"))));
        }

        [IgnoreColumn]
        public string ThemeLocation => Utilities.ResolveUrl(SettingsBase.GetModuleSettings(this.ModuleId).ThemeLocation);

        [IgnoreColumn]
        internal DotNetNuke.Modules.ActiveForums.Enums.ForumStatus GetForumStatusForUser(ForumUserInfo forumUser)
        {
            var canView = DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.HasPerm(this.Security.View, forumUser?.UserRoles);

            if (!canView)
            {
                return DotNetNuke.Modules.ActiveForums.Enums.ForumStatus.Forbidden;
            }

            if (this.LastTopicId == 0)
            {
                return DotNetNuke.Modules.ActiveForums.Enums.ForumStatus.Empty;
            }

            try
            {
                if (forumUser?.UserId == -1 || this.TotalTopics > forumUser?.GetTopicReadCount(this))
                {
                    return DotNetNuke.Modules.ActiveForums.Enums.ForumStatus.UnreadTopics;
                }

                if (this.LastTopicId > forumUser?.GetLastTopicRead(this))
                {
                    return DotNetNuke.Modules.ActiveForums.Enums.ForumStatus.NewTopics;
                }

                return DotNetNuke.Modules.ActiveForums.Enums.ForumStatus.AllTopicsRead;
            }
            catch
            {
                /* this is to handle some limited unit testing without retrieving data */
                return DotNetNuke.Modules.ActiveForums.Enums.ForumStatus.UnreadTopics;
            }
        }

        [IgnoreColumn]
        public DotNetNuke.Modules.ActiveForums.Entities.PermissionInfo Security
        {
            get => this.security ?? (this.security = this.LoadSecurity());
            set => this.security = value;
        }

        internal PermissionInfo LoadSecurity()
        {
            var security = new DotNetNuke.Modules.ActiveForums.Controllers.PermissionController().GetById(this.PermissionsId, this.ModuleId);
            if (security == null)
            {
                security = DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.GetEmptyPermissions(this.ModuleId);
                var log = new LogInfo { LogTypeKey = Abstractions.Logging.EventLogType.ADMIN_ALERT.ToString() };
                log.LogProperties.Add(new LogDetailInfo("Module", Globals.ModuleFriendlyName));
                var message = string.Format(Utilities.GetSharedResource("[RESX:PermissionsMissingForForum]"), this.PermissionsId, this.ForumID);
                log.AddProperty("Message", message);
                LogController.Instance.AddLog(log);
            }
            string cachekey = string.Format(CacheKeys.ForumInfo, this.ModuleId, this.ForumID);
            DataCache.SettingsCacheStore(this.ModuleId, cachekey, this);

            return security;
        }

        [IgnoreColumn]
        public SettingsInfo MainSettings
        {
            get
            {
                if (this.mainSettings == null)
                {
                    this.mainSettings = this.LoadMainSettings();
                    string cachekey = string.Format(CacheKeys.ForumInfo, this.ModuleId, this.ForumID);
                    DataCache.SettingsCacheStore(this.ModuleId, cachekey, this);
                }

                return this.mainSettings;
            }

            set => this.mainSettings = value;
        }

        internal SettingsInfo LoadMainSettings()
        {
            return SettingsBase.GetModuleSettings(this.ModuleId);
        }

        [IgnoreColumn]
        public PortalSettings PortalSettings
        {
            get => this.portalSettings ?? (this.portalSettings = this.LoadPortalSettings());
            set => this.portalSettings = value;
        }

        internal PortalSettings LoadPortalSettings()
        {
            return Utilities.GetPortalSettings(this.PortalId);
        }

        [IgnoreColumn]
        public FeatureSettings FeatureSettings
        {
            get => this.featureSettings ?? (this.featureSettings = this.LoadFeatureSettings());
            set => this.featureSettings = value;
        }

        internal FeatureSettings LoadFeatureSettings()
        {
            return new DotNetNuke.Modules.ActiveForums.Entities.FeatureSettings(moduleId: this.ModuleId, settingsKey: this.ForumSettingsKey);
        }

        [IgnoreColumn]
        public ModuleInfo ModuleInfo
        {
            get => this.moduleInfo ?? (this.moduleInfo = this.LoadModuleInfo());
            set => this.moduleInfo = value;
        }

        internal ModuleInfo LoadModuleInfo()
        {
            return DotNetNuke.Entities.Modules.ModuleController.Instance.GetModule(this.ModuleId, DotNetNuke.Common.Utilities.Null.NullInteger, false);
        }

        [IgnoreColumn]
        public string RssLink
        {
            get
            {
                if (this.rssLink == null)
                {
                    if (this.FeatureSettings.AllowRSS)
                    {
                        var url = new Uri(Utilities.NavigateURL(this.TabId));
                        this.rssLink =
                            $"{url.Scheme}://{url.Host}:{url.Port}/DesktopModules/ActiveForums/feeds.aspx?portalid={this.PortalId}&forumid={this.ForumID}&tabid={this.TabId}&moduleid={this.ModuleId}" +
                            (this.SocialGroupId > 0 ? $"&GroupId={this.SocialGroupId}" : string.Empty);
                    }
                    else
                    {
                        this.rssLink = string.Empty;
                    }
                }

                return this.rssLink;
            }
        }

        #region "Deprecated Methods"

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Not Used.")]
        [IgnoreColumn]
        public int LastPostLastPostID { get; set; }

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Not Used.")]
        [IgnoreColumn]
        public int LastPostParentPostID { get; set; }

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Not Used.")]
        [IgnoreColumn]
        public int CustomFieldType { get; set; }

        #endregion "Deprecated Methods"

        [IgnoreColumn]
        internal string GetForumStatusCss(DotNetNuke.Modules.ActiveForums.Entities.ForumUserInfo forumUser)
        {
            switch (this.GetForumStatusForUser(forumUser))
            {
                case ForumStatus.Forbidden:
                    {
                        return "dcf-forumstatus-no-access";
                    }

                case ForumStatus.Empty:
                    {
                        return "dcf-forumstatus-no-topics";
                    }

                case ForumStatus.NewTopics:
                    {
                        return "dcf-forumstatus-new-topics";
                    }

                case ForumStatus.UnreadTopics:
                    {
                        return "dcf-forumstatus-unread-topics";
                    }

                case ForumStatus.AllTopicsRead:
                    {
                        return "dcf-forumstatus-all-topics-read";
                    }

                default:
                    {
                        return "dcf-forumstatus-all-topics-read";
                    }
            }
        }

        internal string GetForumFolderIcon(DotNetNuke.Modules.ActiveForums.Entities.ForumUserInfo forumUser, DotNetNuke.Modules.ActiveForums.SettingsInfo mainSettings)
        {
            switch (this.GetForumStatusForUser(forumUser))
            {
                case ForumStatus.Forbidden:
                    {
                        return mainSettings.ThemeLocation + "images/folder_forbidden.png";
                    }

                case ForumStatus.Empty:
                    {
                        return mainSettings.ThemeLocation + "images/folder_closed.png";
                    }

                case ForumStatus.NewTopics:
                    {
                        return mainSettings.ThemeLocation + "images/folder_new.png";
                    }

                case ForumStatus.UnreadTopics:
                    {
                        return mainSettings.ThemeLocation + "images/folder_new.png";
                    }

                case ForumStatus.AllTopicsRead:
                    {
                        return mainSettings.ThemeLocation + "images/folder.png";
                    }

                default:
                    {
                        return mainSettings.ThemeLocation + "images/folder.png";
                    }
            }
        }

        internal string GetForumFolderIconCss(DotNetNuke.Modules.ActiveForums.Entities.ForumUserInfo forumUser)
        {
            switch (this.GetForumStatusForUser(forumUser))
            {
                case ForumStatus.Forbidden:
                    {
                        return "fa-folder fa-grey";
                    }

                case ForumStatus.Empty:
                    {
                        return "fa-folder-o fa-grey";
                    }

                case ForumStatus.NewTopics:
                    {
                        return "fa-folder fa-red";
                    }

                case ForumStatus.UnreadTopics:
                    {
                        return "fa-folder fa-red";
                    }

                case ForumStatus.AllTopicsRead:
                    {
                        return "fa-folder fa-blue";
                    }

                default:
                    {
                        return "fa-folder fa-blue";
                    }
            }
        }

        /// <inheritdoc/>
        [IgnoreColumn]
        public DotNetNuke.Services.Tokens.CacheLevel Cacheability => DotNetNuke.Services.Tokens.CacheLevel.notCacheable;

        /// <inheritdoc/>
        public string GetProperty(string propertyName, string format, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, Scope accessLevel, ref bool propertyNotFound)
        {
            if (!DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.HasPerm(this.Security.View, accessingUser.PortalID, this.ModuleId, accessingUser.UserID))
            {
                return string.Empty;
            }

            // replace any embedded tokens in format string
            if (format.Contains("["))
            {
                var tokenReplacer = new DotNetNuke.Modules.ActiveForums.Services.Tokens.TokenReplacer(this.PortalSettings, new DotNetNuke.Modules.ActiveForums.Controllers.ForumUserController(this.ModuleId).GetByUserId(accessingUser.PortalID, accessingUser.UserID), this, this.RequestUri, this.RawUrl)
                {
                    AccessingUser = accessingUser,
                };
                format = tokenReplacer.ReplaceEmbeddedTokens(format);
            }

            int length = -1;
            if (propertyName.Contains(":"))
            {
                var splitPropertyName = propertyName.Split(':');
                propertyName = splitPropertyName[0];
                length = Utilities.SafeConvertInt(splitPropertyName[1], -1);
            }

            propertyName = propertyName.ToLowerInvariant();
            switch (propertyName)
            {
                case "forumid":
                    return PropertyAccess.FormatString(this.ForumID.ToString(), format);
                case "themelocation":
                    return PropertyAccess.FormatString(this.ThemeLocation.ToString(), format);
                case "forumdescription":
                    return PropertyAccess.FormatString(length > 0 && this.ForumDesc.Length > length ? this.ForumDesc.Substring(0, length) : this.ForumDesc, format);
                case "forumname":
                case "forumnamenolink":
                    return PropertyAccess.FormatString(this.ForumName, format);
                case "forumgroupid":
                    return PropertyAccess.FormatString(this.ForumGroupId.ToString(), format);
                case "forummainlink":
                    return PropertyAccess.FormatString(Utilities.NavigateURL(this.TabId), format);
                case "grouplink":
                case "forumgrouplink":
                    return PropertyAccess.FormatString(new ControlUtils().BuildUrl(
                            this.PortalSettings.PortalId,
                            this.GetTabId(),
                            this.ModuleId,
                            this.ForumGroup.PrefixURL,
                            string.Empty,
                            this.ForumGroupId,
                            -1,
                            -1,
                            -1,
                            string.Empty,
                            1,
                            -1,
                            -1),
                        format);
                case "forumlink":
                case "forumurl":
                    return PropertyAccess.FormatString(
                        new ControlUtils().BuildUrl(
                            this.PortalSettings.PortalId,
                            this.GetTabId(),
                            this.ModuleId,
                            this.ForumGroup.PrefixURL,
                            this.PrefixURL,
                            this.ForumGroupId,
                            this.ForumID,
                            -1,
                            -1,
                            string.Empty,
                            1,
                            -1,
                            this.SocialGroupId),
                        format);
                case "parentforumlink":
                    return PropertyAccess.FormatString(new ControlUtils().BuildUrl(
                            this.PortalSettings.PortalId,
                            this.GetTabId(),
                            this.ModuleId,
                            this.ForumGroup.PrefixURL,
                            this.ParentForumUrlPrefix,
                            this.ForumGroupId,
                            this.ParentForumId,
                            -1,
                            -1,
                            string.Empty,
                            1,
                            -1,
                            this.SocialGroupId),
                        format);
                case "parentforumname":
                    return this.ParentForumId < 1
                        ? string.Empty
                        : PropertyAccess.FormatString(this.ParentForumName, format);
                case "parentforumid":
                    return this.ParentForumId < 1
                        ? string.Empty
                        : PropertyAccess.FormatString(this.ParentForumId.ToString(), format);
                case "forumgroupname":
                case "groupname":
                    return PropertyAccess.FormatString(this.ForumGroup.GroupName, format);
                case "socialgroupid":
                    return PropertyAccess.FormatString(this.SocialGroupId.ToString(), format);
                case "lastpostid":
                    return PropertyAccess.FormatString(this.LastPostID.ToString(), format);
                case "subscribercount":
                    return PropertyAccess.FormatString(this.SubscriberCount.ToString(), format);
                case "totaltopics":
                    return PropertyAccess.FormatString(this.TotalTopics.ToString(), format);
                case "totalreplies":
                    return PropertyAccess.FormatString(this.TotalReplies.ToString(), format);
                case "lastpostsubject":
                    return this.LastPostID < 1
                        ? string.Empty
                        : PropertyAccess.FormatString(DotNetNuke.Modules.ActiveForums.Controllers.ForumController.GetLastPostSubjectLinkTag(this.LastPost, length > 0 ? length : this.LastPostSubject.Length, this, this.GetTabId()), format);
                case "lastpostdate":
                    return this.LastPostID < 1
                        ? string.Empty
                        : PropertyAccess.FormatString(Utilities.GetUserFormattedDateTime(
                                (DateTime?)this.LastPostDateTime,
                                formatProvider,
                                accessingUser.Profile.PreferredTimeZone.GetUtcOffset(DateTime.UtcNow)),
                            format);
                case "lastpostuserid":
                    return this.LastPostID < 1
                        ? string.Empty
                        : PropertyAccess.FormatString(this.LastPostUserID.ToString(), format);
                case "lastpostusername":
                    return this.LastPostID < 1
                        ? string.Empty
                        : PropertyAccess.FormatString(this.LastPostUserName, format);
                case "lastpostfirstname":
                    return this.LastPostID < 1
                        ? string.Empty
                        : PropertyAccess.FormatString(this.LastPostFirstName, format);
                case "lastpostlastname":
                    return this.LastPostID < 1
                        ? string.Empty
                        : PropertyAccess.FormatString(this.LastPostLastName, format);
                case "lastpostauthordisplaynamelink":
                    return this.LastPostID > 0 && this.LastPostUserID > 0 && Controllers.ForumUserController.CanLinkToProfile(
                            this.PortalSettings,
                            this.MainSettings,
                            this.ModuleId,
                            new Controllers.ForumUserController(this.ModuleId).GetByUserId(
                                accessingUser.PortalID,
                                accessingUser.UserID),
                            new Controllers.ForumUserController(this.ModuleId).GetByUserId(
                                accessingUser.PortalID,
                                this.LastPostUserID))
                            ? PropertyAccess.FormatString(Utilities.NavigateURL(this.PortalSettings.UserTabId, string.Empty, new[] { $"userId={this.LastPostUserID}" }), format)
                            : string.Empty;
                case "lastpostdisplayname":
                case "lastpostauthordisplayname":
                    var forumUserController = new Controllers.ForumUserController(this.ModuleId);
                    return this.LastPostID > 0 && this.LastPostUserID > 0
                            ? PropertyAccess.FormatString(Controllers.ForumUserController.GetDisplayName(
                                        this.PortalSettings,
                                        this.MainSettings,
                                        forumUserController.GetByUserId(accessingUser.PortalID, accessingUser.UserID)
                                            .GetIsMod(this.ModuleId),
                                        forumUserController.GetUserIsAdmin(accessingUser.PortalID,
                                            this.ModuleId,
                                            accessingUser.UserID) ||
                                        forumUserController.GetUserIsSuperUser(accessingUser.PortalID,
                                            this.ModuleId,
                                            accessingUser.UserID),
                                        this.LastPostUserID,
                                        this.LastPostUserName,
                                        this.LastPostFirstName,
                                        this.LastPostLastName,
                                        this.LastPostDisplayName).Replace("&amp;#", "&#")
                                    .Replace("Anonymous", this.LastPostDisplayName),
                                format)
                            : string.Empty;

                case "statuscssclass":
                    return PropertyAccess.FormatString(
                        this.GetForumStatusCss(new Controllers.ForumUserController(this.ModuleId).GetByUserId(accessingUser.PortalID, accessingUser.UserID)), format);
                case "forumicon":
                    return PropertyAccess.FormatString(
                        this.GetForumFolderIcon(new Controllers.ForumUserController(this.ModuleId).GetByUserId(accessingUser.PortalID, accessingUser.UserID), this.MainSettings), format);
                case "forumiconcss":
                    return PropertyAccess.FormatString(
                        this.GetForumFolderIconCss(new Controllers.ForumUserController(this.ModuleId).GetByUserId(
                            accessingUser.PortalID,
                            accessingUser.UserID)),
                        format);
                case "rsslink":
                    return this.FeatureSettings.AllowRSS && Controllers.PermissionController.HasPerm(this.Security.Read,
                            new Controllers.ForumUserController(this.ModuleId)
                                .GetByUserId(accessingUser.PortalID, accessingUser.UserID).UserRoles)
                        ? PropertyAccess.FormatString(this.RssLink, format)
                        : string.Empty;
            }

            propertyNotFound = true;
            return string.Empty;
        }

        private int GetTabId()
        {
            return this.PortalSettings.ActiveTab.TabID == -1 || this.PortalSettings.ActiveTab.TabID == this.PortalSettings.HomeTabId ? this.TabId : this.PortalSettings.ActiveTab.TabID;
        }
    }
}
