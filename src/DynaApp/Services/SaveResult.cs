namespace DynaApp.Services
{
    public class SaveResult
    {
        public SaveResult(SaveStatus theStatus, string theMessage = "")
        {
            this.Message = string.Empty;
            this.Status = theStatus;
            this.Message = theMessage;
        }

        public SaveStatus Status { get; private set; }
        public string Message { get; private set; }

        public static SaveResult Failure
        {
            get
            {
                return new SaveResult(SaveStatus.Failure);
            }
        }

        public static SaveResult Success
        {
            get
            {
                return new SaveResult(SaveStatus.Success);
            }
        }
    }
}