using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Enums
{
    public class LearningProgramType
    {
        public const string Entry = "Entry Development Training Program";
        public const string Mandatory = "Mandatory Training Program";
        public const string Thematic = "Thematic Training Program";
        public const string Instructor = "Instructor Development Program";
        public const int EntryId = 1;
        public const int MandatoryId = 2;
        public const int ThematicId = 3;
        public const int InstructorId = 4;
    }

    public class LearningType
    {
        public const string Online = "Online";
        public const string Offline = "Offline";
        public const string Both = "Online/Offline";
        public const int OnlineId = 1;
        public const int OfflineId = 2;
        public const int BothId = 3;
    }

    public class LearningMaterialType
    {
        public const string Podcast = "Podcast";
        public const string Video = "Video Learning";
        public const string Journal = "Journal";
        public const string Game = "Game";
        public const int PodcastId = 1;
        public const int VideoId = 2;
        public const int JournalId = 3;
        public const int GameId = 4;
    }

    public class LearningPriceType
    {
        public const string Free = "Free";
        public const string Paid = "Paid";
        public const int FreeId = 1;
        public const int PaidId = 2;
    }

    public class NewsTypeCategory
    {
        public const string Internal = "True";
        public const string External = "False";
        public const int InternalId = 1;
        public const int ExternalId = 0;
    }
}
