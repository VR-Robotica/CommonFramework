using System;

namespace SingletonPattern
{
	/// <summary>
	/// The 'Singleton' class
	/// </summary>
	class Singleton
	{
		// this field holds the singleton instance
		private static Singleton _instance = null;

		// this field holds the lock handle for thread locking
		private static object _handle = new object ();

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonPattern.Singleton"/> class.
		/// </summary>
		/// <remarks>Note that the constructor is protected which makes it inaccessible to clients.</remarks>
		protected Singleton()
		{
		}

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		/// <value>The instance of the singleton.</value>
		public static Singleton Instance
		{
			get
			{
				// thread-safe lazy initialization
				// remove this lock statement if your code is not multi-threaded
				lock (_handle)
				{
					if (_instance == null)
					{
						_instance = new Singleton ();
					}
				}

				return _instance;
			}
		}
	}
	
}
