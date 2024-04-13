using System.Drawing.Imaging;

namespace HackatonBackend.Image
{
    public class MetadataConverter
    {
        public static string GetGpsInfo(PropertyItem propItem)
        {
            double degrees = BitConverter.ToUInt32(propItem.Value, 0) / BitConverter.ToUInt32(propItem.Value, 4);
            double minutes = BitConverter.ToUInt32(propItem.Value, 8) / BitConverter.ToUInt32(propItem.Value, 12);
            double seconds = BitConverter.ToUInt32(propItem.Value, 16) / BitConverter.ToUInt32(propItem.Value, 20);

            string directionRef = System.Text.Encoding.ASCII.GetString(propItem.Value, 4, 4).Trim();
            string latitudeRef = directionRef[0] == 'N' ? "North" : "South";

            string longitudeRef = System.Text.Encoding.ASCII.GetString(propItem.Value, 20, 4).Trim();
            string longitudeDir = longitudeRef[0] == 'E' ? "East" : "West";

            return $"{degrees}° {minutes}' {seconds}\" {latitudeRef}, {longitudeDir}";
        }
        public static string GetMetadata(string filePath)
        {
            string ret = "";
            try
            {
                using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    var image = System.Drawing.Image.FromStream(stream, false, false);

                    if (image.PropertyIdList != null)
                    {
                        foreach (int propertyId in image.PropertyIdList)
                        {
                            var propItem = image.GetPropertyItem(propertyId);

                            if (propertyId == 0x0004)
                            {
                                var gpsInfo = Image.MetadataConverter.GetGpsInfo(propItem);
                                ret += $"GPS Info: {gpsInfo}\n";
                            }
                            else if (propertyId == 0x013b)
                            {
                                var author = System.Text.Encoding.ASCII.GetString(propItem.Value);
                                ret += $"Author: {author}\n";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading metadata: {ex.Message}");
            }
            return ret;
        }

    }
}

