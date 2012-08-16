using System;
using Base.Message;

namespace Http.Message
{
	public class HttpMessage
	{
		static HttpMessage()
		{
			BufferManager = new BufferManager();
		}

		public static IBufferManager BufferManager { get; set; }

		public static ContentType GetContentType(string fileName, byte[] content)
		{
			int point = fileName.LastIndexOf('.');
			if (point >= 0)
			{
				switch (fileName.Substring(point + 1).ToLower())
				{
					case "htm":
					case "html":
						return HasUtf8Bom(content) ? ContentType.TextHtmlUtf8 : ContentType.TextHtml;
					case "css":
						return ContentType.TextCss;
					case "txt":
						return ContentType.TextPlain;
					case "js":
						return ContentType.TextJavascript;
					case "gif":
						return ContentType.ImageGif;
					case "jpg":
					case "jpeg":
						return ContentType.ImageJpeg;
					case "png":
						return ContentType.ImagePng;
					case "tiff":
						return ContentType.ImageTiff;
				}
			}

			return ContentType.None;
		}

		public static bool HasUtf8Bom(byte[] bytes)
		{
			return bytes.Length > 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;
		}
	}

	public enum ContentType
	{
		None,
		TextHtmlUtf8,
		TextHtml,
		TextJavascript,
		TextPlain,
		TextXml,
		TextCss,
		ImageGif,
		ImageJpeg,
		ImagePng,
		ImageTiff,
		ApplicationJson,
		ApplicationXml,
		ApplicationJavascript,
	}
}
