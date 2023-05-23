﻿using System.Net.Sockets;

namespace RestaurantReviewProgram.Models
{
    public class Restaurant
    {
        // Attributes
        private string name;
        private Guid id;
        // Location Attributes
        private string address;
        private string city;
        private string state;
        private string zip;
        private double latitude;
        private double longitude;
        // Review attributes
        private int positiveReviews;
        private int negativeReviews;
        private string color;
        // Constructor
        public Restaurant(string name, string address, string city, string state, string zip)
        {
            this.name = name;
            this.address = address;
            this.city = city; 
            this.state = state; 
            this.zip = zip;
        }
        // Getters/Setters
        /// <summary>The restaurant's name</summary>
        /// <example>Local Woodfired Grill</example>
        public string Name
        {
            get { return name; }
        }
        /// <summary>A unique GUID to identify a restaurant</summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>Street address the restaurant is located at</summary>
        /// <example>5315 Windward Parkway</example>
        public string Address
        {
            get { return address; }
        }
        /// <summary>City the restaurant is located in</summary>
        /// <example>Alpharetta</example>
        public string City
        {
            get { return city; }
        }
        /// <summary>State the restaurant is located in</summary>
        /// <example>GA</example>
        public string State
        {
            get { return state; }
        }
        /// <summary>Postal code the restaurant is located in</summary>
        /// <example>30004</example>
        public string Zip
        {
            get { return zip; }
        }
        /// <summary>Restaurant's latitude</summary>
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        /// <summary>Restaurant's longitude</summary>
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        /// <summary>Count of restaurant's positive reviews</summary>
        public int PositiveReviews 
        {
            get { return positiveReviews; }
        }
        /// <summary>Count of restaurant's negative reviews</summary>
        public int NegativeReviews 
        {
            get { return negativeReviews; }
        }
        /// <summary>Evaluated when creating a marker; Green is majority positive reviews, red is majority negative, yellow is equal positive and negative reviews</summary>
        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        // Methods
        public void addReview(RestaurantReview restaurantReview)
        {
            if (restaurantReview.RevSentiment == 1) { positiveReviews++; }
            else if (restaurantReview.RevSentiment == 0) { negativeReviews++; }
        }
    }
}
