using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;


namespace SampleProject
{
	
	public class ListViewExtended : ListView
	{
		
		private ImageList StatusImageList = new ImageList();
		
        // khai báo 
		public ListViewExtended()
		{
			this.OwnerDraw = true;
		
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.DrawColumnHeader += Me_DrawColumnHeader;
            this.DrawItem += Me_DrawItem;
            this.DrawSubItem += Me_DrawSubItem;
           
            this.View = View.Details;
            this.BackColor = Color.White;
            this.ShowItemToolTips = true;
            this.FullRowSelect = true;
            this.HideSelection = true;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            this.Columns.Add("Filename", 225, HorizontalAlignment.Left);
			this.Columns.Add("Size", 80, HorizontalAlignment.Right);
            this.Columns.Add("Completed", 100, HorizontalAlignment.Right);
            this.Columns.Add("Progress", 300, HorizontalAlignment.Center);
            this.Columns.Add("Status", 125, HorizontalAlignment.Left);
           
            StatusImageList.ColorDepth = ColorDepth.Depth32Bit;
            StatusImageList.ImageSize = new Size(16, 16);
            StatusImageList.Images.Add("Initializing", global::My.Resources.Resources.StatusInitializing);
            StatusImageList.Images.Add("Downloading", global::My.Resources.Resources.StatusDownload);
            StatusImageList.Images.Add("Paused", global::My.Resources.Resources.StatusPaused);
            StatusImageList.Images.Add("Finished", global::My.Resources.Resources.StatusFinished);
            StatusImageList.Images.Add("Error", global::My.Resources.Resources.StatusError);
            this.SmallImageList = StatusImageList;

        }
		
		private void Me_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
		}
		
		private void Me_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			e.DrawDefault = false;
		}
		

		private void Me_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			
			if (((int) (e.ItemState) & (int) ListViewItemStates.Selected) != 0)
			{
				e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
			}

            //view ở cột 3

            if (e.ColumnIndex == 3)
            {
                //StringFormat sf = new StringFormat();
                //sf.Alignment = StringAlignment.Center;
                //e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                //int FillPercent = System.Convert.ToInt32(((double.Parse(e.SubItem.Text)) / 100) * (e.Bounds.Width - 2));
                //Brush brGradient = new LinearGradientBrush(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), Color.Green, Color.White, 270, true);
                //e.Graphics.FillRectangle(brGradient, e.Bounds.X + 1, e.Bounds.Y + 2, FillPercent, e.Bounds.Height - 3);
                //e.Graphics.DrawString(e.SubItem.Text + " %", this.Font, Brushes.Black, (float)(e.Bounds.X + ((double)e.Bounds.Width / 2)), e.Bounds.Y + 3, sf);
                //e.Graphics.DrawRectangle(Pens.LightGray, e.Bounds.X, e.Bounds.Y + 1, e.Bounds.Width - 1, e.Bounds.Height - 2);
            }
            else
            {
                e.DrawDefault = true;
            }
        }
		
		public void StartDownload(string URL, string LocalFilePath)
		{
			DownloadFileAsyncExtended wClient = new DownloadFileAsyncExtended();
			ListViewItem lvw = this.Items.Add(Path.GetFileName(LocalFilePath));

			lvw.SubItems.Add("0 Bytes"); 
			lvw.SubItems.Add("Initializing..."); 
			lvw.SubItems.Add("0 Bytes"); 
			lvw.SubItems.Add("0"); 
			
			lvw.ImageKey = "Initializing"; 
			lvw.Tag = wClient;
			
			// giao diện trong quá trình tải
			wClient.DownloadProgressChanged += DownloadProgressChanged;
			wClient.DownloadCompleted += DownloadCompleted;
			
			// IMPORTANT !!
			wClient.SynchronizingObject = this;
			
	        // chia nhỏ để tải
			wClient.ProgressUpdateFrequency = DownloadFileAsyncExtended.UpdateFrequency.Second;
			
		    // tải file
			wClient.DowloadFileAsync(URL, LocalFilePath, lvw);
			wClient = null;
		}


		//View tải khi đang tải
		private void DownloadProgressChanged(object sender, FileDownloadProgressChangedEventArgs e)
		{
			
			ListViewItem lvw = (ListViewItem) e.userToken;
			
			lvw.SubItems[1].Text = ConvertBytes(e.TotalBytesToReceive);
			lvw.SubItems[4].Text = "Downloading";
			lvw.SubItems[2].Text = ConvertBytes(e.BytesReceived);
			lvw.SubItems[3].Text = System.Convert.ToString(e.ProgressPercentage);
			lvw.ImageKey = "Downloading";
		}
		
		// View tải file khi hoàng thành
		private void DownloadCompleted(object sender, FileDownloadCompletedEventArgs e)
		{
			ListViewItem lvw = (ListViewItem) e.userToken;
			
			if (e.ErrorMessage != null) 
			{
				lvw.SubItems[4].Text = e.ErrorMessage.Message.ToString();
				lvw.ImageKey = "Error";
				lvw.Tag = null;
				
			}
			else if (e.Cancelled)
			{
				lvw.SubItems[4].Text = "Paused";
				lvw.ImageKey = "Paused";
				
			}
			else 
			{
				lvw.SubItems[4].Text = "Finished";
				lvw.ImageKey = "Finished";
				
				lvw.Tag = null;
			}
		}
		
		// view Bytes đã tải được
		private string ConvertBytes(long Bytes)
		{
			if (Bytes < 1024)
			{
				return "1 KB";
			}
			else
			{
				return string.Format("{0:#,#} KB", (double) Bytes / 1024);
			}
		}
	}
	
}
