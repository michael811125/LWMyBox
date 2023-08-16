namespace MyBox
{
	public static class MyString
	{
		public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
		public static bool NotNullOrEmpty(this string str) => !string.IsNullOrEmpty(str);
	}
}