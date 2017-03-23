using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Pvirtech.Framework.Controls
{
    public class PressKey
    {
        private Key _processKey;

		public char Display
		{
			get;
			private set;
		}

		public Key ProcessKey
		{
			get
			{
				return this._processKey;
			}
			set
			{
				this._processKey = value;
				this.Display = PhoneDevice.ProcessDisplayKey(value);
			}
		}

        public CustomButtonStates IsPress
		{
			get;
			set;
		}

		public PressKey()
		{
		}

		public PressKey(Key processKey, CustomButtonStates isPress)
		{
			this.ProcessKey = processKey;
			this.IsPress = isPress;
		}
	}
     
}
