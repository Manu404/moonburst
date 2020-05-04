namespace MoonBurst.Api.Client
{
    public interface IFactory<out T>
    {
        T Build();
    }

    public interface IFactory<out T, in Y>
    {
        T Build(Y data);
    }

}