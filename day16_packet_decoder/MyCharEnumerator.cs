using System.Collections;

namespace day16_packet_decoder;

public class MyCharEnumerator : IEnumerator<char>
{
    private string? _str;
    private int _index;
    private char _current;
    
    public MyCharEnumerator(string? str)
    {
        _str = str ?? throw new ArgumentNullException(nameof(str));
        _index = -1;
    }
    
    public bool MoveNext()
    {
        if (_index < _str!.Length - 1)
        {
            _current = _str![++_index];
            return true;
        }
        _index = _str!.Length;
        return false;
    }

    public void Reset()
    {
        _index = -1;
        _current = char.MinValue;
    }

    public char Current
    {
        get
        {
            if (_index < 0) throw new InvalidOperationException("Enum not started. Call MoveNext()");
            if (_index >= _str!.Length) throw new InvalidOperationException("Enum ended");
            return _current;
        }
    }

    public int Index => _index;

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        Reset();
        _str = null;
    }
}