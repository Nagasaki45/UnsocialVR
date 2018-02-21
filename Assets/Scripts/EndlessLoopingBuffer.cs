// A looping buffer. Cyclic writing and cyclic backwards-fowards reading.
// Wrapper for existing arrays.
public class EndlessLoopingBuffer<T> {

    private T[] data;
    private int length;
    private int cursor = 0;
    private int top = 0;
    private int bottom = 0;
    private int step = 1;
    private bool writing = true;


    public EndlessLoopingBuffer(T[] _data)
    {
        data = _data;
        length = data.Length;
    }


    public void Write(T datum)
    {
        writing = true;
        data[cursor] = datum;
        top = cursor;
        cursor = (cursor + step + length) % length;
        bottom = cursor;
    }


    public T Read()
    {
        if (writing)
        {
            writing = false;
            cursor = top;
            step *= -1;
        }
        cursor = (cursor + step + length) % length;
        if (cursor == bottom || cursor == top)
        {
            step *= -1;
        }
        return data[cursor];
    }
}
