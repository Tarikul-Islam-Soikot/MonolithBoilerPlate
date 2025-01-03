﻿namespace MonolithBoilerPlate.Common
{
    public static class ApplicationExceptionMessage
    {
        #region Common Message
        public const string NullObjectReference = "Null object reference error occured.";
        public const string SavedSuccessfully = "Saved successfully.";
        public const string UpdatedSuccessfully = "Updated successfully.";
        public const string DeletedSuccessfully = "Deleted successfully.";
        public const string SaveFailed = "Save failed.";
        public const string UpdateFailed = "Update failed.";
        public const string DeleteFailed = "Delete failed.";
        public const string DeleteOperationNotValid = "Delete operation not valid as this entry is used in {0}.";
        public const string NotFound = "{0} not found.";
        public const string UnAuthorized = "Authentication fail. {0}";
        public const string Required = "{0} required.";
        public const string DateValidation = "{0} can't be greater than {1}.";
        public const string DateRequiredValidation = "Minimum one {0} & {1} is required.";
        public const string QueueEmpty = "No pending data found for entry.";
        public const string AlreadyExists = "{0} already exists.";
        public const string PleaseEntryUniqueName = "Please enter a unique {0} name.";
        public const string NotValid = "{0} is not valid";
        public const string NotAvailable = "{0} not available";
        public const string InvalidValue = "Invalid value.";
        public const string NotExist = "{0} not exist";
        public const string ContainsNonActive = "{0} contains non-active {1}.";
        public const string ImportedSuccessfully = "{0}({1} data['s]) imported successfully.";
        public const string AlreadyAssigned = "{0} already assigned";
        public const string SomethingWentWrong = "Something went wrong";
        public const string DateOverlapped = "Date overlapped!";
        public const string SessionExpired = "Session Expired";
        public const string OtpValidationFailed = "Otp Validation Failed";
        public const string OtpTryLimitExceeded = "Otp Try Limit Exceeded";
        public const string OtpValidationSuccessful = "Otp Validation Successful";

        #endregion
    }
}
