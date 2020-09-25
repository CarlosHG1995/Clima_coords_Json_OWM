using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Clima_coords_Json_OWM
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btn_recupera_Click(object sender, RoutedEventArgs e)
        {
            var permiso = await Geolocator.RequestAccessAsync();
            if (permiso != GeolocationAccessStatus.Allowed)
            {
                info.Text = "Sin permiso de localización o ubicación desactivada";
                return;
            }
            var geo = new Geolocator { DesiredAccuracyInMeters = 0 };
            var pos = await geo.GetGeopositionAsync();

            OpenWheaterMapProxy mitiempo = await OpenWheaterMapProxy.RecuperaTiempo(pos.Coordinate.Point.Position.Latitude,
             pos.Coordinate.Point.Position.Longitude);
            info.Text = "Ciudad "+ mitiempo.Name + " - " + mitiempo.Clouds 
                + " Latitud " + mitiempo.Coord.Lat + " Longitud " + mitiempo.Coord.Lon + " - " 
                + " Temperatura " + mitiempo.Main.Temp+"°C" + " Presión Atmos "+ mitiempo.Main.Pressure
                + " Humedad " + mitiempo.Main.Humidity + "Descripción del clima: "
                +  mitiempo.Weather[0].Description;

            string icon = "http://openweathermap.org/img/wn/" + mitiempo.Weather[0].Icon + "@2x.png";
            image.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));

            string icon1 = "ms-appx:///Assets/Weather/" + mitiempo.Weather[0].Icon + ".png";
            image1.Source = new BitmapImage(new Uri(icon1, UriKind.Absolute));

        }

        private async void btn_registra_tile_Click(object sender, RoutedEventArgs e)
        {
            var permiso = await Geolocator.RequestAccessAsync();
            if (permiso != GeolocationAccessStatus.Allowed)
            {
                info.Text = "Sin permiso de localización o ubicación desactivada";
                return;
            }
            var geo = new Geolocator { DesiredAccuracyInMeters = 0 };
            var pos = await geo.GetGeopositionAsync();
            double lat = pos.Coordinate.Point.Position.Latitude;
            double lon = pos.Coordinate.Point.Position.Longitude;

            string url = "http://servicioopenweather2.azurewebsites.net/?lat="+lat.ToString()+"&lon="+lon.ToString();
            url = url.Replace(",",".");
              
            Uri tileContent = new Uri(url);
            var actualizador = TileUpdateManager.CreateTileUpdaterForApplication();
            actualizador.StartPeriodicUpdate(tileContent, PeriodicUpdateRecurrence.HalfHour);


        }

        private async void  btn_consulta_ip_Click(object sender, RoutedEventArgs e)
        {
            //info.Text = "IP : " + txt_ip.Text; 
            using (var cliente = new HttpClient())
            {
                try
                {
                    if (txt_ip.Text == null)
                    {
                        info.Text = "Ip no válida, ingrese otra";
                    }
                    else
                    {
                        IpStackProxy data = await IpStackProxy.RecuperaTiempo(txt_ip.Text);

                        /*Task<string> task_jsontxt = cliente.GetStringAsync("http://api.ipstack.com/" + txt_ip.Text +
                        "?access_key=406badb299329af9ea3bdceffc3558d0&format=1");
                        info.Text = "Consultando info de la ip ingresada";
                        var jsontxt = await task_jsontxt;
                        var data = IpStackProxy.FromJson(jsontxt);*/
                        info.Text = "Consultando info de la ip ingresada";
                        info.Text = "IP : " + data.Ip + " tipo de ip " + data.Type + " Continente: "
                        + data.ContinentName + "\n País: " + data.CountryName + " \n Región del país: "
                        + data.RegionName + "\n Ciudad: " + data.City + "\n Coordenadas: latitud "
                        + data.Latitude + "° \n longitud : " + data.Longitude + "° \n Capital: "
                        + data.Location.Capital + "\n Idioma: " + data.Location.Languages[0].Name;


                        var icono = data.Location.CountryFlag;
                        image1.Source = new BitmapImage(new Uri(icono.AbsoluteUri, UriKind.Absolute));

                    }

                }
                catch (Exception ex)
                {
                    info.Text = "Error" + ex.Message;
                }

            }
        }
    }
}
