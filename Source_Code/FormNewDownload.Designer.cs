// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using SampleProject;

namespace SampleProject
{
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public 
	partial class FormNewDownload : System.Windows.Forms.Form
	{
		
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		
		private System.ComponentModel.Container components = null;
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.TextBoxURL = new System.Windows.Forms.TextBox();
            this.TextBoxFilename = new System.Windows.Forms.TextBox();
            this.TextBoxBrowse = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.FBDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(207, 230);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 28);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(313, 230);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // TextBoxURL
            // 
            this.TextBoxURL.Location = new System.Drawing.Point(16, 44);
            this.TextBoxURL.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextBoxURL.Name = "TextBoxURL";
            this.TextBoxURL.Size = new System.Drawing.Size(585, 22);
            this.TextBoxURL.TabIndex = 2;
            // 
            // TextBoxFilename
            // 
            this.TextBoxFilename.Location = new System.Drawing.Point(16, 112);
            this.TextBoxFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextBoxFilename.Name = "TextBoxFilename";
            this.TextBoxFilename.Size = new System.Drawing.Size(585, 22);
            this.TextBoxFilename.TabIndex = 3;
            // 
            // TextBoxBrowse
            // 
            this.TextBoxBrowse.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxBrowse.Location = new System.Drawing.Point(16, 180);
            this.TextBoxBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextBoxBrowse.Name = "TextBoxBrowse";
            this.TextBoxBrowse.ReadOnly = true;
            this.TextBoxBrowse.Size = new System.Drawing.Size(477, 22);
            this.TextBoxBrowse.TabIndex = 4;
            this.TextBoxBrowse.TextChanged += new System.EventHandler(this.TextBoxBrowse_TextChanged);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(16, 25);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(99, 17);
            this.Label1.TabIndex = 5;
            this.Label1.Text = "link Download:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(16, 92);
            this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(73, 17);
            this.Label2.TabIndex = 6;
            this.Label2.Text = "File name:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(16, 160);
            this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(65, 17);
            this.Label3.TabIndex = 7;
            this.Label3.Text = "Save To:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(503, 178);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 28);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // FormNewDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 313);
            this.ControlBox = false;
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.TextBoxBrowse);
            this.Controls.Add(this.TextBoxFilename);
            this.Controls.Add(this.TextBoxURL);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "FormNewDownload";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Link Download";
            this.Load += new System.EventHandler(this.FormAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.TextBox TextBoxURL;
		internal System.Windows.Forms.TextBox TextBoxFilename;
		internal System.Windows.Forms.TextBox TextBoxBrowse;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Button btnBrowse;
		internal System.Windows.Forms.FolderBrowserDialog FBDialog;
	}
	
}
