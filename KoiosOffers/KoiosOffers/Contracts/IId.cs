namespace KoiosOffers.Contracts
{
    public interface IId<TId> where TId : struct
    {
        TId Id { get; set; }
    }
}
