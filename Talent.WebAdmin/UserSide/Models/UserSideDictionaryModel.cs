using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideDictionaryModel
    {
        public Guid DictionaryId { get; set; }
        public Guid BlobId { get; set; }
        public string ImageUrl { get; set; }
        public string FileType { get; set; }
        public string DictionaryName { get; set; }
        public bool DictionaryStatus { get; set; }
        public bool? IsFavorite { get; set; }
        public string DictionaryArti { get; set; }
        public string DictionaryManfaat { get; set; }
        public string DictionaryFakta { get; set; }
        public string DictionaryBasicOperation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UserSideDictionaryNameListView
    {
        public Guid DictionaryId { get; set; }
        public string DictionaryName { get; set; }
        public bool? IsFavorite { get; set; }
    }

    public class UserSideDictionaryIsFavoriteListView
    {
        public Guid DictionaryId { get; set; }
        public string DictionaryName { get; set; }
        public bool? IsFavorite { get; set; }
        public string EmployeeId { get; set; }
    }

    public class UserSideDictionaryPaginateListView
    {
        public List<UserSideDictionaryNameListView> DictionaryList { get; set; }
        public List<UserSideDictionaryIsFavoriteListView> DictionaryIsFavoriteList { get; set; }
    }
}
