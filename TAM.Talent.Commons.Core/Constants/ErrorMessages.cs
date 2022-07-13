namespace TAM.Talent.Commons.Core.Constants
{
    /// <summary>
    /// This class used for storing error message constant values.
    /// </summary>
    public class ErrorMessages
    {
        /// <summary>
        /// This constant value is used for telling the users that data was not found in the database.
        /// </summary>
        public const string DataNotFound = " was not found";

        /// <summary>
        /// Error Message for token not valid.
        /// </summary>
        public const string TokenNotValid = "Token issue or audience or expiration are not valid.";

        /// <summary>
        /// Error Message for token header not found.
        /// </summary>
        public const string TokenHeaderNotFound = "Request header JSON Web Token was not found!";
    }
}
