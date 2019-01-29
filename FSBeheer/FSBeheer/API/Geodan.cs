using System;
using System.Net.Http;

namespace FSBeheer.API
{
    public class Geodan : IDisposable
    {
        private readonly string _Key;

        public Geodan()
        {
            _Key = "6c4c63db-de9a-11e8-8aac-005056805b87";
        }

        public void Dispose() { }

        public string FindRoute(string addressFrom, string addressTo)
        {
            var coordsFrom = AddressToCoords(addressFrom);
            var coordsTo = AddressToCoords(addressTo);

            var url = String.Format("https://services.geodan.nl/routing/route?fromcoordx={0}&fromcoordy={1}&tocoordx={2}&tocoordy={3}&outputformat=json&key={4}",
                coordsFrom.X, coordsFrom.Y, coordsTo.X, coordsTo.Y, _Key);

            using (var client = new HttpClient())
            {
                var response = client.GetStringAsync(url).Result;
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                string km = json.features[0].properties.distance;
                if (double.TryParse(km, out double kmDouble))
                    return Math.Round(kmDouble, 2) + " km";
                else
                    return "Afstand onbekend";
            }
        }

        private Coordinates AddressToCoords(string address)
        {
            var url = String.Format("https://services.geodan.nl/geosearch/free?q={0}&q.opAND&servicekey={1}", address, _Key);

            using (var client = new HttpClient())
            {
                var response = client.GetStringAsync(url).Result;
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                string geom;
                try { geom = json.response.docs[0].geom; }
                catch (Exception) { return null; }
                string[] geomArr = geom.Split('(')[1].Split(')')[0].Split(' ');
                var coords = new Coordinates(geomArr[0], geomArr[1]);
                return coords;
            }
        }
    }

    public class Coordinates
    {
        public Coordinates(string X, string Y)
        {
            double.TryParse(X, out double paramX);
            this.X = paramX;
            double.TryParse(Y, out double paramY);
            this.Y = paramY;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
