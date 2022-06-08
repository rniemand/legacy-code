namespace FtpSync.Enums
{
    public enum FtpFileChangeType
    {
        ChangedLocally,
        ChangedRemotely,
        MissingLocally,
        MissingRemotely,
        ModifiedLocally,
        ModifiedRemotely,
        NoChangesFound,
        Unconfirmed
    }
}
