namespace oSIP.Net
{
    public class TraceEvent
    {
        public TraceEvent(string fileName, int lineNumber, TraceLevel level, string message)
        {
            FileName = fileName;
            LineNumber = lineNumber;
            Level = level;
            Message = message;
        }

        public string FileName { get; }
        public int LineNumber { get; }
        public TraceLevel Level { get; }
        public string Message { get; }
    }
}