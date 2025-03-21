<%@ Register TagPrefix="af" Assembly="DotNetNuke.Modules.ActiveForums" Namespace="DotNetNuke.Modules.ActiveForums.Controls"%>
	<div class="dcf-forum-view">
[BREADCRUMB]
[GROUPSECTION]
		<div class="dcf-forums">
			<div class="dcf-group-title-wrap">
				<h3 class="dcf-group-title">[RESX:Group]: [FORUMGROUP:GROUPLINK|
					<a href="{0}" class="dcf-forumgroup-link">[FORUMGROUP:GROUPNAME]</a>]
				</h3>
				<span class="dcf-group-collapse">[FORUMGROUP:GROUPCOLLAPSE]</span>
			</div>

        [GROUP]
			<div class="dcf-forums-group">
				<table class="dcf-table dcf-table-100">
					<thead>
						<tr class="dcf-table-head-row">
							<th scope="col" class="dcf-th dcf-col-text">
								<div class="dcf-th-text">[RESX:FORUMHEADER]</div>
							</th>
							<th scope="col" class="dcf-th dcf-col-topics">
								<div class="dcf-icon-text">
									<i class="fa fa-files-o"></i>
									<span class="dcf-link-text">[RESX:TOPICSHEADER]</span>
								</div>
							</th>
							<th scope="col" class="dcf-th dcf-col-replies">
								<div class="dcf-icon-text">
									<i class="fa fa-reply"></i>
									<span class="dcf-link-text">[RESX:REPLIESHEADER]</span>
								</div>
							</th>
							<th scope="col" class="dcf-th dcf-col-subscribers">
								<div class="dcf-icon-text">
									<i class="fa fa-envelope-o"></i>
									<span class="dcf-link-text">[RESX:SUBSCRIBERS]</span>
								</div>
							</th>
							<th scope="col" class="dcf-th dcf-col-last-post">
								<div class="dcf-icon-text">
									<i class="fa fa-file-o"></i>
									<span class="dcf-link-text">[RESX:LASTPOSTHEADER]</span>
								</div>
							</th>
						</tr>
					</thead>
				[FORUMS]
					<tbody>
						<tr class="dcf-table-body-row dcf-main-forums">
							<td class="dcf-col dcf-col-text">
								<div class="dcf-col-text-inner">
							[FORUM:FORUMICONCSS|
									<div class="dcf-forum-icon" style="height: 30px; margin-right: 10px;">
										<i class="fa {0} fa-2x"></i>
									</div>
                            ]
									<div class="dcf-forum-title-text">
										<h4 class="dcf-forum-title">
                                        [FORUM:FORUMLINK|
											<a href="{0}" class="dcf-forum-link">[FORUM:FORUMNAME]</a>
                                        ]
										</h4>
										<div class="dcf-forum-description">[FORUM:FORUMDESCRIPTION]</div>
									</div>
								</div>
							</td>
							<td class="dcf-col dcf-col-topics">[FORUM:TOTALTOPICS]</td>
							<td class="dcf-col dcf-col-replies">[FORUM:TOTALREPLIES]</td>
							<td class="dcf-col dcf-col-subscribers">[FORUM:SUBSCRIBERCOUNT]</td>
							<td class="dcf-col dcf-col-last-post">
								<div class="dcf-last-post">
							[FORUM:LASTPOSTSUBJECT:25]
									<div class="dcf-last-profile">
                            [FORUM:LASTPOSTAUTHORDISPLAYNAMELINK|
                            [RESX:BY]
										<i class="fa fa-user fa-fw fa-blue"></i>&nbsp;
										<a href="{0}" class="dcf-profile-link" rel="nofollow">[FORUM:LASTPOSTAUTHORDISPLAYNAME]</a>|
                            [RESX:BY]
										<i class="fa fa-user fa-fw fa-blue"></i>[FORUM:LASTPOSTAUTHORDISPLAYNAME]
                            ]
									</div>
									<div class="dcf-last-date">
                            [FORUM:LASTPOSTDATE]
							</div>
								</div>
							</td>
						</tr>

				[SUBFORUMS]
						<tr class="dcf-table-body-row dcf-sub-forums">
							<td class="dcf-col" colspan="5">
								<h5 class="dcf-sub-forum-title">
									<i class="fa fa-code-fork"></i> [RESX:Child] [RESX:FORUMS]
								</h5>
							</td>
						</tr>
						<tr class="dcf-table-body-row dcf-sub-forums">
							<td class="dcf-col dcf-col-text">
								<span class="aftopictitle">[FORUM:FORUMLINK|
									<a href="{0}" class="dcf-forum-link">[FORUM:FORUMNAME]</a>]
								</span>
								<span class="aftopictitle">([RESX:Child] [RESX:of] [FORUM:PARENTFORUMNAME])</span>
								<span class="aftopicsubtitle">[FORUM:FORUMDESCRIPTION]</span>
							</td>
							<td class="dcf-col dcf-col-topics">[FORUM:TOTALTOPICS]</td>
							<td class="dcf-col dcf-col-replies">[FORUM:TOTALREPLIES]</td>
							<td class="dcf-col dcf-col-subscribers">[FORUM:SUBSCRIBERCOUNT]</td>
							<td class="dcf-col dcf-col-last-post">
								<div class="dcf-last-post">
							[FORUM:LASTPOSTSUBJECT:25]
									<div class="dcf-last-profile">
                            [FORUM:LASTPOSTAUTHORDISPLAYNAMELINK|
                            [RESX:BY]
										<i class="fa fa-user fa-fw fa-blue"></i>&nbsp;
										<a href="{0}" class="dcf-profile-link" rel="nofollow">[FORUM:LASTPOSTAUTHORDISPLAYNAME]</a>|
                            [RESX:BY]
										<i class="fa fa-user fa-fw fa-blue"></i>[FORUM:LASTPOSTAUTHORDISPLAYNAME]
                            ]
									</div>
									<div class="dcf-last-date">
                            [FORUM:LASTPOSTDATE]
							</div>
								</div>
							</td>
						</tr>
				[/SUBFORUMS]
					</tbody>
			[/FORUMS]
				</table>
			</div>
	[/GROUP]
		</div>
[/GROUPSECTION]
		<!-- Who's online -->
[WHOSONLINE]
		<!-- Jump To -->
		<div style="text-align:right;">[JUMPTO]</div>
	</div>