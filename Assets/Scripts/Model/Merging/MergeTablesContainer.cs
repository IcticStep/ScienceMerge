using System;
using System.Collections.Generic;

namespace Model.Merging
{
    public class MergeTablesContainer
    {
        public MergeTablesContainer() => CreateStartTables();
        
        public event Action OnStateChanged;

        private const int StartTableAmount = 2;
        private readonly List<MergeTable> _mergeTables = new();
        
        public IReadOnlyList<MergeTable> MergeTables => _mergeTables;

        private void CreateStartTables()
        {
            for (var i = 0; i < StartTableAmount; i++)
                AddNewTable();
        }

        public void AddNewTable()
        {
            _mergeTables.Add(new MergeTable());
            OnStateChanged?.Invoke();
        }
    }
}