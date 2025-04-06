namespace NewsPage.Enums
{
    public enum CampaignStatusEnum
    {
        PENDING,     // Campaign is waiting to start
        ACTIVE,      // Campaign is currently running
        COMPLETED,   // Campaign has finished successfully
        CANCELLED,   // Campaign was cancelled before completion
        EXPIRED      // Campaign time has ended without completion
    }
}
