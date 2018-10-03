using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using System.Runtime.Serialization;

namespace GoogleMaps
{
    public partial class Form1 : Form
    {
        GMapOverlay markers = new GMapOverlay("markers");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            map.MapProvider = GMapProviders.GoogleMap;
            double lat = Convert.ToDouble(textBox1.Text);
            double longt = Convert.ToDouble(textBox2.Text);
            map.Position = new PointLatLng(lat,longt);
            map.MinZoom = 2;
            map.MaxZoom = 15;
            map.Zoom = 15;

        }

      

        private void map_MouseDown(object sender, MouseEventArgs e)
        {
            double X = map.FromLocalToLatLng(e.X, e.Y).Lng;
            double Y = map.FromLocalToLatLng(e.X, e.Y).Lat;


            var bmp = new Bitmap(GoogleMaps.Properties.Resources.parking);


            GMapMarker marker = new GMarkerGoogle(new PointLatLng(Y, X), GMarkerGoogleType.red_dot);

           // var size = new Size(25,25);

           // marker.Size = size;
            
            marker.ToolTipText = "Emergency Request! \nTeller:Juan Delacruz \nTerminal:Entrance1";
            marker.ToolTipMode = MarkerTooltipMode.Always;


            marker.ToolTip.Fill = Brushes.GhostWhite;
            marker.ToolTip.Foreground = Brushes.Black;
            marker.ToolTip.Stroke = Pens.White;
            marker.ToolTip.TextPadding = new Size(10, 10);
            


            Font font = new Font("Tahoma",14);
            marker.ToolTip.Font = font;
            

            markers.Markers.Add(marker);
            map.Overlays.Add(markers);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PointLatLng start = new PointLatLng(14.676041, 121.043700);
            PointLatLng end = new PointLatLng(14.681922, 121.042110);
            MapRoute route = GMap.NET.MapProviders.GoogleMapProvider.Instance.GetRoute(
              start, end, false, false, 15);

            GMapRoute r = new GMapRoute(route.Points, "My route");

            GMapOverlay routesOverlay = new GMapOverlay("routes");
            routesOverlay.Routes.Add(r);
            map.Overlays.Add(routesOverlay);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            markers.Markers.Clear();
            map.Overlays.Clear();
        }
    }

    public class GmapMarkerWithLabel : GMapMarker, ISerializable
    {
        private Font font;
        private GMarkerGoogle innerMarker;

        public string Caption;

        public GmapMarkerWithLabel(PointLatLng p, string caption, GMarkerGoogleType type)
            : base(p)
        {
            font = new Font("Arial", 14);
            innerMarker = new GMarkerGoogle(p, type);

            Caption = caption;
        }

        public override void OnRender(Graphics g)
        {
            if (innerMarker != null)
            {
                innerMarker.OnRender(g);
            }

            g.DrawString(Caption, font, Brushes.Black,new PointF(0.0f, innerMarker.Size.Height));
        }

        public override void Dispose()
        {
            if (innerMarker != null)
            {
                innerMarker.Dispose();
                innerMarker = null;
            }

            base.Dispose();
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        protected GmapMarkerWithLabel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
