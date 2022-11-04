namespace Database;

public class Table<TColumns>
{
    public Table()
    {
        LastId = 0;
        Rows = new List<Row<TColumns>>();
    }

    public IEnumerable<Row<TColumns>> Rows { get; private set; }

    public int LastId { get; private set; }

    public Table<TColumns> Create(TColumns columns)
    {
        var newLastId = LastId + 1;
        var newRows = new List<Row<TColumns>>(Rows) { new(newLastId, columns) };

        LastId = newLastId;
        Rows = newRows;

        return this;
    }

    public Table<TColumns> Update(Row<TColumns> row)
    {
        var newRows = new List<Row<TColumns>>(Rows);

        var rowIndex = newRows.FindIndex(r => r.Id == row.Id);
        if (rowIndex < 0) throw new DatabaseOperationException($"Failed to update table row with id {row.Id}");

        newRows.Insert(rowIndex, row);
        Rows = newRows;

        return this;
    }

    public Table<TColumns> Delete(Row<TColumns> row)
    {
        var newRows = new List<Row<TColumns>>(Rows);

        var rowIndex = newRows.FindIndex(r => r.Id == row.Id);
        if (rowIndex < 0) throw new DatabaseOperationException($"Failed to delete table row with id {row.Id}");

        newRows.RemoveAt(rowIndex);
        Rows = newRows;

        return this;
    }
}

public record Row<TColumns>(int Id, TColumns columns);

public class DatabaseOperationException : Exception
{
    internal DatabaseOperationException(string msg) : base(msg)
    {
    }
}