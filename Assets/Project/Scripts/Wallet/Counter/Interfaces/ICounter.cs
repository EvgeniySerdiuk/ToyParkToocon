using System;

public interface ICounter 
{
    public int Count { get; }
    public event Action OnCountChangeEvent;
    public bool TryChangeCountByVal(int val);
}
