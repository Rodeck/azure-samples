using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BitMiracle.Docotic.Pdf;

public class BitMiracleParser : IDocParser
{
    public IPdfTemplate Parse(Stream doc)
    {
        //convert stream to byte buffer
        var buffer = new byte[doc.Length];
        doc.Read(buffer, 0, buffer.Length);
        using var pdf = new PdfDocument(buffer);

        var controls = pdf.GetControls();

        return new IPdfTemplate(controls.Select(c => new FormField(c.Name)).ToArray());
    }
}