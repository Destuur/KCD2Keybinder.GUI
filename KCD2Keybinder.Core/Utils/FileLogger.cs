using Microsoft.Extensions.Logging;

namespace KDC2Keybinder.Core.Utils
{
	public class FileLogger : ILogger
	{
		private readonly string filePath;
		private readonly object logLock = new object();
		private readonly string categoryName;

		public FileLogger(string filePath, string categoryName)
		{
			this.filePath = filePath;
			this.categoryName = categoryName;

			var dir = Path.GetDirectoryName(this.filePath);
			if (!Directory.Exists(dir) && !string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

			File.WriteAllText(this.filePath, $"=== Log started at {DateTime.Now:yyyy-MM-dd HH:mm:ss} ==={Environment.NewLine}");
		}

		IDisposable ILogger.BeginScope<TState>(TState state) => null!;
		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(LogLevel logLevel, EventId eventId,
			TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			if (formatter == null) return;

			var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {categoryName}: {formatter(state, exception)}";

			lock (logLock)
			{
				File.AppendAllText(filePath, message + Environment.NewLine);
			}
		}
	}
}
