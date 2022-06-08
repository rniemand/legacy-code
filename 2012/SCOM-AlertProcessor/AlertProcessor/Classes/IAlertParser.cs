namespace AlertProcessor.Classes
{
    public interface IAlertParser
    {
        bool ProcessAlert(ScomAlert alert);
    }
}
