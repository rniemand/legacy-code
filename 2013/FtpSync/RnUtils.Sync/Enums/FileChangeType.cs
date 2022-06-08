namespace RnUtils.Sync.Enums
{
    public enum FileChangeType
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
