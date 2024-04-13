using Microsoft.OpenApi.Extensions;

namespace HackatonBackend.OpenStreetMapInterface
{
    public enum Engines
    {
        graphhopper_car,
        fossgis_osrm_car,
        fossgis_valhalla_car,
        graphhopper_bicycle,
        fossgis_osrm_bike,
        fossgis_valhalla_bicycle,
        graphhopper_foot,
        fossgis_osrm_foot,
        fossgis_valhalla_foot,
    }
    public static class OpenStreetMapLinkGenerator
    {
        private static string GenerateLink(double startX, double startY, double endX, double endY, Engines engine)
        {
            return $"https://www.openstreetmap.org/directions?engine={engine.GetDisplayName()}&route={startX}%2C{startY}%3B{endX}%2{endY}#map=13/49.1997/16.6118";
        }

        
    }
}
