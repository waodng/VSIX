﻿using EnvDTE;

namespace FormatDocumentOnSave
{
    public interface IDocumentFormatter
    {
        void Format(Document document, IDocumentFilter filter, string command);
    }
}