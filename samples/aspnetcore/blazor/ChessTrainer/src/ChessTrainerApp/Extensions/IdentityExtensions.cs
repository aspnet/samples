using System.Security.Claims;

namespace MjrChess.Trainer
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// Helper method for getting user ID from a ClaimsPrincipal.
        /// </summary>
        /// <param name="principal">A ClaimsPrincipal representing a user.</param>
        /// <returns>The user's ID or null if no name ID claim exists.</returns>
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
