namespace TheCarHub.Core.Internal
{
    public interface ICarBase<TPictureUris> : ICarBase
    {
        TPictureUris PictureUris { get; }
    }
}