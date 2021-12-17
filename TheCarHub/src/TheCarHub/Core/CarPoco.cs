using TheCarHub.Core.Internal;

namespace TheCarHub
{
    public class CarPoco : CarPoco<IReadOnlyList<string>>, ICar
    {
        public CarPoco()
        {
            SetPictureUrisIfNull();
        }

        public CarPoco(ICar car)
            : base(car)
        {
            SetPictureUrisIfNull();
        }

        public CarPoco(ICarBase car, IReadOnlyList<string> pictureUris)
            : base(car, pictureUris ?? new List<string>())
        { }

        private void SetPictureUrisIfNull()
        {
            if (this.PictureUris is null)
                this.PictureUris = new List<string>();
        }
    }
}