using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace BTUpdater
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class BTUpdater : System.Windows.Forms.Form
	{

		public BTUpdater(string[] args)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Show();

			if (args.Length == 0)
			{
				Application.Exit();
			}

			// Get the current folder.
			string appPath = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
			string currentFolder = appPath.Substring(0, appPath.LastIndexOf("\\"));

			string binaryName = args[0];
			string shareName = args[1];

			string path = Path.Combine(shareName, binaryName);



			File.Copy(path, ".\\" + binaryName, true);

			System.Threading.Thread.Sleep(200);

			NativeMethods.ProcessInfo pi;
			byte[] si = new byte[128];
			bool success = NativeMethods.CreateProcess(Path.Combine(currentFolder, binaryName),
				null,
				IntPtr.Zero,
				IntPtr.Zero,
				false,
				0,
				IntPtr.Zero,
				currentFolder,
				si,
				out pi);


			Application.Exit();
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			// 
			// BTUpdater
			// 
			this.ClientSize = new System.Drawing.Size(178, 31);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "BTUpdater";

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static void Main(string[] args) 
		{
			Application.Run(new BTUpdater(args));
		}
	}
}
