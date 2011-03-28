using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace BusTracker
{
	public class ScrollLabel : Control
	{
		private static Timer s_timer;
		private static ArrayList s_instances;
		private const int SCROLL_PIXELS = 1;
		private const int SCROLL_INTERVAL = 50;
		private const int EXTRA_SPACE = 30;
		private const int TEXT_Y_OFFSET = -2;
		//private static string s_suffix = ""; //"  •  ";

		private Font m_font;
		private int m_scrollPosition;
		private string m_text;
		private int m_textWidth;
		private bool m_needToScroll;
		private Brush m_fgBrush;
		private Brush m_bgBrush;
		private Bitmap m_bmp;

		public ScrollLabel()
		{
			if (s_timer == null)
			{
				s_instances = new ArrayList();
				s_timer = new Timer();
				s_timer.Interval = SCROLL_INTERVAL;
				s_timer.Enabled = true;
				s_timer.Tick += new EventHandler(s_timer_Tick);
			}
			s_instances.Add(this);

			m_text = "";
			m_bmp = new Bitmap(200, 40);
			m_scrollPosition = 0;
			m_fgBrush = new SolidBrush(this.ForeColor);
			m_bgBrush = new SolidBrush(this.BackColor);
			m_font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);

			base.Resize += new EventHandler(ScrollLabel_Resize);
		}
		
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (m_needToScroll)
			{
				int x1 = -m_scrollPosition;
				int x2 = -m_scrollPosition + m_textWidth + EXTRA_SPACE;

				e.Graphics.DrawImage(m_bmp, -m_scrollPosition, 0); 
			}
			else
			{
				e.Graphics.DrawImage(m_bmp, 0, 0); 
			}
		}


		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				m_bgBrush = new SolidBrush(value);
				base.BackColor = value;
			}
		}

		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				m_fgBrush = new SolidBrush(value);
				base.ForeColor = value;
			}
		}

		public override Font Font
		{
			get
			{
				return m_font;
			}
			set
			{
				m_font = value;
			}
		}

		public override string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (!m_text.Equals(value))
				{
					m_text = value;
					
					// Measure the width of the text
					Graphics g = this.CreateGraphics();
					m_textWidth = (int)g.MeasureString(m_text, m_font).Width;

					if (m_textWidth > this.Width)
					{
						m_bmp = new Bitmap(2 * (m_textWidth + EXTRA_SPACE), this.Height);
						g = Graphics.FromImage(m_bmp);

						g.FillRectangle(m_bgBrush, 0, 0, 2 * (m_textWidth + EXTRA_SPACE), this.Height);
						g.DrawString(m_text, m_font, m_fgBrush, 0, TEXT_Y_OFFSET); 
						g.DrawString(m_text, m_font, m_fgBrush, m_textWidth + EXTRA_SPACE, TEXT_Y_OFFSET); 

						this.Scrolling = true;
					}
					else
					{
						m_bmp = new Bitmap(this.Width, this.Height);
						g = Graphics.FromImage(m_bmp);

						g.FillRectangle(m_bgBrush, 0, 0, this.Width, this.Height);
						g.DrawString(m_text, m_font, m_fgBrush, 0, TEXT_Y_OFFSET); 
					
						this.Scrolling = false;
					}
				}
			}
		}

		public bool Scrolling
		{
			get { return m_needToScroll; }
			set
			{
				m_scrollPosition = 0;
				m_needToScroll = value;
				this.Invalidate();
			}
		}

		private void DoScroll()
		{
			if (m_needToScroll)
			{
				m_scrollPosition += SCROLL_PIXELS;
				if (m_scrollPosition > m_textWidth + EXTRA_SPACE)
				{
					m_scrollPosition = 0;
				}
				//this.Invalidate();
			}
			this.Invalidate();
		}

		private void ScrollLabel_Resize(object sender, EventArgs e)
		{
		}

		private static void s_timer_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < s_instances.Count; i++)
			{
				((ScrollLabel)s_instances[i]).DoScroll();
			}
		}
	}


}
