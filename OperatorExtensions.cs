using System;

// Token: 0x02000123 RID: 291
public static class OperatorExtensions
{
	// Token: 0x0600070A RID: 1802 RVA: 0x0002383E File Offset: 0x00021A3E
	public static string String(this Operator op)
	{
		switch (op)
		{
		case Operator.Addition:
			return "+";
		case Operator.Subtraction:
			return "-";
		case Operator.Multiplication:
			return "X";
		case Operator.Division:
			return "÷";
		default:
			return "WHAT THE HECK DID YOU DO?!?!?!";
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00023875 File Offset: 0x00021A75
	public static int Length(this Operator op)
	{
		return 4;
	}
}
