using System;
using System.Collections.Generic;

namespace Model
{
    public class MergeTablesContainer
    {
        public MergeTablesContainer() => CreateStartTables();
        
        public event Action OnStateChanged;
        public IReadOnlyList<MergeTable.MergeTable> MergeTables => _mergeTables;
        
        private const int StartTableAmount = 2;
        private readonly List<MergeTable.MergeTable> _mergeTables = new();

        private void CreateStartTables()
        {
            for (var i = 0; i < StartTableAmount; i++)
                AddNewTable();
        }

        public void AddNewTable()
        {
            _mergeTables.Add(new MergeTable.MergeTable());
            OnStateChanged?.Invoke();
        }
    }
}