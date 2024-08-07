//
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
using System;
using System.Web.UI;
namespace DotNetNuke.Modules.ActiveForums
{
    public partial class af_modban : ForumBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.LocalResourceFile = "~/DesktopModules/ActiveForums/App_LocalResources/af_modban.ascx.resx"; 
            btnBan.Click += new System.EventHandler(btnBan_Click);
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect(Utilities.NavigateURL(TabId));
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            base.Render(htmlWriter);
            string html = stringWriter.ToString();
            html = html.Replace("[AF:LINK:FORUMMAIN]", "<a href=\"" + Utilities.NavigateURL(TabId) + "\">[RESX:FORUMS]</a>");
            html = html.Replace("[AF:LINK:FORUMGROUP]", "<a href=\"" + Utilities.NavigateURL(TabId, "", ParamKeys.GroupId + "=" + ForumInfo.ForumGroupId) + "\">" + ForumInfo.GroupName + "</a>");
            html = html.Replace("[AF:LINK:FORUMNAME]", "<a href=\"" + Utilities.NavigateURL(TabId, "", new string[] { ParamKeys.ForumId + "=" + ForumId, ParamKeys.ViewType + "=" + Views.Topics }) + "\">" + ForumInfo.ForumName + "</a>");
            html = Utilities.LocalizeControl(html);
            writer.Write(html);
        } 
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(Utilities.NavigateURL(TabId, "", new string[] { ParamKeys.ForumId + "=" + ForumId, ParamKeys.ViewType + "=" + Views.Topic, ParamKeys.TopicId + "=" + TopicId }));
        }
        private void btnBan_Click(object sender, System.EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect(Utilities.NavigateURL(TabId, "", new string[] { ParamKeys.ForumId + "=" + ForumId, ParamKeys.ViewType + "=" + Views.Topic, ParamKeys.TopicId + "=" + TopicId }));
            }
            else
            {
                DotNetNuke.Modules.ActiveForums.Controllers.UserController.BanUser(PortalId:PortalId, ModuleId:ForumModuleId, ModuleTitle: ModuleContext.Configuration.ModuleTitle, TabId:TabId, ForumId:ForumId, TopicId: TopicId, ReplyId:ReplyId, BannedBy: UserInfo, AuthorId: AuthorId);
                Response.Redirect(Utilities.NavigateURL(TabId, "", new string[] { ParamKeys.ForumId + "=" + ForumId, (ReplyId > 0 ? ParamKeys.TopicId + "=" + TopicId : string.Empty), ParamKeys.ViewType + "=confirmaction", ParamKeys.ConfirmActionId + "=" + ConfirmActions.UserBanned + (SocialGroupId > 0 ? "&" + Literals.GroupId + "=" + SocialGroupId : string.Empty) }));
            }
        }
    }
}
