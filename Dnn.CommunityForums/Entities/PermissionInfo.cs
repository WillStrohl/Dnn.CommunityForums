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

namespace DotNetNuke.Modules.ActiveForums.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Web.Caching;

    using DotNetNuke.ComponentModel.DataAnnotations;

    [TableName("activeforums_Permissions")]
    [PrimaryKey("PermissionsId", AutoIncrement = true)]
    [Cacheable("activeforums_Permissions", CacheItemPriority.Low)]
    [Scope("ModuleId")]
    public class PermissionInfo
    {
        [IgnoreColumn] private string cacheKeyTemplate => CacheKeys.PermissionsInfo;

        public int PermissionsId { get; set; }

        public int ModuleId { get; set; }

        [ColumnName("CanView")]
        public string View { get; set; }

        [ColumnName("CanRead")]
        public string Read { get; set; }

        [ColumnName("CanCreate")]
        public string Create { get; set; }

        [ColumnName("CanReply")]
        public string Reply { get; set; }

        [ColumnName("CanEdit")]
        public string Edit { get; set; }

        [ColumnName("CanDelete")]
        public string Delete { get; set; }

        [ColumnName("CanLock")]
        public string Lock { get; set; }

        [ColumnName("CanPin")]
        public string Pin { get; set; }

        [ColumnName("CanAttach")]
        public string Attach { get; set; }

        [ColumnName("CanPoll")]
        public string Poll { get; set; }

        [ColumnName("CanBlock")]
        public string Block { get; set; }

        [ColumnName("CanTrust")]
        public string Trust { get; set; }

        [ColumnName("CanSubscribe")]
        public string Subscribe { get; set; }

        [ColumnName("CanAnnounce")]
        public string Announce { get; set; }

        [ColumnName("CanTag")]
        public string Tag { get; set; }

        [ColumnName("CanCategorize")]
        public string Categorize { get; set; }

        [ColumnName("CanPrioritize")]
        public string Prioritize { get; set; }

        [ColumnName("CanModerate")]
        public string Moderate { get; set; }

        [ColumnName("CanMove")]
        public string Move { get; set; }

        [ColumnName("CanSplit")]
        public string Split { get; set; }

        [ColumnName("CanBan")]
        public string Ban { get; set; }

        [IgnoreColumn]
        public ObjectType Type { get; set; }

        [IgnoreColumn]
        public string ObjectId { get; set; }

        [IgnoreColumn]
        public string ObjectName { get; set; }

        [Obsolete("Deprecated in Community Forums. Scheduled for removal in 10.00.00. Not Used.")]
        [IgnoreColumn]
        public int UserTrustLevel { get; set; }

        public bool EqualPermissions(PermissionInfo other)
        {
            return !(other is null) &&
                   this.View == other.View &&
                   this.Read == other.Read &&
                   this.Create == other.Create &&
                   this.Reply == other.Reply &&
                   this.Edit == other.Edit &&
                   this.Delete == other.Delete &&
                   this.Lock == other.Lock &&
                   this.Pin == other.Pin &&
                   this.Attach == other.Attach &&
                   this.Poll == other.Poll &&
                   this.Block == other.Block &&
                   this.Trust == other.Trust &&
                   this.Subscribe == other.Subscribe &&
                   this.Announce == other.Announce &&
                   this.Tag == other.Tag &&
                   this.Categorize == other.Categorize &&
                   this.Prioritize == other.Prioritize &&
                   this.Moderate == other.Moderate &&
                   this.Move == other.Move &&
                   this.Split == other.Split &&
                   this.Ban == other.Ban;
        }

        internal string GetCacheKey() => string.Format(this.cacheKeyTemplate, this.ModuleId, this.PermissionsId);
    }
}
