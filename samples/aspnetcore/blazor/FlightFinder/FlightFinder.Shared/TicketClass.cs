using System;

namespace FlightFinder.Shared
{
    public enum TicketClass : int
    {
        Economy = 0,
        PremiumEconomy = 1,
        Business = 2,
        First = 3,
    }

    public static class TicketClassExtensions
    {
        public static string ToDisplayString(this TicketClass ticketClass)
        {
            switch (ticketClass)
            {
                case TicketClass.Economy: return "Economy";
                case TicketClass.PremiumEconomy: return "Premium Economy";
                case TicketClass.Business: return "Business";
                case TicketClass.First: return "First";
                default: throw new ArgumentException("Unknown ticket class: " + ticketClass.ToString());
            }
        }
    }

}
