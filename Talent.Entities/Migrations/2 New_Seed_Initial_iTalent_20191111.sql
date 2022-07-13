INSERT INTO CompetencyTypes(CompetencyTypeName) 
VALUES
	('Hard'),
	('Soft')

GO

INSERT INTO EBadges(EBadgeName) 
VALUES
	('Bronze'),
	('Silver'),
	('Gold')

GO

INSERT INTO EvaluationTypes(EvaluationTypeName) 
VALUES
	('Knowledge'),
	('Skill'),
	('Behavior')

GO

INSERT INTO ProgramTypes(ProgramTypeName) 
VALUES
	('Entry Development Training Program'),
	('Mandatory Training Program'),
	('Thematic Training Program'),
	('Instructor Development Program')

GO

INSERT INTO Levels(LevelName) 
VALUES
	('On Boarding'),
	('Development Program'),
	('Instructor Development Program'),
	('Instructor Retention Program'),
	('Level 1'),
	('Level 2'),
	('Level 3'),
	('Level 4'),
	('Level 5'),
	('N/A')

GO

INSERT INTO LearningTypes(LearningName) 
VALUES
	('Online'),
	('Offline'),
	('Online/Offline')

GO

INSERT INTO CourseCategories(CourseCategoryName) 
VALUES
	('Position'),
	('Product'),
	('Non-Product'),
	('Others')

GO

INSERT INTO ApprovalStatus(ApprovalName) VALUES
	('Approved'),
	('Waiting Approval'),
	('Draft'),
	('Rejected')

GO

INSERT INTO MaterialTypes(MaterialTypeName) 
VALUES
	('Podcast'),
	('Video Learning'),
	('Journal'),
	('Game')

GO

INSERT INTO TrainingTypes(TrainingTypeName) 
VALUES
	('Pre'),
	('During'),
	('Post'),
	('Total')
GO

INSERT INTO PointTypes(PointTypeName) 
VALUES
	('Learning Time'), 
	('Teaching Time'), 
	('Coaching Time'), 
	('NPS'), 
	('Task')

GO

INSERT INTO QuestionTypes(QuestionTypeName) 
VALUES
	('True/False'),
	('Matching'),
	('Sequence'),
	('Tebak Gambar'),
	('Hot Spot'),
	('Short Answer'),
	('Open Question/Essay'),
	('Checklist'),
	('Rating'),
	('Multiple Choice'),
	('File Upload'),
	('Matrix')

GO

INSERT INTO RewardTypes([Name])
VALUES
	('Modules'),
	('Coach'),
	('Seminar'),
	('Merchandise'),
	('Voucher'),
	('Trip'),
	('Training Discount'),
	('Learning & Training Tools')
GO

INSERT INTO BannerTypes
VALUES 
	(1, 'Regular'),
	(2, 'Specific')
GO

INSERT INTO ITALENT_UAT.dbo.MobilePages (Name,Route,IsForBanner) VALUES 
('main','/',0)
,('search','/search',0)
,('searchSort','/search/sort',0)
,('searchFilter','/search/filter',0)
,('searchFilterMore','/search/filter/more',0)
,('login','/login',0)
,('loginForm','/login/form',0)
,('welcome','/welcome',0)
,('tutorial','/tutorial',0)
,('guide','/guide',1)
,('profile','/profile',0)
,('eventHome','/event/home',0)
,('eventDetail','/event/detail',1)
,('eventInvite','/event/invite',0)
,('eventAdd','/event/add',0)
,('coach','/coach',1)
,('bookCoach','/coach/book',0)
,('learningDetail','/course/overview',1)
,('learningHome','/course/home',0)
,('learningTestimony','/course/testimony',0)
,('learningAssign','/course/assign',0)
,('module','/module',0)
,('badgeDetail','/badge/detail',0)
,('testPre','/test/pre',0)
,('testStart','/test/start',0)
,('testFinish','/test/finish',0)
,('testFeedback','/test/feedback',0)
,('team','/team',0)
,('teamFilter','/team/filter',0)
,('teamFilterMore','/team/filter/more',0)
,('teamAssign','/team/assign',0)
,('teamMapping','/team/mapping',0)
,('userTeam','/team/user',0)
,('point','/point',0)
,('rankFilter','/rank/filter',0)
,('rankFilterMore','/rank/filter/more',0)
,('pointSearch','/point/search',0)
,('pointHistory','/point/history',0)
,('pointSort','/point/sort',0)
,('pointRedeem','/point/redeem',1)
,('inbox','/inbox',0)
,('personalMapping','/profile/personal/mapping',0)
,('newsHome','/news/home',0)
,('newsDetail','/news/detail',1)
,('addInterestAspiration','/profile/add/interest/aspiration',0)
,('certificateDetail','/profile/certificate/upload',0)
,('certificateUpload','/profile/certificate/upload',0)
,('learningHistory','/profile/learning/history',0)
,('learningHistoryDetail','/profile/learning/history/detail',0)
,('learningHistoryFilter','/profile/learning/history/filter',0)
,('learningHistoryFilterMore','/profile/learning/history/filter/more',0)
,('trainingList','/profile/training/list',0)
,('traineeList','/profile/training/list/trainee/list',0)
,('evaluateTrainee','/profile/training/list/trainee/list/evaluate',0)
,('searchPosition','/profile/team/mapping/search/position',0)
,('timetable','/timetable',0)
,('timetableSort','/timetable/sort',0)
,('timetableFilter','/timetable/filter',0)
,('timetableFilterMore','/timetable/filter/more',0)
,('profileSchedule','/profile/schedule',0)
,('profileEdit','/profile/edit',0)
,('insightHome','/insight/home',0)
,('insightDetail','/insight/detail',0)
,('insightFinish','/insight/finish',0)
,('complaint','/complaint',0)


INSERT INTO GuideTypes([Name]) 
VALUES
('Get Started'),
('Find Your Path'),
('Build Your Expertise'),
('Grow Your Career'),
('Connect With People'),
('Reward & Recognition'),
('Tutorial')


INSERT INTO RewardPointTypes
VALUES 
('Learning Points'), 
('Teaching Points'), 
('Total Points')

INSERT INTO Menus (MenuId,Name) VALUES 
('DSB','Dashboard')
,('MD','Master Data')
,('RPRT','Report')
,('STP','Setup')
,('UM','User Management')


INSERT INTO [Pages](PageId,MenuId,Name,NeedApproval) VALUES 
('AC','STP','Approval Content',0)
,('AM','STP','Approval Mapping',0)
,('ASM','MD','Assessment',0)
,('BNR','MD','Banner',1)
,('CAL','RPRT','Calendar',0)
,('CCH','UM','Coach',0)
,('COMP','MD','Competency',0)
,('COMPM','MD','Competency Mapping',0)
,('CRS','MD','Course',1)
,('DS','MD','Digital Signature',0)
,('EC','MD','Event Category',0)
,('EVN','MD','Event',1)
,('GDE','MD','Guide',1)
,('HB','MD','Hobby',0)
,('IN','RPRT','Inbox',0)
,('KA','MD','Key Action',0)
,('MDL','MD','Module',1)
,('MNG','DSB','Management',0)
,('NWS','MD','News',1)
,('OPR','DSB','Operation',0)
,('PM','UM','Position Mapping',0)
,('RTG','STP','Release Training',1)
,('RWRD','MD','Reward',0)
,('STC','STP','Setup Top 5 Course',0)
,('STCRS','STP','Setup Course',1)
,('STL','STP','Setup Learning',0)
,('STM','STP','Setup Module',1)
,('STP','STP','Setup Pop Quiz',1)
,('STPT','STP','Setup Time Point',0)
,('STSL','STP','Setup TSL',0)
,('SVY','MD','Survey',1)
,('TPC','MD','Topic',0)
,('TSK','STP','Task',1)
,('UPS','UM','User Privilege Settings',0)
,('UR','UM','User Role',0)
;


INSERT INTO NewsCategories (Name) VALUES 
('Market Challenge')
,('New Product Information')
,('Competitor Movement')
,('New Technology')
,('Best Practice Sharing')
,('Lifestyle')
,('Announcement')


INSERT INTO TrainingServiceLevels ([Name]) VALUES 
('Sales')
,('After Sales')
,('Total')

INSERT INTO PointTransactionTypes (PointTransactionTypeId,Name) VALUES 
(1,'Income')
,(2,'Outcome')

INSERT INTO MobileInboxTypes (MobileInboxTypeId, Name) VALUES 
(1, 'Resign')
,(2, 'Team Member Rotation Request')
,(3, 'Rotation Accepted')
,(4, 'Rotation Rejected')
,(5, 'Team Member Request')
,(6, 'Remedial')
GO

INSERT INTO EmailTemplates (EmailTemplateId,Title,Template) VALUES 
(1,'Coach Booking','Coach Booking
<br><br>
Kepada <coachname>, 
<br>
<table>
<tr>
<td>Saya yang bernama</td>
<td><requestername> ingin mem-booking jadwal anda pada,</td>
</tr>
<tr>
<td>hari</td>
<td><schaduledate></td>
</tr>
<tr>
<td>Waktu</td>
<td><schaduletime></td>
</tr>
</table>
<br>
Terima kasih.')
,(2,'Coach Booking','<div>
    <p>Kepada {{CoachName}},</p>

    <p>
        Saya yang bernama {{RequesterName}} ingin mem-booking jadwal anda pada,<br/>
        hari : {{ScheduleDate}} <br />
        waktu : {{ScheduleTime}}
    </p>

    <p>Catatan : {{Message}}</p>

    <p>Terima kasih.</p>
</div>')
GO