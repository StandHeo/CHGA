using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pvirtech.Framework.Controls
{
   public  class PhoneDevice
    {
        public static readonly List<Key> AccessKey;

		public static readonly List<char> CanInputChar;

		private static readonly Dictionary<char, List<Key>> KeyBoardMapping;

		public static readonly List<Key> DeleteKey; 
       
		static PhoneDevice()
		{
			PhoneDevice.AccessKey = new List<Key>
			{
				Key.D1,
				Key.D2,
				Key.D3,
				Key.D4,
				Key.D5,
				Key.D6,
				Key.D7,
				Key.D8,
				Key.D9,
				Key.D0,
				Key.NumPad0,
				Key.NumPad1,
				Key.NumPad2,
				Key.NumPad3,
				Key.NumPad4,
				Key.NumPad5,
				Key.NumPad6,
				Key.NumPad7,
				Key.NumPad8,
				Key.NumPad9,
				Key.Subtract,
				Key.OemMinus,
				Key.Back,
				Key.Delete,
				Key.Left,
				Key.Right,
				Key.Up,
				Key.Down
			};
			PhoneDevice.CanInputChar = new List<char>
			{
				'1',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'0',
				'-',
				'X'
			};
			PhoneDevice.KeyBoardMapping = new Dictionary<char, List<Key>>
			{
				{
					'1',
					new List<Key>
					{
						Key.D1,
						Key.NumPad1
					}
				},
				{
					'2',
					new List<Key>
					{
						Key.D2,
						Key.NumPad2
					}
				},
				{
					'3',
					new List<Key>
					{
						Key.D3,
						Key.NumPad3
					}
				},
				{
					'4',
					new List<Key>
					{
						Key.D4,
						Key.NumPad4
					}
				},
				{
					'5',
					new List<Key>
					{
						Key.D5,
						Key.NumPad5
					}
				},
				{
					'6',
					new List<Key>
					{
						Key.D6,
						Key.NumPad6
					}
				},
				{
					'7',
					new List<Key>
					{
						Key.D7,
						Key.NumPad7
					}
				},
				{
					'8',
					new List<Key>
					{
						Key.D8,
						Key.NumPad8
					}
				},
				{
					'9',
					new List<Key>
					{
						Key.D9,
						Key.NumPad9
					}
				},
				{
					'0',
					new List<Key>
					{
						Key.D0,
						Key.NumPad0
					}
				},
				{
					'-',
					new List<Key>
					{
						Key.Subtract,
						Key.OemMinus
					}
				},
				{
					'X',
					new List<Key>
					{
						Key.Delete,
						Key.Back
					}
				}
			};
			PhoneDevice.DeleteKey = new List<Key>
			{
				Key.Back,
				Key.Delete
			};
		}

		public static char ProcessDisplayKey(Key key)
		{
			var items = from mapping in PhoneDevice.KeyBoardMapping
			where mapping.Value.Contains(key)
			select mapping; 
			 
			return items.Select(o=>o.Key).FirstOrDefault<char>();
		}
	 
    }
}
