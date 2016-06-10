using System.Collections.Generic;

public interface IInventory<T>
{
	List<T> GetInventoryContents();

    bool TryAddItem(T itemToAdd);

	bool CanAddItem(T itemToAdd);
}
