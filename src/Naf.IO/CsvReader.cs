using Naf;

namespace Naf.IO;

/// <summary>
/// Provides a comma seperated data reader.
/// </summary>
public sealed class CsvReader
{
    private readonly string[] _lines;
    private int _lineNum;
    private readonly CsvReadSettings _settings;

    /// <summary>
    /// Returns a new instance of the reader using the provided filename using default settings.
    /// </summary>
    /// <param name="fileName">Filename of the csv file.</param>
    public CsvReader(string fileName) : this(fileName, new CsvReadSettings())
    {
        Argument.NotNull(fileName, "A filename must be provided");
    }

    /// <summary>
    /// Returns a new instance of the reader using the provided filename using specific settings.
    /// </summary>
    /// <param name="fileName">Filename of the csv file.</param>
    /// <param name="readSettings">Settings to open the reader.</param>
    public CsvReader(string fileName, CsvReadSettings readSettings)
    {
        _lines = System.IO.File.ReadAllLines(fileName, readSettings.Encoding);
        _settings = readSettings;
        _lineNum = 0;
        if (!readSettings.SkipFirstLine)
            _lineNum = -1;
    }

    #region === public properties ===
    /// <summary>
    /// Returns true whist reading if possible.
    /// </summary>
    /// <returns></returns>
    public bool Read()
    {
        if (_lineNum > (_lines.Length - 2))
            return false;

        _lineNum += 1;
        return true;
    }
    /// <summary>
    /// Returns the line of the current position.
    /// </summary>
    public string Line
    {
        get
        {
            return _lines[_lineNum];
        }
    }
    #endregion

    #region === public methods ===
    /// <summary>
    /// Reads a value from the field number.
    /// </summary>
    /// <param name="fieldNum"></param>
    /// <returns></returns>
    public string GetValue(int fieldNum)
    {
        string newLine = "";
        bool insideText = false;
        foreach (var c in Line)
        {
            if (c == _settings.TextQualifier)
                insideText = !insideText;
            if (c == _settings.FieldSplitter && !insideText)
            {
                newLine += "\t";
                continue;
            }
            newLine += c;
        }
        string[] values = newLine.Split('\t');
        return values[fieldNum];
    } 
    #endregion
}
