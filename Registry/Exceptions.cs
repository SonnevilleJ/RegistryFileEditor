using System;
using System.Runtime.Serialization;

namespace Sonneville.Registry
{
	/// <summary>
	/// Specifies that a file is not a valid RegistryFile.
	/// </summary>
	[Serializable]
	public class InvalidRegistryFileException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the Sonneville.Registry.InvalidRegistryFileException class.
		/// </summary>
		public InvalidRegistryFileException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the Sonneville.Registry.InvalidRegistryFileException class with a specified error message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public InvalidRegistryFileException(String message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Sonneville.Registry.InvalidRegistryFileException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public InvalidRegistryFileException(String message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.
		/// </summary>
		/// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
		protected InvalidRegistryFileException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
