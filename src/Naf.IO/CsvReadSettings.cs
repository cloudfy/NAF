using System.Text;

namespace Naf.IO;

public class CsvReadSettings
{
    public bool SkipFirstLine = false;
    public char FieldSplitter = ',';
    public char TextQualifier = '"';

    public CsvReadSettings()
    {

    }
    public Encoding Encoding = System.Text.Encoding.UTF8;
}
