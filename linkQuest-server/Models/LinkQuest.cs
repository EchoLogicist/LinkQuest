using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linkQuest_server.Models
{
    public class LinkQuest
    {
        public Cell Left {get; set;} = new Cell();
        public Cell Right {get; set;} = new Cell();
        public Cell Top {get; set;} = new Cell();
        public Cell Bottom {get; set;} = new Cell();
        public string cellOwner {get; set;} = string.Empty;
    }

    public class Cell
    {
        public string UserName {get; set;} = string.Empty;
        public bool Checked {get; set;}

    }

    public class CellUpdate
    {
        public string Cell {get; set;} = string.Empty;
        public int RowIndex {get; set;}
        public int ColumnIndex {get; set;}
    }
}