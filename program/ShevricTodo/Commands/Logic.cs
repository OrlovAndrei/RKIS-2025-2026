namespace TodoList.Commands;

public static class Logic<T> where T : class
{
	public async static Task<T?> ProcessQuantity(
		IEnumerable<T> items,
		Func<Task<T?>> ifTheQuantityIsZero,
		Func<Task<T?>> ifTheQuantityIsOne,
		Func<Task<T?>> ifTheQuantityIsMany)
	{
		switch (items.Count())
		{
			case 0:
				return await ifTheQuantityIsZero();
			case 1:
				return await ifTheQuantityIsOne();
			default:
				return await ifTheQuantityIsMany();
		}
	}
}