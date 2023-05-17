namespace RestaurantReviewProgram.Models
{
    /* Yes this is bad code to have all in one file. It's only like this temporarily, so if I 
       decide to go a different route I can get comment/delete all this quickly. */
    public class GeoJSON
    {
        private string type {get; set;}
        private List<Feature> features { get; set; }
        public GeoJSON(Restaurant restaurant)
        {
            /* Need to set values for all nested classes, cannot be done at this high
               a level BUT feels silly to start at a low level and work up? */
        }
    }
    public class Feature
    {
        private string type { get; set; } // Should be set to "Feature"?
        private Geometry geometry { get; set; }
        private Properties properties { get; set; }
    }
    public class Geometry
    {
        private string type { get; set; }
        private double[] coordinates { get; set; }
    }
    public class Properties
    {
        private string name { get; set; }
        private string description { get; set; }
    }
}
/*  Example 
     {
        "type": "FeatureCollection",
        "features": [
          {
              "type": "Feature",
              "geometry": {
                  "type": "Point",
                  "coordinates": [-74.006393, 40.714172]
              },
              "properties": {
                  "name": "New York",
                  "description": "New York"
              }
          }
        ]
    }
*/