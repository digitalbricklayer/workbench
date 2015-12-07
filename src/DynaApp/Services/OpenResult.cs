namespace DynaApp.Services
{
    public class OpenResult
    {
        public OpenResult(OpenStatus theStatus, string theMessage = "")
        {
            this.Status = theStatus;
            this.Message = theMessage;
        }

        public OpenStatus Status { get; private set; }
        public string Message { get; private set; }

        public static OpenResult Success
        {
            get
            {
                return new OpenResult(OpenStatus.Success);
            }
        }
    }
}