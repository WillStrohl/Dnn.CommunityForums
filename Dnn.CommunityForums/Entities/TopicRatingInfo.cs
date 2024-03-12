﻿//
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
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//
using System; 
using DotNetNuke.ComponentModel.DataAnnotations;
using System.Web.Caching;
namespace DotNetNuke.Modules.ActiveForums.Entities
{
    [TableName("activeforums_Topics_Ratings")]
    [PrimaryKey("RatingId", AutoIncrement = true)]
    [Scope("TopicId")]
    [Cacheable("activeforums_Topics_Ratings", CacheItemPriority.Normal)]
    public class TopicRatingInfo
    {
        public int RatingId { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public bool Helpful { get; set; }
        public string Comments { get; set; }
        public string IPAddress { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}