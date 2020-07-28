using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FlightFinder.Client.Components;
using FlightFinder.Shared;
using Microsoft.AspNetCore.Components;

namespace FlightFinder.Client.Services
{
    public class AppState
    {
        // Actual state
        public IReadOnlyList<Itinerary> SearchResults { get; private set; }

        // Flag to indicate search is in progress
        public bool SearchInProgress { get; private set; }

        // shortlist (internal store)
        private readonly List<Itinerary> shortlist = new List<Itinerary>();

        // expose shortlist as a readonly list
        public IReadOnlyList<Itinerary> Shortlist => shortlist;

        // Lets components receive change notifications
        // Could have whatever granularity you want (more events, hierarchy...)
        public event Action OnChange;

        // Receive 'http' instance from DI
        private readonly HttpClient http;

        // constructor with DI
        public AppState(HttpClient httpInstance)
        {
            http = httpInstance;
        }

        // search for flights using Web API
        public async Task Search(SearchCriteria criteria)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            var response = await http.PostAsJsonAsync("/api/flightsearch", criteria);
            SearchResults = await response.Content.ReadFromJsonAsync<Itinerary[]>();
            SearchInProgress = false;
            NotifyStateChanged();
        }

        public void AddToShortlist(Itinerary itinerary)
        {
            shortlist.Add(itinerary);
            NotifyStateChanged();
        }

        public void RemoveFromShortlist(Itinerary itinerary)
        {
            shortlist.Remove(itinerary);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
