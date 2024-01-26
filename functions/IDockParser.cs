using System.Collections.Generic;
using System.IO;
using System.Linq;

public interface IDocParser
{
    IPdfTemplate Parse(Stream doc);
}

public record FormField(string Name);

public record IPdfTemplate(IEnumerable<FormField> Fields)
{
    public override string ToString()
    {
        return $"Fields: [{string.Join(", ", Fields.Select(f => f.Name))}]";
    }
}