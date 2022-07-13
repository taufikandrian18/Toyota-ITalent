using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Enums
{
    public class ResponseMessageEnum
    {
        public const string AlreadyExist = "Data Already Exist!";
        public const string SuccessAddSave = "Success to Add Data!";
        public const string SuccessEditSave = "Success to Edit Data!";
        public const string SuccessSave = "Success to save data into database.";
        public const string SuccessBaseString = "Success, ";
        public const string FailedSave = "Failed to save data into database.";
        public const string SuccessDelete = "Success to Remove Data!";
        public const string FailedDelete = "Failed to save delete into database.";
        public const string ErrorOnProcessing = "Failed, there's an error while processing.";
        public const string FailedBaseString = "Failed, ";
        public const string FailedTokenExipred = "Failed, Token has expired";
    }
}
