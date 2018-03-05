public class CircularBuffer<T> {

    private T[] data;
    private int length;
    private int index = 0;


    public int Length
    {
        get { return length; }
    }


    public CircularBuffer(int length)
    {
        this.length = length;
        this.data = new T[this.length];
    }


    public void Add(T item)
    {
        this.data[this.index] = item;
        this.index = (this.index + 1) % this.length;
    }


    public T Get(int index)
    {
        return this.data[(this.index + index) % this.length];
    }

    public T[] ToArray()
    {
        T[] toReturn = new T[this.length];
        for (int i = 0; i < this.length; i++)
        {
            toReturn[i] = Get(i);
        }
        return toReturn;
    }

}
