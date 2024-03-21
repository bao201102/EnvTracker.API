namespace EnvTracker.Application.Utilities
{
    public static class ExceptionExtensions
    {
        public static string GetExceptionMessage(this Exception ex)
        {
            var lsMessage = new List<string>();
            if (!string.IsNullOrEmpty(ex.Message))
            {
                lsMessage.Add("Exception Message: " + ex.Message);
            }

            if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
            {
                lsMessage.Add("\tInnerException Message: " + ex.InnerException.Message);
                if (ex.InnerException.InnerException != null
                    && !string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                {
                    lsMessage.Add("\tInnerException Message: " + ex.InnerException.InnerException.Message);
                    if (ex.InnerException.InnerException.InnerException != null
                    && !string.IsNullOrEmpty(ex.InnerException.InnerException.InnerException.Message))
                    {
                        lsMessage.Add("\tInnerException Message: " + ex.InnerException.InnerException.InnerException.Message);
                    }
                }
            }
            return string.Join("\r\n", lsMessage);
        }
    }
}
