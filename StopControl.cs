using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace BusTracker
{
	/// <summary>
	/// Summary description for RouteControl.
	/// </summary>
	public class StopControl : Control
	{
		public StopControl(string stopName, int maxSlots)
		{
			this.Paint += new PaintEventHandler(StopControl_OnPaint);

			m_stopTextFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
			m_minutesFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);

			m_stopLabel = new Label();
			m_stopLabel.Text = stopName;
			m_stopLabel.Top = 0;
			m_stopLabel.Left = 5;
			m_stopLabel.Font = m_stopTextFont;
			m_stopLabel.ForeColor = ColorService.STOP_NAME;
			m_stopLabel.BackColor = ColorService.BACKGROUND;
			this.Controls.Add(m_stopLabel);

			m_minutesLabel = new Label();
			m_minutesLabel.Text = "mins.";
			m_minutesLabel.Font = m_minutesFont;
			m_minutesLabel.Top = 3;
			m_minutesLabel.Left = 210;
			m_minutesLabel.ForeColor = ColorService.SEPARATOR_LINE;
			m_minutesLabel.BackColor = ColorService.BACKGROUND;
			this.Controls.Add(m_minutesLabel);
			
			m_busControls = new ArrayList();

			this.BackColor = ColorService.BACKGROUND;

			ChangeSlotCount(maxSlots);
		}
		
		private void StopControl_OnPaint(object sender, PaintEventArgs e)
		{
			// Update the width of the stop label to match its text.
			SizeF textSize = e.Graphics.MeasureString(m_stopLabel.Text, m_stopLabel.Font);
			m_stopLabel.Width = (int)textSize.Width + 3;
			m_stopLabel.Height = (int)textSize.Height;
			m_minutesLabel.Height = m_stopLabel.Height;

			Pen p = new Pen(ColorService.SEPARATOR_LINE);
			// Line at bottom
			e.Graphics.DrawLine(p, 0, this.ClientRectangle.Height - 1, this.ClientRectangle.Width, this.ClientRectangle.Height - 1);
		}

		public void ChangeSlotCount(int newCount)
		{
			if (newCount > m_busControls.Count)
			{
				BusControl b = null;
				for (int i = m_busControls.Count; i < newCount; i++)
				{
					b = new BusControl();
					b.Top = i * b.Height + 13;
					b.Left = 3;
					m_busControls.Add(b);
					this.Controls.Add(b);
				}

				if (b != null)
				{
					this.Height = b.Height * newCount + 20;
					this.Width = (b.Left * 2) + b.Width;
				}
			}
			else if (newCount < m_busControls.Count)
			{
				int controlHeight = 0;
				for (int i = newCount; i < m_busControls.Count; i++)
				{
					BusControl b = (BusControl)m_busControls[0];
					controlHeight = b.Height;
					this.Controls.Remove(b);
					b.Dispose();
				}

				m_busControls.RemoveRange(newCount, m_busControls.Count - newCount);

				this.Height = controlHeight * newCount + 20;
			}
		}

		public void Refresh(BusInfo[] busInfo)
		{
			int controlCount = m_busControls.Count;
			if (busInfo.Length < controlCount)
			{
				controlCount = busInfo.Length;
			}
			for (int i = 0; i < controlCount; i++)
			{
				((BusControl)m_busControls[i]).UpdateBusInfo(busInfo[i]);
			}

			m_minutesLabel.Visible = (controlCount > 0);

			this.Invalidate();
		}

		private Label m_stopLabel;
		private Label m_minutesLabel;
		private ArrayList m_busControls;
		private Font m_stopTextFont;
		private Font m_minutesFont;
	}
}
