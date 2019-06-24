using System;


namespace IQChess
{
	public sealed class TooManyInstanceException : Exception
	{
		public TooManyInstanceException(string message) : base(message) { }
	}

	public sealed class CannotLoadException : Exception { }

	public sealed class CannotUndoException : Exception { }

	public sealed class CannotRedoException : Exception { }

	public sealed class CannotCreateInstanceException : Exception
	{
		public CannotCreateInstanceException(string message) : base(message) { }
	}
}