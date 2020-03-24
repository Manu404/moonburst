namespace MoonBurst.ViewModel
{
    public interface IFactory<T>
    {
        T Build();
    }

    public interface IFactory<T, Y>
    {
        T Build(Y data);
    }

}