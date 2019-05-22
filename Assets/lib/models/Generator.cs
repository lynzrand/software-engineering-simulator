

namespace Sesim.Models
{
    public interface IPickedGenerator<TRes, TArg>
    {
        float GetWeight(TArg c);

        TRes Generate(TArg C);
    }
}
