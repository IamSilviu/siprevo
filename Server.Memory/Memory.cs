using System;
using System.Collections.Generic;

namespace Server.Memory
{
	public static class Memory<T>
		where T: IDefaultValue, new()
	{
		private static Stack<T> _items = new Stack<T>();

		public static T New()
		{
			T result = default(T);

			lock (_items)
			{
				if (_items.Count > 0)
				{
					result = _items.Pop();
				}
				else
				{
					result = new T();
					result.SetDefaultValue();
				}
			}

			return result;
		}

		public static void Free(T item)
		{
			item.SetDefaultValue();

			lock (_items)
				_items.Push(item);
		}

		public static void Free(ref T item)
		{
			Free(item);
			item = default(T);
		}
	}
}
