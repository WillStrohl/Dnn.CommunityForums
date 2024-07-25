﻿//
// Community Forums
// Copyright (c) 2013-2024
// by DNN Community
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
//

namespace DotNetNuke.Modules.ActiveForums.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Microsoft.ApplicationBlocks.Data;

    public class Common : DataConfig
    {
        #region Security
        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 9.00.00. Not used from at least v4 forward.")]
        public void SavePermissionSet(int PermissionSetId, string PermissionSet) => SqlHelper.ExecuteNonQuery(this.connectionString, this.dbPrefix + "Permissions_Save", PermissionSetId, PermissionSet);

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 9.00.00. Not used from at least v4 forward.")]
        public IDataReader GetRoles(int PortalId) => SqlHelper.ExecuteReader(this.connectionString, this.dbPrefix + "Permissions_GetRoles", PortalId);

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Use DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.GetPermSet.")]
        public string GetPermSet(int PermissionsId, string requestedAccess) => new DotNetNuke.Modules.ActiveForums.Controllers.PermissionController().GetPermSet(-1, PermissionsId, requestedAccess);

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Use DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.SavePermSet.")]
        public string SavePermSet(int PermissionsId, string requestedAccess, string PermSet) => new DotNetNuke.Modules.ActiveForums.Controllers.PermissionController().SavePermSet(-1, PermissionsId, requestedAccess, PermSet);

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Use DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.CreateAdminPermissions().")]
        public int CreatePermSet(string AdminRoleId) => new DotNetNuke.Modules.ActiveForums.Controllers.PermissionController().CreateAdminPermissions(AdminRoleId, -1).PermissionsId;

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Use DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.CheckForumIdsForView().")]
        public string CheckForumIdsForView(int ModuleId, string ForumIds, string UserRoles) => DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.CheckForumIdsForViewForRSS(ModuleId, ForumIds, UserRoles);

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Use DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.WhichRolesCanViewForum().")]
        public string WhichRolesCanViewForum(int ModuleId, int ForumId, string UserRoles) => DotNetNuke.Modules.ActiveForums.Controllers.PermissionController.WhichRolesCanViewForum(ModuleId, ForumId, UserRoles);

        #endregion

        #region Views

        public DataSet UI_ActiveView(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, int timeFrame, string forumIds)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_ActiveView", portalId, moduleId, userId, rowIndex, maxRows, sort, timeFrame, forumIds);
        }

        public DataSet UI_NotReadView(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, string forumIds)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_NotRead", portalId, moduleId, userId, rowIndex, maxRows, sort, forumIds);
        }

        public DataSet UI_UnansweredView(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, string forumIds)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_UnansweredView", portalId, moduleId, userId, rowIndex, maxRows, sort, forumIds);
        }

        public DataSet UI_TagsView(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, string forumIds, int tagId)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_TagsView", portalId, moduleId, userId, rowIndex, maxRows, sort, forumIds, tagId);
        }

        public DataSet UI_MyTopicsView(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, string forumIds)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_MyTopicsView", portalId, moduleId, userId, rowIndex, maxRows, sort, forumIds);
        }

        public DataSet UI_MostLiked(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, int timeFrame, string forumIds)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_MostLiked", portalId, moduleId, userId, rowIndex, maxRows, sort, timeFrame, forumIds);
        }

        public DataSet UI_MostReplies(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, int timeFrame, string forumIds)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_MostReplies", portalId, moduleId, userId, rowIndex, maxRows, sort, timeFrame, forumIds);
        }

        public DataSet UI_Announcements(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, string forumIds)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_Announcements", portalId, moduleId, userId, rowIndex, maxRows, sort, forumIds);
        }

        public DataSet UI_Unresolved(int portalId, int moduleId, int userId, int rowIndex, int maxRows, string sort, string forumIds)
        {
            return SqlHelper.ExecuteDataset(this.connectionString, this.dbPrefix + "UI_Unresolved", portalId, moduleId, userId, rowIndex, maxRows, sort, forumIds);
        }
        #endregion

        #region TagCloud
        public IDataReader TagCloud_Get(int PortalId, int ModuleId, string ForumIds, int Rows)
        {
            return SqlHelper.ExecuteReader(this.connectionString, this.dbPrefix + "UI_TagCloud", PortalId, ModuleId, ForumIds, Rows);
        }

        #endregion
        #region Tags
        public int Tag_GetIdByName(int PortalId, int ModuleId, string TagName, bool IsCategory)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(this.connectionString, this.dbPrefix + "Tags_GetByName", PortalId, ModuleId, TagName.Replace("-", " ").ToLowerInvariant(), IsCategory));
        }

        #endregion
        #region TopMembers
        public IDataReader TopMembers_Get(int PortalId, int Rows)
        {
            return SqlHelper.ExecuteReader(this.connectionString, this.dbPrefix + "UI_TopMembers", PortalId, Rows);
        }

        #endregion
        #region CustomURLS
        public Dictionary<string, string> GetPrefixes(int PortalId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            using (IDataReader dr = SqlHelper.ExecuteReader(this.connectionString, this.dbPrefix + "Forums_GetPrefixes", PortalId))
            {
                while (dr.Read())
                {
                    string prefix = dr["PrefixURL"].ToString();
                    string tabid = dr["TabId"].ToString();
                    string forumid = dr["ForumId"].ToString();
                    string moduleId = dr["ModuleId"].ToString();
                    string archived = dr["Archived"].ToString();
                    string forumgroupId = dr["ForumGroupId"].ToString();
                    string groupPrefix = dr["GroupPrefixURL"].ToString();
                    if (!string.IsNullOrEmpty(groupPrefix))
                    {
                        prefix = groupPrefix + "/" + prefix;
                    }

                    dict.Add(prefix, tabid + "|" + forumid + "|" + moduleId + "|" + archived + "|" + forumgroupId + "|" + groupPrefix);
                }

                dr.Close();
            }

            return dict;
        }

        public string GetUrl(int ModuleId, int ForumGroupId, int ForumId, int TopicId, int UserId, int ContentId)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(this.connectionString, this.dbPrefix + "Util_GetUrl", ModuleId, ForumGroupId, ForumId, TopicId, UserId, ContentId));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public IDataReader FindByURL(int PortalId, string URL)
        {
            return SqlHelper.ExecuteReader(this.connectionString, this.dbPrefix + "FindByURL", PortalId, URL);
        }

        public IDataReader URLSearch(int PortalId, string URL)
        {
            return SqlHelper.ExecuteReader(this.connectionString, this.dbPrefix + "URL_Search", PortalId, URL);
        }

        public void ArchiveURL(int PortalId, int ForumGroupId, int ForumId, int TopicId, string URL)
        {
            SqlHelper.ExecuteNonQuery(this.connectionString, this.dbPrefix + "URL_Archive", PortalId, ForumGroupId, ForumId, TopicId, URL);
        }

        public bool CheckForumURL(int PortalId, int ModuleId, string VanityName, int ForumId, int ForumGroupId)
        {
            try
            {
                SettingsInfo _mainSettings = SettingsBase.GetModuleSettings(ModuleId);
                DotNetNuke.Modules.ActiveForums.Entities.ForumGroupInfo fg = new DotNetNuke.Modules.ActiveForums.Controllers.ForumGroupController().GetById(ForumGroupId, ModuleId);
                if (!string.IsNullOrEmpty(fg.PrefixURL))
                {
                    VanityName = fg.PrefixURL + "/" + VanityName;
                }

                if (!string.IsNullOrEmpty(_mainSettings.PrefixURLBase))
                {
                    VanityName = _mainSettings.PrefixURLBase + "/" + VanityName;
                }

                int tmpForumId = -1;
                tmpForumId = Convert.ToInt32(SqlHelper.ExecuteScalar(this.connectionString, this.dbPrefix + "URL_CheckForumVanity", PortalId, VanityName));
                if (tmpForumId > 0 && ForumId == -1)
                {
                    return false;
                }
                else if (tmpForumId == ForumId && ForumId > 0)
                {
                    return true;
                }
                else if (tmpForumId <= 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;

            }

            return false;
        }

        public bool CheckGroupURL(int PortalId, int ModuleId, string VanityName, int ForumGroupId)
        {
            try
            {
                SettingsInfo _mainSettings = SettingsBase.GetModuleSettings(ModuleId);
                if (!string.IsNullOrEmpty(_mainSettings.PrefixURLBase))
                {
                    VanityName = _mainSettings.PrefixURLBase + "/" + VanityName;
                }

                int tmpForumGroupId = -1;
                tmpForumGroupId = Convert.ToInt32(SqlHelper.ExecuteScalar(this.connectionString, this.dbPrefix + "URL_CheckGroupVanity", PortalId, VanityName));
                if (tmpForumGroupId > 0 && ForumGroupId == -1)
                {
                    return false;
                }
                else if (tmpForumGroupId == ForumGroupId && ForumGroupId > 0)
                {
                    return true;
                }
                else if (tmpForumGroupId <= 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;

            }

            return false;
        }

        #endregion
    }
}
