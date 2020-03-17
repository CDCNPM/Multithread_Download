using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;


using System.IO;
using SampleProject;
using System.Drawing.Drawing2D;

namespace SampleProject
{
	
	public partial class FormMain
	{
		public FormMain()
		{
			InitializeComponent();
           
            //Added to support default instance behavour in C#
            if (defaultInstance == null)
				defaultInstance = this;
		}
		
		private static FormMain defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static FormMain Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new FormMain();
					defaultInstance.FormClosed += new System.Windows.Forms.FormClosedEventHandler(defaultInstance_FormClosed);
				}
				
				return defaultInstance;
			}
			set
			{
				defaultInstance = value;
			}
		}
		
		static void defaultInstance_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			defaultInstance = null;
		}
		
		
		public void FormMain_Load(System.Object sender, System.EventArgs e)
		{
	
			System.Net.ServicePointManager.DefaultConnectionLimit = 5;
           
		}
		
        // đóng form
		public void FormMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			DownloadFileAsyncExtended wClient = default(DownloadFileAsyncExtended);
			
			for (int i = 0; i <= ListViewEx.Items.Count - 1; i++)
			{
				if (ListViewEx.Items[i].Tag != null)
				{
					wClient = (DownloadFileAsyncExtended) (ListViewEx.Items[i].Tag);
					wClient.CancelAsync();
				}
			}
		}
		
		// mở form và bắt đầu tải file
		public void btnAddNewDownload_Click(System.Object sender, System.EventArgs e)
		{
			string strURL = "";
			string strFileName = "";
			string strSavePath = "";
			
			using (FormNewDownload frm = new FormNewDownload())
			{
				if (frm.ShowDialog() == DialogResult.OK)
				{
					strURL = frm.TextBoxURL.Text.Trim();
					strFileName = frm.TextBoxFilename.Text.Trim();
					strSavePath = frm.TextBoxBrowse.Text.Trim();
					ListViewEx.StartDownload(strURL, Path.Combine(strSavePath, strFileName));
				}
			}
			
		}
		
        // thay đổi chức năng khi đã chọn vào item
		public void ListViewEx_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Point objDrawingPoint = new Point();
			ListViewItem objListViewItem = default(ListViewItem);
			
			objDrawingPoint = ListViewEx.PointToClient(Cursor.Position);
			if (!ReferenceEquals(objDrawingPoint, null))
			{
				objListViewItem = ListViewEx.GetItemAt(objDrawingPoint.X, objDrawingPoint.Y);

				if (!ReferenceEquals(objListViewItem, null))
				{
					btnResume.Enabled = true;
					btnPause.Enabled = true;
					btnRemove.Enabled = true;
				}
				else 
				{
					btnResume.Enabled = false;
					btnPause.Enabled = false;
					btnRemove.Enabled = false;
				}
			}
		}
		
        // tải lại file đã chọn
		public void btnResume_Click(System.Object sender, System.EventArgs e)
		{
			DownloadFileAsyncExtended wClient = default(DownloadFileAsyncExtended);
			
			for (int i = 0; i <= ListViewEx.SelectedItems.Count - 1; i++)
			{
				if (ListViewEx.SelectedItems[i].Tag != null)
				{
					wClient = (DownloadFileAsyncExtended) (ListViewEx.SelectedItems[i].Tag);
					if (wClient.IsBusy == false)
					{
						wClient.ResumeAsync();
					}
				}
			}
		}
		
        // tải lại tất cả các file
		public void btnResumeAll_Click(System.Object sender, System.EventArgs e)
		{
			DownloadFileAsyncExtended wClient = default(DownloadFileAsyncExtended);	
			for (int i = 0; i <= ListViewEx.Items.Count - 1; i++)
			{
				if (ListViewEx.Items[i].Tag != null)
				{
					wClient = (DownloadFileAsyncExtended) (ListViewEx.Items[i].Tag);
					if (wClient.IsBusy == false)
					{
						wClient.ResumeAsync();
					}
				}
			}
		}
		
        // ngương tải file đã chọn
		public void btnPause_Click(System.Object sender, System.EventArgs e)
		{
			DownloadFileAsyncExtended wClient = default(DownloadFileAsyncExtended);
			
			for (int i = 0; i <= ListViewEx.SelectedItems.Count - 1; i++)
			{
				if (ListViewEx.SelectedItems[i].Tag != null)
				{
					wClient = (DownloadFileAsyncExtended) (ListViewEx.SelectedItems[i].Tag);
					wClient.CancelAsync();
				}
			}
		}
		
        // dừng tải các file
		public void btnPauseAll_Click(System.Object sender, System.EventArgs e)
		{
			DownloadFileAsyncExtended wClient = default(DownloadFileAsyncExtended);
			
			for (int i = 0; i <= ListViewEx.Items.Count - 1; i++)
			{
				if (ListViewEx.Items[i].Tag != null)
				{
					wClient = (DownloadFileAsyncExtended) (ListViewEx.Items[i].Tag);
					wClient.CancelAsync();
				}
			}
		}
		
        // xóa file đã chọn
		public void btnRemove_Click(System.Object sender, System.EventArgs e)
		{
			DownloadFileAsyncExtended wClient = default(DownloadFileAsyncExtended);
			for (int i = ListViewEx.SelectedItems.Count - 1; i >= 0; i--)
			{
				if (ListViewEx.SelectedItems[i].Tag != null)
				{
					wClient = (DownloadFileAsyncExtended) (ListViewEx.SelectedItems[i].Tag);
					wClient.CancelAsync();
					ListViewEx.SelectedItems[i].Tag = null;
					ListViewEx.SelectedItems[i].Remove();
				}
				else
				{
					ListViewEx.SelectedItems[i].Remove();
				}
			}
		}
		
        //xóa tất cả các file
		public void btnRemoveAll_Click(System.Object sender, System.EventArgs e)
		{
			DownloadFileAsyncExtended wClient = default(DownloadFileAsyncExtended);
			for (int i = ListViewEx.Items.Count - 1; i >= 0; i--)
			{
				if (ListViewEx.Items[i].Tag != null)
				{
					wClient = (DownloadFileAsyncExtended) (ListViewEx.Items[i].Tag);
					wClient.CancelAsync();
					ListViewEx.Items[i].Tag = null;
					ListViewEx.Items[i].Remove();
				}
				else
				{
					ListViewEx.Items[i].Remove();
				}
			}
		}

        private void ListViewEx_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // lấy dữ liệu từ Web
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (FormGetDataWeb frm = new FormGetDataWeb())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                   
                }
            }

        }
    }
	
}
