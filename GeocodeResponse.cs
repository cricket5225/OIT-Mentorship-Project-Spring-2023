﻿namespace RestaurantReviewProgram.Models
{
    public class GeocodeResponse
    {
        /// <summary>List of geocode results, from JSON</summary>
        public List<GeocodeResult> Results { get; set; }
    }
}