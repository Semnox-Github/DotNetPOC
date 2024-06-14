namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    ///  This class holds the BulkIssues configuarations
    /// </summary>
    public class BulkIssues
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool cardList;
        private bool cardRange;
        public BulkIssues()
        {
            log.LogMethodEntry();
            cardList = true;
            cardRange = true;
            log.LogMethodExit();
        }

        public BulkIssues(bool cardlist, bool cardRange)
        {
            log.LogMethodEntry(cardlist, cardRange);
            this.cardList = cardlist;
            this.cardRange = cardRange;
            log.LogMethodExit();
        }
        public bool list { get { return cardList; } set { cardList = value; } }
        public bool range { get { return cardRange; } set { cardRange = value; } }

    }
}
