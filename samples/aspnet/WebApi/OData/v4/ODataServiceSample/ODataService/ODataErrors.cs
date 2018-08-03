using Microsoft.OData.Core;

namespace ODataService
{
    /// <summary>
    /// A set of useful correctly formatted OData errors.
    /// </summary>
    public static class ODataErrors
    {
        public static ODataError EntityNotFound(string entityName)
        {
            return new ODataError()
            {
                Message = string.Format("Cannot find {0}", entityName), 
                ErrorCode = "Entity Not Found"
            };
        }

        public static ODataError DeletingLinkNotSupported(string navigation)
        {
            return new ODataError()
            {
                Message = string.Format("Deleting a '{0}' link is not supported.", navigation), 
                ErrorCode = "Deleting link failed."
            };
        }

        public static ODataError CreatingLinkNotSupported(string navigation)
        {
            return new ODataError()
            {
                Message = string.Format("Creating a '{0}' link is not supported.", navigation), 
                ErrorCode = "Creating link failed."
            };
        }
    }
}
