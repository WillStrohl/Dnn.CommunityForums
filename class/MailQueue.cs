//
// Community Forums
// Copyright (c) 2013-2021
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
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN doworkafaffdaafdfdfffdfdffd WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections.Generic;
using DotNetNuke.Services.Scheduling;
namespace DotNetNuke.Modules.ActiveForums.Queue
{
    [Obsolete("Deprecated in Community Forums. Scheduled removal in v9.0.0.0. Use DotNetNuke.Modules.ActiveForums.Controllers.MailQueue().")]
    public class Controller : DotNetNuke.Modules.ActiveForums.Controllers.MailQueueController
	{
        [Obsolete("Deprecated in Community Forums. Scheduled removal in v9.0.0.0. Use DotNetNuke.Modules.ActiveForums.Controllers.MailQueue.Add(int portalId, int moduleId, string EmailFrom, string EmailTo, string EmailSubject, string EmailBody, string EmailBodyPlainText, string EmailCC, string EmailBCC).")]
        public static void Add(string emailFrom, string emailTo, string emailSubject, string emailBody, string emailBodyPlainText, string emailCC, string emailBcc)
        {
            DotNetNuke.Modules.ActiveForums.Controllers.MailQueueController.Add(-1, -1, emailFrom, emailTo, emailSubject, emailBody, emailBodyPlainText, emailCC, emailBcc);
        }
        [Obsolete("Deprecated in Community Forums. Scheduled removal in v9.0.0.0. Use DotNetNuke.Modules.ActiveForums.Controllers.MailQueue.Add(int portalId, int moduleId, string EmailFrom, string EmailTo, string EmailSubject, string EmailBody, string EmailBodyPlainText, string EmailCC, string EmailBCC).")]
        public static void Add(int portalId, string emailFrom, string emailTo, string emailSubject, string emailBody, string emailBodyPlainText, string emailCC, string emailBcc)
        {
            DotNetNuke.Modules.ActiveForums.Controllers.MailQueueController.Add(-1, -1, emailFrom, emailTo, emailSubject, emailBody, emailBodyPlainText, emailCC, emailBcc);
        }
    }
	public class Scheduler : SchedulerClient
	{
		public Scheduler(ScheduleHistoryItem objScheduleHistoryItem)
		{
			ScheduleHistoryItem = objScheduleHistoryItem;
		}


		public override void DoWork()
		{
			try
			{
				var intQueueCount = ProcessQueue();
				ScheduleHistoryItem.Succeeded = true;
				ScheduleHistoryItem.AddLogNote(string.Concat("Processed ", intQueueCount, " messages"));
			}
			catch (Exception ex)
			{
				ScheduleHistoryItem.Succeeded = false;
				ScheduleHistoryItem.AddLogNote(string.Concat("Process Queue Failed. ", ex));
				Errored(ref ex);
				DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
			}
		}

		private static int ProcessQueue()
		{
			var intQueueCount = 0;
			try
            {
                DotNetNuke.Modules.ActiveForums.Controllers.MailQueueController.GetBatch().ForEach(m =>
                {
                    intQueueCount += 1;
                    var message = new DotNetNuke.Modules.ActiveForums.Entities.Message
                    {
                        PortalId = m.PortalId,
                        Subject = m.EmailSubject,
                        SendFrom = m.EmailFrom,
                        SendTo = m.EmailTo,
                        Body = m.EmailBody,
                        BodyText = m.EmailBodyPlainText,
                    };

                    var canDelete = DotNetNuke.Modules.ActiveForums.Controllers.MessageController.Send(message);
                    if (canDelete)
                    {
                        try
                        {
                            DotNetNuke.Modules.ActiveForums.Controllers.MailQueueController.Delete(m.Id);
                        }
                        catch (Exception ex)
                        {
                            DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                        }
                    }
                    else
                    {
                        intQueueCount = intQueueCount - 1;
                    }
                });
                return intQueueCount;
            }
            ////{
            ////	var dr = DataProvider.Instance().MailQueue_List();
            ////	while (dr.Read())
            ////	{
            ////		int Id = Convert.ToInt32(dr["Id"]);
            ////                 intQueueCount += 1;
            ////		var objEmail = new Message
            ////                 {
            ////                     PortalId = Convert.ToInt32(dr["PortalId"]),
            ////                     Subject = dr["EmailSubject"].ToString(),
            ////			SendFrom = dr["EmailFrom"].ToString(),
            ////			SendTo = dr["EmailTo"].ToString(),
            ////			Body = dr["EmailBody"].ToString(),
            ////			BodyText = dr["EmailBodyPlainText"].ToString(),
            ////		};

            ////		var canDelete = objEmail.SendMail();
            ////		if (canDelete)
            ////		{
            ////			try
            ////			{ 
            ////				DataProvider.Instance().MailQueue_Delete(Convert.ToInt32(dr["Id"]));
            ////			}
            ////			catch (Exception ex)
            ////			{
            ////				DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            ////			}
            ////		}
            ////		else
            ////		{
            ////			intQueueCount = intQueueCount - 1;
            ////		}
            ////	}
            ////	dr.Close();
            ////	dr.Dispose();

            ////	return intQueueCount;
            ////}
            catch (Exception ex)
			{
				DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
				return -1;
			}
		}
	}
    [Obsolete("Deprecated in Community Forums. Scheduled removal in v9.0.0.0. Use DotNetNuke.Modules.ActiveForums.Entities.Message.")]
    public class Message : DotNetNuke.Modules.ActiveForums.Entities.Message 
	{
        [Obsolete("Deprecated in Community Forums. Scheduled removal in v9.0.0.0. Use DotNetNuke.Modules.ActiveForums.Controllers.MessageController.Send().")]
        public bool SendMail()
		{ return DotNetNuke.Modules.ActiveForums.Controllers.MessageController.Send(this); }
	}
}