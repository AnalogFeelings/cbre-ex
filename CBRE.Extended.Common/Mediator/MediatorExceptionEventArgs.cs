namespace CBRE.Extended.Common.Mediator;

public class MediatorExceptionEventArgs : EventArgs 
{
    public Enum Message { get; set; }
    public Exception Exception { get; set; }

    public MediatorExceptionEventArgs(Enum Message, Exception Exception) 
    {
        this.Exception = Exception;
        this.Message = Message;
    }
}