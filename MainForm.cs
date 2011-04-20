using System;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Reflection;

namespace BusTracker
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private delegate void MethodInvoker();
		public const string AppName = "BusTracker";
		public const string ShortcutFolder = "\\Windows\\Start Menu\\Programs";
		public const string StartMenuShortcutFolder = "\\Windows\\Start Menu";
		public const string StartUpFolder = "\\Windows\\StartUp";

		public MainForm()
		{
			// Create shortcuts on startup if it doesn't exist.
			string exePath = Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
			string shortcutPath = Path.Combine(ShortcutFolder, string.Format("{0}.lnk", AppName));
			string startMenuShortcutPath = Path.Combine(StartMenuShortcutFolder, string.Format("{0}.lnk", AppName));
			string startUpFolderShortcutPath = Path.Combine(StartUpFolder, string.Format("{0}.lnk", AppName));
			string shortcutData = string.Format("{0}#\"{1}\"", exePath.Length, exePath);

			// Shortcut in start menu
			if (!File.Exists(shortcutPath) && !File.Exists(startMenuShortcutPath))
			{
				StreamWriter f = new StreamWriter(shortcutPath);
				f.WriteLine(shortcutData);
				f.Close();
			}
			if (!File.Exists(startUpFolderShortcutPath))
			{
				StreamWriter f = new StreamWriter(startUpFolderShortcutPath);
				f.WriteLine(shortcutData);
				f.Close();
			}

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_infoFetcher = new InfoFetcher();
			m_infoFetcher.BusInfoAvailable += new BusInfoAvailableEventHandler(m_infoFetcher_BusInfoAvailable);
			m_infoFetcher.BusInfoFetchComplete += new EventHandler(m_infoFetcher_BusInfoFetchComplete);
			m_infoFetcher.BusInfoSourceAvailabilityChanged += new EventHandler(m_infoFetcher_BusInfoSourceAvailabilityChanged);

			this.Menu = null;
			this.WindowState = FormWindowState.Maximized;

			m_stopControls = new StopControl[StopsToLoad.Stops.Length];
			for (int i = 0; i < m_stopControls.Length; i++)
			{
				m_stopControls[i] = new StopControl(StopsToLoad.Stops[i].FriendlyName, StopsToLoad.Stops[i].MaxBuses);
				m_stopControls[i].Left = 0;
				m_panel.Controls.Add(m_stopControls[i]);
			}

			m_panel.BackColor = ColorService.BACKGROUND;
			this.BackColor = ColorService.BACKGROUND;

			m_quitButton.BackColor = ColorService.BUTTON_BACKGROUND;
			m_quitButton.ForeColor = ColorService.BUTTON_FOREGROUND;
			m_quitButtonPanel.BackColor = m_quitButton.ForeColor;
			m_refreshButton.BackColor = ColorService.BUTTON_BACKGROUND;
			m_refreshButton.ForeColor = ColorService.BUTTON_FOREGROUND;
			m_refreshButtonPanel.BackColor = m_refreshButton.ForeColor;

			m_timeLabel.BackColor = ColorService.BACKGROUND;
			m_timeLabel.ForeColor = ColorService.TIME;
			m_timeLabel.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);

			m_rebootButton.ForeColor = Color.White;
			m_rebootButton.BackColor = Color.Red;

			// Load the image for when the tracker is offline
			System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BusTracker.offline.jpg");
			m_offlinePictureBox.Image = new Bitmap(stream);

			UpdateStopControlsLayout();
			RefreshInfo();
			m_updateTimer.Enabled = true;
			//ScreenOn();
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
			if (disposing)
				m_disposed = true;
		}

		public void UpdateStopControlsLayout()
		{
			for (int i = 0; i < m_stopControls.Length; i++)
			{
				if (i == 0)
				{
					m_stopControls[i].Top = 0;
				}
				else
				{
					m_stopControls[i].Top = m_stopControls[i - 1].Top + m_stopControls[i - 1].Height;
				}
			}
		}

		public void RefreshInfo()
		{
			m_infoFetcher.Start(StopsToLoad.Stops);
		}

		public void SetOffline(bool offline)
		{
			m_panel.Visible = !offline;
			m_offlinePictureBox.Visible = offline;
			m_rebootButton.Visible = offline;

			if (m_rebootTimer.Enabled != offline)
				m_rebootTimer.Enabled = offline;
		}

		private void m_infoFetcher_BusInfoAvailable(object sender, BusInfoAvailableEventArgs e)
		{
			if (!m_disposed)
			{
				m_fetchedInfoResult = e;
				Invoke(new EventHandler(BusInfoAvailable));
			}
		}

		private void BusInfoAvailable(object senderDUMMY, EventArgs eaDUMMY)
		{
			if (m_disposed) return;
			BusInfoAvailableEventArgs e = m_fetchedInfoResult;

			m_refreshing = true;

			// Don't include buses that left more than x minutes ago 
			ArrayList listToDisplay = new ArrayList(e.BusInfos.Count);
			for (int i = 0; i < e.BusInfos.Count; i++)
			{
				BusInfo b = (BusInfo)e.BusInfos[i];
				if (b.MinutesToDeparture >= e.LoadedStop.TimeThreshold)
				{
					listToDisplay.Add(b);
				}
			}
			
			// Convert to an array
			BusInfo[] infosAsArray = new BusInfo[listToDisplay.Count];
			for (int i = 0; i < listToDisplay.Count; i++)
			{
				infosAsArray[i] = (BusInfo)listToDisplay[i];
			}

			m_stopControls[e.StopIndex].Refresh(infosAsArray);

			m_refreshing = false;
		}
		private System.Windows.Forms.Panel m_panel;

		private StopControl[] m_stopControls;
		private System.Windows.Forms.Timer m_updateTimer;
		private System.Windows.Forms.Label m_timeLabel;
		private System.Windows.Forms.Panel m_refreshButtonPanel;
		private System.Windows.Forms.Panel m_quitButtonPanel;
		private System.Windows.Forms.Button m_quitButton;
		private System.Windows.Forms.Button m_refreshButton;
		private System.Windows.Forms.Button m_rebootButton;
		private System.Windows.Forms.Timer m_rebootTimer;
		private InfoFetcher m_infoFetcher;
		private bool m_refreshing;
		private bool m_disposed;
		private BusInfoAvailableEventArgs m_fetchedInfoResult;
		private System.Windows.Forms.PictureBox m_offlinePictureBox;
		private int m_backlightHandle;


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_panel = new System.Windows.Forms.Panel();
			this.m_offlinePictureBox = new System.Windows.Forms.PictureBox();
			this.m_updateTimer = new System.Windows.Forms.Timer();
			this.m_timeLabel = new System.Windows.Forms.Label();
			this.m_refreshButtonPanel = new System.Windows.Forms.Panel();
			this.m_refreshButton = new System.Windows.Forms.Button();
			this.m_quitButtonPanel = new System.Windows.Forms.Panel();
			this.m_quitButton = new System.Windows.Forms.Button();
			this.m_rebootButton = new System.Windows.Forms.Button();
			this.m_rebootTimer = new System.Windows.Forms.Timer();
			// 
			// m_panel
			// 
			this.m_panel.Size = new System.Drawing.Size(240, 296);
			// 
			// m_offlinePictureBox
			// 
			this.m_offlinePictureBox.Size = new System.Drawing.Size(240, 296);
			this.m_offlinePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.m_offlinePictureBox.Visible = false;
			// 
			// m_updateTimer
			// 
			this.m_updateTimer.Interval = 1000;
			this.m_updateTimer.Tick += new System.EventHandler(this.m_updateTimer_Tick);
			// 
			// m_timeLabel
			// 
			this.m_timeLabel.Location = new System.Drawing.Point(104, 299);
			this.m_timeLabel.Size = new System.Drawing.Size(80, 16);
			this.m_timeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// m_refreshButtonPanel
			// 
			this.m_refreshButtonPanel.Controls.Add(this.m_refreshButton);
			this.m_refreshButtonPanel.Location = new System.Drawing.Point(7, 296);
			this.m_refreshButtonPanel.Size = new System.Drawing.Size(90, 24);
			// 
			// m_refreshButton
			// 
			this.m_refreshButton.Location = new System.Drawing.Point(1, 1);
			this.m_refreshButton.Size = new System.Drawing.Size(88, 22);
			this.m_refreshButton.Text = "Force Refresh";
			this.m_refreshButton.Click += new System.EventHandler(this.m_refreshButton_Click);
			// 
			// m_quitButtonPanel
			// 
			this.m_quitButtonPanel.Controls.Add(this.m_quitButton);
			this.m_quitButtonPanel.Location = new System.Drawing.Point(190, 296);
			this.m_quitButtonPanel.Size = new System.Drawing.Size(42, 24);
			// 
			// m_quitButton
			// 
			this.m_quitButton.Location = new System.Drawing.Point(1, 1);
			this.m_quitButton.Size = new System.Drawing.Size(40, 22);
			this.m_quitButton.Text = "Quit";
			this.m_quitButton.Click += new System.EventHandler(this.m_quitButton_Click);
			// 
			// m_rebootButton
			// 
			this.m_rebootButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Bold);
			this.m_rebootButton.Location = new System.Drawing.Point(51, 248);
			this.m_rebootButton.Size = new System.Drawing.Size(144, 40);
			this.m_rebootButton.Text = "REBOOT";
			this.m_rebootButton.Visible = false;
			this.m_rebootButton.Click += new System.EventHandler(this.m_rebootButton_Click);
			// 
			// m_rebootTimer
			// 
			this.m_rebootTimer.Interval = 5000;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.Controls.Add(this.m_rebootButton);
			this.Controls.Add(this.m_timeLabel);
			this.Controls.Add(this.m_panel);
			this.Controls.Add(this.m_refreshButtonPanel);
			this.Controls.Add(this.m_quitButtonPanel);
			this.Controls.Add(this.m_offlinePictureBox);
			this.Text = "JB\'s Bus Tracker";

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void m_refreshButton_Click(object sender, System.EventArgs e)
		{
			FlashButton(m_refreshButton);
			m_refreshButton.Enabled = false;
			RefreshInfo();
		}

		private void m_quitButton_Click(object sender, System.EventArgs e)
		{
			FlashButton(m_quitButton);

			while (m_refreshing)
				System.Threading.Thread.Sleep(500);
			this.Dispose(true);

			m_infoFetcher.Stop();
		}

		private void m_updateTimer_Tick(object sender, System.EventArgs e)
		{
			//m_updateTimer.Enabled = false;

			m_timeLabel.Text = DateTime.Now.ToString("h:mm tt");
			if (DateTime.Now.Second == 0)
			{
				RefreshInfo();
			}
			/*if (DateTime.Now.Minute % 3 == 0)
			{
				if (m_screenOn)
					ScreenOff();
				else
					ScreenOn();
			}*/

			//m_updateTimer.Enabled = true;
		}

		private void ScreenOn()
		{
			m_backlightHandle = NativeMethods.SetPowerRequirement("BKL1:", NativeMethods.DevicePowerState.D0, 1,null, 0);
	}

		private void ScreenOff()
		{
			NativeMethods.ReleasePowerRequirement(m_backlightHandle);
		}

		private void FlashButton(Button b)
		{
			Color fc = b.ForeColor;
			Color bc = b.BackColor;
			int delay = 60;

			for (int i = 0; i < 3; i++)
			{
				b.BackColor = fc;
				b.ForeColor = bc;
				b.Refresh();
				System.Threading.Thread.Sleep(delay);
				b.BackColor = bc;
				b.ForeColor = fc;
				b.Refresh();
				System.Threading.Thread.Sleep(delay);
			}
		}

		private void m_infoFetcher_BusInfoFetchComplete(object sender, EventArgs e)
		{
			this.Invoke(new EventHandler(BusInfoFetchComplete));
		}

		private void BusInfoFetchComplete(object sender, EventArgs e)
		{
			m_refreshButton.Enabled = true;
		}

		private void m_infoFetcher_BusInfoSourceAvailabilityChanged(object sender, EventArgs e)
		{
			SetOffline(!m_infoFetcher.InfoSourceAvailable);
		}

		private void m_rebootButton_Click(object sender, System.EventArgs e)
		{
			HAL.Reset();
		}

		private void m_rebootTimer_Tick(object sender, EventArgs e)
		{
			HAL.Reset();
		}
	}
}
