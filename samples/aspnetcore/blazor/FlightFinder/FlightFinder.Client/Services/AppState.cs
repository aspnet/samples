using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FlightFinder.Shared;
using Microsoft.AspNetCore.Blazor;

namespace FlightFinder.Client.Services
{
    public class AppState
    {
        // Actual state
        public IReadOnlyList<FlightItinerary> FlightSearchResults { get; private set; }
        public IReadOnlyList<CarRentalItinerary> RentalCarSearchResults { get; private set; }

        public bool SearchInProgress { get; private set; }

        private readonly List<object> shortlist = new List<object>();
        public IReadOnlyList<object> Shortlist => shortlist;

        // Lets components receive change notifications
        // Could have whatever granularity you want (more events, hierarchy...)
        public event Action OnChange;

        // Receive 'http' instance from DI
        private readonly HttpClient http;
        public AppState(HttpClient httpInstance)
        {
            http = httpInstance;
        }

        public async Task SearchFlights(FlightSearchCriteria criteria)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            FlightSearchResults = await http.PostJsonAsync<FlightItinerary[]>("/api/flightsearch", criteria);
            SearchInProgress = false;
            NotifyStateChanged();
        }

        public async Task SearchRentalCars(CarRentalSearchCriteria criteria)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            RentalCarSearchResults = await http.PostJsonAsync<CarRentalItinerary[]>("/api/carrentalsearch", criteria);
            SearchInProgress = false;
            NotifyStateChanged();
        }

        public void AddToShortlist(FlightItinerary itinerary)
        {
            shortlist.Add(itinerary);
            NotifyStateChanged();
        }

        public void AddToShortlist(CarRentalItinerary itinerary)
        {
            shortlist.Add(itinerary);
            NotifyStateChanged();
        }

        public void RemoveFromShortlist(object itinerary)
        {
            shortlist.Remove(itinerary);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
