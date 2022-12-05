namespace CBRE.Extended.Common.Mediator;

public interface IMediatorListener
{
    void Notify(Enum Message, object Data);
}