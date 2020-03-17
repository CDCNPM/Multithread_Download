using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;


using System.IO;
using System.Net;
using System.Threading;
using SampleProject;


namespace SampleProject
{
	
	//// main tải file.
	public class DownloadFileAsyncExtended
	{
		
		private string _URL = string.Empty;
		private string _LocalFilePath = string.Empty;
		private object _userToken = null;
		private long _ContentLenght = 0;
		private int _TotalBytesReceived = 0;
		
		//// lớp tải file.
		public void DowloadFileAsync(string URL, string LocalFilePath, object userToken)
		{
			HttpWebRequest Request = default(HttpWebRequest);
			Uri fileURI = new Uri(URL); 
			
			if (fileURI.Scheme != Uri.UriSchemeHttp && fileURI.Scheme != Uri.UriSchemeHttps)
			{
				throw (new Exception("error!! URL"));
			}

            _URL = URL;
			_LocalFilePath = LocalFilePath;
			_userToken = userToken;
			
			//khỏi tạo một request
			Request = (HttpWebRequest) (HttpWebRequest.Create(new Uri(URL)));
			Request.Credentials = Credentials;
			Request.AllowAutoRedirect = true;
			Request.ReadWriteTimeout = 30000;
			Request.Proxy = Proxy;
			Request.KeepAlive = false;
			Request.Headers = Headers;
			
			// đi đến đường dẫn file nếu muốn tiếp tục tải
			if (_ResumeAsync)
			{
				FileInfo FileInfo = new FileInfo(LocalFilePath);
				if (FileInfo.Exists)
				{
					Request.AddRange(Convert.ToInt32(FileInfo.Length));
				}
			}
			
			_isbusy = true;
			_CancelAsync = false;
			
			HttpWebRequestState State = new HttpWebRequestState(LocalFilePath, Request, _ResumeAsync, userToken);
			IAsyncResult result = Request.BeginGetResponse(GetResponse_Callback, State);
			ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), State, 30000, true);
		}
		
		private void GetResponse_Callback(IAsyncResult result)
		{
			HttpWebRequestState State = (HttpWebRequestState) result.AsyncState;
			FileStream DestinationStream = null;
			HttpWebResponse Response = null;
			Stopwatch Duration = new Stopwatch();
			byte[] Buffer = new byte[8192];
			int BytesRead = 0;
			int ElapsedSeconds = 0;
			int DownloadSpeed = 0;
			int DownloadProgress = 0;
			int BytesReceivedThisSession = 0;
			
			try
			{
				Response = (HttpWebResponse) (State.Request.EndGetResponse(result));
				_ResponseHeaders = Response.Headers;
				
				// tai file lỗi
				if ((int) Response.StatusCode != System.Convert.ToInt32(HttpStatusCode.OK)& (int) Response.StatusCode != System.Convert.ToInt32(HttpStatusCode.PartialContent))
				{
					OnDownloadCompleted(new FileDownloadCompletedEventArgs(new Exception(Response.StatusCode.ToString()), false, State.userToken));
					return ;
				}
				
				// mở file để viết dữ liệu vào file
				if (State.ResumeDownload)
				{
					DestinationStream = new FileStream(State.LocalFilePath, FileMode.OpenOrCreate, FileAccess.Write);
				}

                // tạo file để viết dữ liệu vào file
                else
                {
					DestinationStream = new FileStream(State.LocalFilePath, FileMode.Create, FileAccess.Write);
					_ContentLenght = Response.ContentLength;
				}
				
				// di chuyển luồng đến đầu và cuối file tải
				DestinationStream.Seek(0, SeekOrigin.End);

				Duration.Start();
				
				using (Stream responseStream = Response.GetResponseStream())
				{
					do
					{
						// đọc các bytes dữ liệu
						BytesRead = responseStream.Read(Buffer, 0, Buffer.Length);
						if (BytesRead > 0)
						{
							DestinationStream.Write(Buffer, 0, BytesRead); //viết dữ liệu vào file 
							_TotalBytesReceived += BytesRead;
							BytesReceivedThisSession += BytesRead;// khi tải lại
							ElapsedSeconds = System.Convert.ToInt32(Duration.Elapsed.TotalSeconds);
                          
                            if (ProgressUpdateFrequency == UpdateFrequency.NoDelay)
							{
								if (ElapsedSeconds > 0)
								{
									DownloadSpeed = BytesReceivedThisSession / ElapsedSeconds;
								}
								OnDownloadProgressChanged(new FileDownloadProgressChangedEventArgs(_TotalBytesReceived, (int) _ContentLenght, ElapsedSeconds, DownloadSpeed, State.userToken));
							}
							else if (ProgressUpdateFrequency == UpdateFrequency.HalfSecond)
							{
								if (System.Convert.ToInt32(Duration.ElapsedMilliseconds - DownloadProgress) >= 500)
								{
									DownloadProgress = (int) Duration.ElapsedMilliseconds;
									if (ElapsedSeconds > 0)
									{
										DownloadSpeed = BytesReceivedThisSession / ElapsedSeconds;
									}
									OnDownloadProgressChanged(new FileDownloadProgressChangedEventArgs(_TotalBytesReceived, (int) _ContentLenght, ElapsedSeconds, DownloadSpeed, State.userToken));
								}
							}
							else if (ProgressUpdateFrequency == UpdateFrequency.Second)
							{
								if (System.Convert.ToInt32(Duration.ElapsedMilliseconds - DownloadProgress) >= 1000)
								{
									DownloadProgress = (int) Duration.ElapsedMilliseconds;
									if (ElapsedSeconds > 0)
									{
										DownloadSpeed = BytesReceivedThisSession / ElapsedSeconds;
									}
									OnDownloadProgressChanged(new FileDownloadProgressChangedEventArgs(_TotalBytesReceived, (int) _ContentLenght, ElapsedSeconds, DownloadSpeed, State.userToken));
								}
							}
							
							// thoát khỏi lòng lặp khi nhấn Pause
							if (_CancelAsync)
							{
								break;
							}
							
						}
					} while (!(BytesRead == 0));
				}
				
				OnDownloadProgressChanged(new FileDownloadProgressChangedEventArgs(_TotalBytesReceived, (int) _ContentLenght, System.Convert.ToInt32(Duration.Elapsed.TotalSeconds), DownloadSpeed, State.userToken));
				
                // khi nhận được thông báo dừng
				if (_CancelAsync)
				{
					OnDownloadCompleted(new FileDownloadCompletedEventArgs(null, true, State.userToken));
				}
				else
				{
					OnDownloadCompleted(new FileDownloadCompletedEventArgs(null, false, State.userToken));
				}
			}
			catch (Exception ex)
			{
				//// Send completed message (Error) to anyone who is listening.
				OnDownloadCompleted(new FileDownloadCompletedEventArgs(ex, false, State.userToken));
			}
			finally
			{
				//đóng file
				if (DestinationStream != null)
				{
					DestinationStream.Flush();
					DestinationStream.Close();
					DestinationStream = null;
				}
				Duration.Reset();
				Duration = null;
				_isbusy = false;
			}
		}
		
		// nếu quá mất kết nối trong thời gian cho phép thì dừng tải file
		private void TimeoutCallback(object State, bool TimedOut)
		{
			if (TimedOut)
			{
				HttpWebRequestState RequestState = (HttpWebRequestState) State;
				if (RequestState != null)
				{
                    // kết thúc tiến trình
					RequestState.Request.Abort();
				}
			}
		}
		
		// hàm đóng khi tai file
		private bool _CancelAsync = false;
		public void CancelAsync()
		{
			_CancelAsync = true;
		}
		
		// hàm tải lại  file
		private bool _ResumeAsync = false;
		public void ResumeAsync()
		{
			if (_isbusy)
			{
				throw (new Exception("erorr!!"));
			}
			
			if (string.IsNullOrEmpty(_URL) && string.IsNullOrEmpty(_LocalFilePath))
			{
				throw (new Exception("erorr!!"));
			}
			else
			{
				_ResumeAsync = true;
				DowloadFileAsync(_URL, _LocalFilePath, _userToken);
			}
		}
		
	
		public enum UpdateFrequency
		{
			NoDelay = 0,
			HalfSecond = 1,
			Second = 2
		}	
		public UpdateFrequency ProgressUpdateFrequency {get; set;}
		public IWebProxy Proxy {get; set;}
		public ICredentials Credentials {get; set;}
        public WebHeaderCollection Headers { get { return new WebHeaderCollection(); }}
        private bool _isbusy = false;
		public bool IsBusy { get { return _isbusy; } }
		private WebHeaderCollection _ResponseHeaders = null;
		public WebHeaderCollection ResponseHeaders { get { return _ResponseHeaders; } }
		
		private System.ComponentModel.ISynchronizeInvoke _synchronizingObject;
		public System.ComponentModel.ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				return this._synchronizingObject;
			}
			set
			{
				this._synchronizingObject = value;
			}
		}

        // có những thay đổi khi tải
        public event EventHandler<FileDownloadProgressChangedEventArgs> DownloadProgressChanged;
		private delegate void DownloadProgressChangedEventInvoker(FileDownloadProgressChangedEventArgs e);

		protected virtual void OnDownloadProgressChanged(FileDownloadProgressChangedEventArgs e)
		{
            if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
            {
                this.SynchronizingObject.Invoke(new DownloadProgressChangedEventInvoker(OnDownloadProgressChanged), new object[] { e });
            }
            else
            {
                DownloadProgressChanged(this, e);
               
            }
        }


        // tải xong
        public event EventHandler<FileDownloadCompletedEventArgs> DownloadCompleted;
		private delegate void DownloadCompletedEventInvoker(FileDownloadCompletedEventArgs e);

		protected virtual void OnDownloadCompleted(FileDownloadCompletedEventArgs e)
		{
			if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
			{
	
				this.SynchronizingObject.Invoke(new DownloadCompletedEventInvoker(OnDownloadCompleted), new object[] {e});
			}
			else
			{
                DownloadCompleted(this, e);
			}
		}
		
		
	}
	
	
	// khởi tạo phương thức HttpWebRequestState
	public class HttpWebRequestState
	{
		private string _LocalFilePath;
		private HttpWebRequest _Request;
		private bool _ResumeDownload;
		private object _userToken;
		
		public HttpWebRequestState(string LocalFilePath, HttpWebRequest Request, bool ResumeDownload, object userToken)
		{
			_LocalFilePath = LocalFilePath;
			_Request = Request;
			_ResumeDownload = ResumeDownload;
			_userToken = userToken;
		}
		
		public string LocalFilePath
		{
			get{ return _LocalFilePath;}
		}
		
		public HttpWebRequest Request
		{
            get{ return _Request;}
		}
		
		public bool ResumeDownload
		{
			get{ return _ResumeDownload;}
		}
		
		public object userToken
		{
			get{ return _userToken;}
		}
	}
	
	
	// khởi tạo phương thức ProgressEvent
	public class FileDownloadProgressChangedEventArgs : System.EventArgs
	{
		
		private int _BytesReceived;
		private int _TotalBytesToReceive;
		private int _DownloadTime;
		private long _DownloadSpeed;
		private object _userToken;
		
		public FileDownloadProgressChangedEventArgs(int BytesReceived, int TotalBytesToReceive, int DownloadTime, long DownloadSpeed, object userToken)
		{
			_BytesReceived = BytesReceived;
			_TotalBytesToReceive = TotalBytesToReceive;
			_DownloadTime = DownloadTime;
			_DownloadSpeed = DownloadSpeed;
			_userToken = userToken;
		}
		
		public int BytesReceived
		{
			get{ return _BytesReceived;}
		}
		
		public int TotalBytesToReceive
		{
			get{ return _TotalBytesToReceive;}
		}
		
		public int ProgressPercentage
		{
			get
			{
				if (_TotalBytesToReceive > 0)
				{
					return (int) (Math.Ceiling((decimal) (((double) _BytesReceived / _TotalBytesToReceive) * 100)));
				}
				else
				{
					return -1;
				}
			}
		}
		
		public int DownloadTimeSeconds
		{
			get{ return _DownloadTime;}
		}
		
		public int RemainingTimeSeconds
		{
			get
            {
				if (DownloadSpeedBytesPerSec > 0)
				{
					return (int) (Math.Ceiling((decimal) ((double) (_TotalBytesToReceive - _BytesReceived) / DownloadSpeedBytesPerSec)));
				}
				else
				{
					return 0;
				}
			}
		}
		
		public long DownloadSpeedBytesPerSec
		{
			get{ return _DownloadSpeed;}
		}
		
		public object userToken
		{
			get{ return _userToken;}
		}
	}
	
	
	// khởi tạo phương thúc CompletedEvent
	public class FileDownloadCompletedEventArgs : System.EventArgs
	{
		
		private Exception _ErrorMessage;
		private bool _Cancelled;
		private object _userToken;
		
		public FileDownloadCompletedEventArgs(Exception ErrorMessage, bool Cancelled, object userToken)
		{
			_ErrorMessage = ErrorMessage;
			_Cancelled = Cancelled;
			_userToken = userToken;
		}
		
		public Exception ErrorMessage
		{
			get{ return _ErrorMessage;}
		}
		
		public bool Cancelled
		{
			get{ return _Cancelled;}
		}
		
		public object userToken
		{
			get{ return _userToken;}
		}
	}
}
