namespace Domain.Entities.TaskEntity;

public class CompletionIndex
{
	public short Completion { get; }
	public static readonly CompletionIndex Max = new(_maxCompletionIndex);
	public static readonly CompletionIndex Min = new(_minCompletionIndex);
	public static readonly CompletionIndex Default = new(_defaultCompletionIndex);
	public const short _maxCompletionIndex = 10;
	private const short _minCompletionIndex = -10;
	public static readonly short _defaultCompletionIndex =
		Division(dividend: Sum(_maxCompletionIndex, _minCompletionIndex),
				 divider: 2);
	private CompletionIndex(short completion)
	{
		Completion = CheckCompletionWithException(completion);
	}
	public decimal Percentage() => Completion / _maxCompletionIndex * 100;
	private static short CheckCompletionWithException(short completion)
	{
		if (completion < _minCompletionIndex)
		{
			throw new Exception(message: $"Completion must not exceed the minimum value({_minCompletionIndex}).");
		}
		if (completion > _maxCompletionIndex)
		{
			throw new Exception(message: $"Completion must not exceed the maximum value({_maxCompletionIndex}).");
		}
		return completion;
	}
	private static short CheckCompletion(short completion)
	{
		if (completion > _maxCompletionIndex)
		{
			completion = _maxCompletionIndex;
		}
		else if (completion < _minCompletionIndex)
		{
			completion = _minCompletionIndex;
		}
		return completion;
	}
	private static short Division(short dividend, short divider) => checked((short)(dividend / divider));
	private static short Multiplication(short factor1, short factor2) => checked((short)(factor1 * factor2));
	private static short Sum(short value1, short value2) => checked((short)(value1 + value2));
	private static short Difference(short value1, short value2) => checked((short)(value1 - value2));
	public static CompletionIndex operator /(CompletionIndex left, short right) => new(
		completion: CheckCompletion(Division(left.Completion, right)));
	public static CompletionIndex operator *(CompletionIndex left, short right) => new(
		completion: CheckCompletion(Multiplication(left.Completion, right)));
	public static CompletionIndex operator +(CompletionIndex left, short right) => new(
		completion: CheckCompletion(Sum(left.Completion, right)));
	public static CompletionIndex operator -(CompletionIndex left, short right) => new(
		completion: CheckCompletion(Difference(left.Completion, right)));
}