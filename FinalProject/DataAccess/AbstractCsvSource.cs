namespace FinalProject.DataAccess;
using Domain;
using System.Collections;

public abstract class AbstractCsvSource<T>(string filePath) : ISource<T> where T : Product
{
    protected readonly string _filePath = filePath;
    public string FilePath() => _filePath;

    public IEnumerator<T> GetEnumerator() => new CsvEnumerator<T>(_filePath, Parse);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public T Parse(string line) => ParseArgs(line.Split(';'));
    
    protected abstract T ParseArgs(string[] args);
    public abstract object Clone();
    

    private class CsvEnumerator<TItem>(string path, Func<string, TItem> parseFunc) : IEnumerator<TItem>
    {
        private StreamReader? _reader;
        private string? _currentLine;

        public TItem Current => _currentLine != null ? parseFunc(_currentLine) : throw new InvalidOperationException();
        object IEnumerator.Current => Current!;

        public bool MoveNext()
        {
            _reader ??= new StreamReader(path);
            _currentLine = _reader.ReadLine();
            return _currentLine != null;
        }

        public void Reset() { _reader?.Dispose(); _reader = null; }
        public void Dispose() => _reader?.Dispose();
    }
}