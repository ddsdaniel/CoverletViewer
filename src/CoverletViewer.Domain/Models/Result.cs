﻿using CoverletViewer.Domain.Abstractions;

namespace CoverletViewer.Domain.Models
{
    public class Result : Indicator
    {
        private int _totalLines;
        private int _notCoveredLines;
        private int _coveredLines;

        public override int TotalLines => _totalLines;
        public override int NotCoveredLines => _notCoveredLines;
        public override int CoveredLines => _coveredLines;
        public string Name { get; private set; }
        public string FullPath { get; private set; }
        public CodeFile CodeFile { get; }

        public Result(string name, string fullPath)
            : this(name, fullPath, null)
        {
            
        }

        public Result(string name, string fullPath, CodeFile codeFile)
        {
            Name = name;
            FullPath = fullPath;
            CodeFile = codeFile;
        }

        public override string ToString() => Name;

        internal void Increment(CodeFile codeFile)
        {
            _totalLines += codeFile.TotalLines;
            _notCoveredLines += codeFile.NotCoveredLines;
            _coveredLines += codeFile.CoveredLines;
        }
    }
}
