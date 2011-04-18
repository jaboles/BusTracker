using System;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace BusTracker
{
	public class BusControl : Control
	{
		public static string[] StringConversions = new string[]
			{
				"Rd", "rd",
				"Sr-", "SR-",
				"Tc", "TC",
		};

		public BusControl()
		{
			m_mainFont = new Font(FontFamily.GenericSansSerif, MAX_FONT_SIZE, FontStyle.Regular);
			m_largeDestinationFont = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Regular);
			m_smallDestinationFont = new Font(FontFamily.GenericSansSerif, 7, FontStyle.Bold);
			/*m_fonts = new Font[MAX_FONT_SIZE];
			for (int i = 0; i < m_fonts.Length; i++)
			{
				m_fonts[i] = new Font(FontFamily.GenericSansSerif, i + 1, FontStyle.Regular);
			}*/

			m_routeNumberLabel = new Label();
			m_routeNumberLabel.Left = 0;
			m_routeNumberLabel.Top = 0;
			m_routeNumberLabel.Width = 40;
			m_routeNumberLabel.Height = (int)(m_mainFont.Size * 1.5);
			m_routeNumberLabel.Font = m_mainFont;
			m_routeNumberLabel.BackColor = ColorService.BACKGROUND;
			m_routeNumberLabel.ForeColor = ColorService.NORMAL_BUS;
			this.Controls.Add(m_routeNumberLabel);

			m_destinationLabel = new ScrollLabel();
			m_destinationLabel.Left = m_routeNumberLabel.Left + m_routeNumberLabel.Width;
			m_destinationLabel.Top = m_routeNumberLabel.Top + 2;
			m_destinationLabel.Width = 160;
			m_destinationLabel.Height = m_routeNumberLabel.Height - (m_destinationLabel.Top - m_routeNumberLabel.Top);
			m_destinationLabel.Font = m_mainFont;
			m_destinationLabel.BackColor = ColorService.BACKGROUND;
			m_destinationLabel.ForeColor = ColorService.NORMAL_BUS;

			m_departureLabel = new Label();
			m_departureLabel.Left = 198;
			m_departureLabel.Top = m_routeNumberLabel.Top;
			m_departureLabel.Width = 30;
			m_departureLabel.Height = m_routeNumberLabel.Height;
			m_departureLabel.Font = m_mainFont;
			m_departureLabel.BackColor = ColorService.BACKGROUND;
			m_departureLabel.ForeColor = ColorService.NORMAL_BUS;
			m_departureLabel.TextAlign = ContentAlignment.TopCenter;
			this.Controls.Add(m_departureLabel);

			this.Controls.Add(m_destinationLabel);

			this.Height = m_routeNumberLabel.Height;
			this.Width = m_departureLabel.Left + m_departureLabel.Width;
			this.BackColor = ColorService.BACKGROUND;
		} 

		public void UpdateBusInfo(BusInfo b)
		{
			string route = b.RouteNumber;
			if (route.Length > 3) route = route.Substring(0, 3);
			m_routeNumberLabel.Text = route;
			m_destinationLabel.Text = GetCanonicalDestinationText(b.Destination);
			if (b.IsDepartingNow)
			{
				m_departureLabel.Text = "!";
			}
			else
			{
				m_departureLabel.Text = b.MinutesToDeparture.ToString();
			}

			if (b.IsDepartingNow)
			{
				m_departureLabel.ForeColor = ColorService.DEPARTING_BUS;
			}
			else if (b.ArrivalStatus.ToLower().IndexOf("on time") >= 0)
			{
				m_departureLabel.ForeColor = ColorService.ON_TIME_BUS;
			}
			else if (b.ArrivalStatus.ToLower().IndexOf("early") >= 0)
			{
				m_departureLabel.ForeColor = ColorService.EARLY_BUS;
			}
			else if (b.ArrivalStatus.ToLower().IndexOf("delay") >= 0)
			{
				m_departureLabel.ForeColor = ColorService.DELAYED_BUS;
			}
			else if (b.ArrivalStatus.ToLower().IndexOf("scheduled departure") >= 0)
			{
				m_departureLabel.ForeColor = ColorService.UNKNOWN_BUS;
			}
			else
			{
				m_departureLabel.ForeColor = ColorService.NORMAL_BUS;
			}

			/*Graphics g = this.CreateGraphics();

			if (g.MeasureString(m_destinationLabel.Text, m_largeDestinationFont).Width > (m_destinationLabel.Width + 2))
			{
				m_destinationLabel.Font = m_smallDestinationFont;
				float textHeight = g.MeasureString(m_destinationLabel.Text, m_smallDestinationFont).Height;

				// Detect if wrapping occurs even with small text, and if it doesn't, fix
				// up the vertical alignment
				if (g.MeasureString(m_destinationLabel.Text, m_smallDestinationFont).Width > (m_destinationLabel.Width + 1))
				{
					m_destinationLabel.Top = m_routeNumberLabel.Top;
				}
				else
				{
					m_destinationLabel.Top = m_routeNumberLabel.Top + (int)(textHeight / 2);
				}
			}
			else*/
			{
				m_destinationLabel.Font = m_largeDestinationFont;
			}

			/*int fontIndex = m_fonts.Length - 1;
			while (g.MeasureString(m_destinationLabel.Text, m_fonts[fontIndex]).Width > (m_destinationLabel.Width + 2))
			{
				fontIndex--;
			}
			m_destinationLabel.Font = m_fonts[fontIndex];
			*/
			// If text was made smaller, fix up the vertical alignment.
			/*if (fontIndex != m_fonts.Length - 1)
			{
				float sizeDifference = g.MeasureString(m_routeNumberLabel.Text, m_mainFont).Height -
					g.MeasureString(m_destinationLabel.Text, m_fonts[fontIndex]).Height;

				m_destinationLabel.Top = m_routeNumberLabel.Top + (int)(sizeDifference / 2);
			}
			else
			{
				m_destinationLabel.Top = m_routeNumberLabel.Top;
			}

			*/
		}

		public void Clear()
		{
			m_routeNumberLabel.Text = "";
			m_destinationLabel.Text = "";
			m_departureLabel.Text = "";
		}

		public string GetCanonicalDestinationText(string s)
		{
			string text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
			for (int i = 0; i < StringConversions.Length; i += 2)
			{
				text = text.Replace(StringConversions[i], StringConversions[i + 1]);
			}
			return text;
		}

		private Label m_routeNumberLabel;
		private ScrollLabel m_destinationLabel;
		private Label m_departureLabel;
		private Font m_mainFont;
		private Font m_largeDestinationFont;
		private Font m_smallDestinationFont;

		private const int MAX_FONT_SIZE = 16;
	}
}
