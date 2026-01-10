using Microsoft.Extensions.Logging;

namespace KDC2Keybinder.Core.Utils
{
	public class FileLoggerProvider : ILoggerProvider
	{
		private readonly string filePath;

		public FileLoggerProvider(string filePath)
		{
			this.filePath = filePath;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new FileLogger(filePath, categoryName);
		}

		public void Dispose() { }
	}
}
