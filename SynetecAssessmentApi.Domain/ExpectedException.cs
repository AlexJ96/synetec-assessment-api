using System;

namespace SynetecAssessmentApi.Domain
{
    public class ExpectedException : Exception
    {
        public Int32 ErrorCode { get; set; }
        public String ErrorMessage { get; set; }

        public ExpectedException(ExpectedExceptionDefinition definition) : base("An expected error has occured.")
        {
            ErrorCode = definition.ErrorCode;
            ErrorMessage = definition.ErrorMessage;
        }

        
        public override String ToString()
        {
            return $"Expected Error Occurred - Error Code: {ErrorCode} - Error Message: {ErrorMessage}";
        }

        public class ExpectedExceptionDefinition
        {
            public Int32 ErrorCode { get; set; }
            public String ErrorMessage { get; set; }

            public ExpectedExceptionDefinition(Int32 errorCode, String errorMessage) => (ErrorCode, ErrorMessage) = (errorCode, errorMessage);
        }

        public static readonly ExpectedExceptionDefinition EmployeeNotFound = new ExpectedExceptionDefinition(1000, "No employee was not found with the ID provided.");

        public static readonly ExpectedExceptionDefinition BonusPoolNotProvided = new ExpectedExceptionDefinition(1001, "The bonus pool was not provided.");
    }

}
