using System.Globalization;
using System.Windows.Media;

namespace CustomTreeDrawerWPF.Drawers
{
	public class WPFTreeNodeDrawerSettings
	{
		public WPFTreeNodeDrawerSettings()
		{
		}

		public Brush NodeSelectedColor { get; set; }
		public Brush NodeColor { get; set; }
		public double LineThickness { get; set; }
		public Brush LineColor { get; set; }
		public int FontSize { get; set; }
		public FontFamily FontFamily { get; set; }
		public Brush FontColor { get; set; }
		public Brush BackgroundColor { get; set; }
		public CultureInfo FontCultureInfo { get; set; }
		public System.Windows.Point TextPositionCorrection { get; set; }

		public static WPFTreeNodeDrawerSettings CreateDefault()
		{
			var settings = new WPFTreeNodeDrawerSettings()
			{
				NodeSelectedColor = new SolidColorBrush(Color.FromArgb(120, 200, 200, 200)),
				NodeColor = new SolidColorBrush(Colors.Aqua),
				LineThickness = 0.8f,
				LineColor = new SolidColorBrush(Colors.Black),
				FontSize = 12,
				FontCultureInfo = new CultureInfo("en-us"),
				FontFamily = new FontFamily("Gil Sans MT"),
				FontColor = new SolidColorBrush(Colors.Black),
				TextPositionCorrection = new System.Windows.Point(6, 2),
				BackgroundColor = new SolidColorBrush(Colors.White)
			};

			settings.NodeSelectedColor.Freeze();
			settings.LineColor.Freeze();
			settings.NodeColor.Freeze();
			settings.FontColor.Freeze();
			settings.BackgroundColor.Freeze();
			return settings;
		}
	}
}
