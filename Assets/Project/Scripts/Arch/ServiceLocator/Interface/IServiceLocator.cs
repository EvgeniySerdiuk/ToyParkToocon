public interface IServiceLocator<T> 
{
    TP Register<TP>(TP newService) where TP : T;
    void Unregister<TP>(TP newService) where TP : T;
    TP Get<TP>() where TP : T;
}
